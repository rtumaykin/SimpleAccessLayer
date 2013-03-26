using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using System.Runtime.Serialization;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.SqlServer.Management.Common;
using System.Collections.Specialized;

namespace RomanTumaykin.SimpleDataAccessLayer.Transformation
{
	public class Helper
	{
		private DTE dte;
		private String templateFileName;

		public Helper (EnvDTE.DTE dte, String templateFileName)
		{
			this.dte = dte;
			this.templateFileName = templateFileName;
		}

		private ProjectItem templateFile;
		public ProjectItem TemplateFile
		{
			get
			{
				System.Threading.LazyInitializer.EnsureInitialized(
					ref templateFile,
					() =>
					{
						try
						{
							return (ProjectItem)dte.Solution.FindProjectItem(templateFileName);
						}
						catch
						{
							return (ProjectItem) null;
						}
					}
				);
				return this.templateFile;
			}
		}

		private DalConfig config;
		public DalConfig Config
		{
			get
			{
				System.Threading.LazyInitializer.EnsureInitialized(
					ref config,
					() =>
					{
						try
						{
							ProjectItem xmlProjectItem = TemplateFile.Collection.Parent;
							DataContractSerializer _ser = new DataContractSerializer(typeof(DalConfig));
							string _fileName = xmlProjectItem.get_FileNames(0);
							DalConfig _config = (DalConfig)_ser.ReadObject(XmlReader.Create(_fileName));
							return _config;
						}
						catch 
						{
							return new DalConfig();
						}
					}
				);
				return this.config;
			}
		}

		private string designerConnectionString;
		public string DesignerConnectionString
		{
			get
			{
				System.Threading.LazyInitializer.EnsureInitialized(
					ref designerConnectionString,
					() =>
					{
						try
						{
							Project _activeProject = TemplateFile.ContainingProject;

							string _configurationFilename = null;
							global::System.Configuration.Configuration _configuration = null;

							foreach (EnvDTE.ProjectItem _item in _activeProject.ProjectItems)
							{
								if (Regex.IsMatch(_item.Name, "(app|web).config", RegexOptions.IgnoreCase))
								{
									_configurationFilename = _item.get_FileNames(0);
									break;
								}
							}

							if (!string.IsNullOrEmpty(_configurationFilename))
							{
								// found it, map it and expose salient members as properties
								ExeConfigurationFileMap _configFile = null;
								_configFile = new ExeConfigurationFileMap();
								_configFile.ExeConfigFilename = _configurationFilename;
								_configuration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(_configFile, ConfigurationUserLevel.None);
								string _configConnectionString = _configuration.ConnectionStrings.ConnectionStrings[Config.ApplicationConnectionString].ConnectionString;
								SqlConnectionStringBuilder _sb = new SqlConnectionStringBuilder(_configConnectionString);
								if (Config.DesignerConnection.Authentication is WindowsAuthentication)
								{
									_sb.IntegratedSecurity = true;
								}
								else
								{
									SqlAuthentication _auth = Config.DesignerConnection.Authentication as SqlAuthentication;
									_sb.IntegratedSecurity = false;
									_sb.UserID = _auth.UserName;
									_sb.Password = _auth.Password;
								}
								return _sb.ConnectionString;

							}
							else
							{
								return (string)null;
							}
						}
						catch
						{
							return (string)null;
						}
					}
				);
				return this.designerConnectionString;
			}
		}

		public List<ProcedureParameter> GetProcedureParameterCollection(string objectSchemaName, string objectName)
		{
			string _fullObjectName = Tools.QuoteName(objectSchemaName) + "." + Tools.QuoteName(objectName);
			List<ProcedureParameter> _retValue = new List<ProcedureParameter>();

			using (SqlConnection _conn = new SqlConnection(DesignerConnectionString))
			{
				_conn.Open();

				using (SqlCommand _cmd = _conn.CreateCommand())
				{
					_cmd.CommandType = CommandType.StoredProcedure;
					_cmd.CommandText = "sp_executesql";

					string _stmt = @"
					SELECT 
						p.[name] AS ParameterName,
						p.[max_length] AS MaxByteLength,
						p.[precision] AS [Precision],
						p.[scale] AS Scale,
						p.[is_output] AS IsOutputParameter,
						ISNULL(ut.name, t.[name]) AS TypeName
					FROM sys.[parameters] p
						INNER JOIN sys.[types] t
							ON	t.[user_type_id] = p.[user_type_id]
						LEFT OUTER JOIN sys.[types] ut
							ON	ut.[user_type_id] = t.[system_type_id]
					WHERE p.[object_id] = OBJECT_ID(@ObjectName);
					";

					_cmd.Parameters.AddWithValue("@stmt", _stmt);
					_cmd.Parameters.AddWithValue("@params", "@ObjectName sysname");
					_cmd.Parameters.AddWithValue("@ObjectName", _fullObjectName);

					using (SqlDataReader _reader = _cmd.ExecuteReader())
					{
						while (_reader.Read())
						{
							// Remove @ from the beginning of the parameter
							string _parameterName = _reader.GetSqlString(0).Value.Substring(1);
							string _sqlTypeName = _reader.GetSqlString(5).Value;
							int _maxByteLength = _reader.GetSqlInt16(1).Value;
							byte _precision = _reader.GetSqlByte(2).Value;
							byte _scale = _reader.GetSqlByte(3).Value;
							bool _isOutputParameter = _reader.GetSqlBoolean(4).Value;

							string _clrTypeName = Tools.ClrTypeName(_sqlTypeName);

							if (string.IsNullOrWhiteSpace(_clrTypeName))
								continue; // later I will rewrite it 

							_retValue.Add(new ProcedureParameter(_parameterName, _maxByteLength, _precision, _scale, _isOutputParameter, _sqlTypeName));
						}
					}
				}
			}
			return _retValue;
		}

		public byte GetDatabaseCompatibilityLevel()
		{
			byte _retValue = 0;
			using (SqlConnection _conn = new SqlConnection(DesignerConnectionString))
			{
				_conn.Open();

				try
				{
					using (SqlCommand _cmd = _conn.CreateCommand())
					{
						_cmd.CommandType = CommandType.StoredProcedure;
						_cmd.CommandText = "sp_executesql";
						_cmd.Parameters.AddWithValue("@stmt", @"SELECT @CompatibilityLevel = [compatibility_level] FROM sys.[databases] WHERE [database_id] = DB_ID();");
						_cmd.Parameters.AddWithValue("@params", "@CompatibilityLevel tinyint OUTPUT");
						_cmd.Parameters.Add(new SqlParameter("@CompatibilityLevel", SqlDbType.TinyInt) { Direction = ParameterDirection.Output });
						_cmd.ExecuteNonQuery();
						_retValue = (byte)_cmd.Parameters["@CompatibilityLevel"].Value;
					}
				}
				catch // (Exception e)
				{
					_retValue = 0;
				}
			}
			return _retValue;
		}

		public List<List<ProcedureResultSetColumn>> GetProcedureResultSetColumnCollection(string objectSchemaName, string objectName)
		{
			// SQL 2012 still can use FMTONLY so this is only for the higher versions 
			if (GetDatabaseCompatibilityLevel() > 110)
			{
				return GetProcedureResultSetColumnCollection2014(objectSchemaName, objectName);
			}
			else
			{
				return GetProcedureResultSetColumnCollection2005(objectSchemaName, objectName);
			}
		}
		
		public List<List<ProcedureResultSetColumn>> GetProcedureResultSetColumnCollection2005(string objectSchemaName, string objectName)
		{
			string _fullObjectName = Tools.QuoteName(objectSchemaName) + "." + Tools.QuoteName(objectName);
			List<List<ProcedureResultSetColumn>> _retValue = new List<List<ProcedureResultSetColumn>>();
			try
			{
				SqlConnectionStringBuilder _sb = new SqlConnectionStringBuilder(DesignerConnectionString);
				ServerConnection _conn = _sb.IntegratedSecurity ? new ServerConnection(_sb.DataSource) : new ServerConnection(_sb.DataSource, _sb.UserID, _sb.Password);
				_conn.DatabaseName = _sb.InitialCatalog;
				string _procedureCall = "EXEC " + _fullObjectName;
				bool _isFirstParam = true;
				foreach (var _param in GetProcedureParameterCollection(objectSchemaName, objectName))
				{
					_procedureCall += (_isFirstParam ? " " : ", ") + "@" + _param.ParameterName + " = NULL";
					_isFirstParam = false;
				}

				DataSet[] _ds = _conn.ExecuteWithResults(new StringCollection() { "SET FMTONLY ON;", _procedureCall + ";" });
				foreach (DataTable _dt in _ds[1].Tables)
				{
					List<ProcedureResultSetColumn> _table = new List<ProcedureResultSetColumn>();
					foreach (DataColumn _column in _dt.Columns)
					{
						_table.Add(new ProcedureResultSetColumn(_column.ColumnName, _column.DataType.FullName));
					}
					_retValue.Add(_table);
				}
			}
			catch
			{
				// Whatever happens just don't return anything
				return new List<List<ProcedureResultSetColumn>>();
			}
			// GetProcedureParameterCollection
			return _retValue;
		}
		
		public List<List<ProcedureResultSetColumn>> GetProcedureResultSetColumnCollection2014(string objectSchemaName, string objectName)
		{
			List<ProcedureResultSetColumn> _firstRecordset = new List<ProcedureResultSetColumn>();
			string _fullObjectName = Tools.QuoteName(objectSchemaName) + "." + Tools.QuoteName(objectName);

			using (SqlConnection _conn = new SqlConnection(DesignerConnectionString))
			{
				_conn.Open();

				using (SqlCommand _cmd = _conn.CreateCommand())
				{
					_cmd.CommandType = CommandType.StoredProcedure;
					_cmd.CommandText = "sp_executesql";
					_cmd.Parameters.AddWithValue("@stmt", @"
				SELECT 
					c.[name] AS ColumnName,
					c.[precision],
					c.[scale],
					ISNULL(t.[name], tu.[name]) AS TypeName 
				FROM sys.[dm_exec_describe_first_result_set_for_object](OBJECT_ID(@ObjectFullName), 0) c
					LEFT OUTER JOIN sys.[types] t
						ON	t.[user_type_id] = c.[system_type_id]
					LEFT OUTER JOIN sys.[types] tu
						ON	tu.[user_type_id] = c.[user_type_id]
				WHERE c.name IS NOT NULL");
					_cmd.Parameters.AddWithValue("@params", "@ObjectFullName sysname");
					_cmd.Parameters.AddWithValue("@ObjectFullName", _fullObjectName);
					using (SqlDataReader _reader = _cmd.ExecuteReader())
					{
						while (_reader.Read())
						{
							_firstRecordset.Add(new ProcedureResultSetColumn(_reader.GetSqlString(0).Value, Tools.ClrTypeName(_reader.GetSqlString(3).Value)));

						}
					}
				}
			}

			List<List<ProcedureResultSetColumn>> _retValue = new List<List<ProcedureResultSetColumn>>();
			_retValue.Add(_firstRecordset);
			return _retValue;
		}

		public List<EnumData> GetEnumDataCollection(string objectSchemaName, string objectName, string valueColumnName, string keyColumnName)
		{
			List<EnumData> _retValue = new List<EnumData>();
			string _fullObjectName = Tools.QuoteName(objectSchemaName) + "." + Tools.QuoteName(objectName);

			using (SqlConnection _conn = new SqlConnection(DesignerConnectionString))
			{
				_conn.Open();
				using (SqlCommand _cmd = _conn.CreateCommand())
				{
					_cmd.CommandType = CommandType.StoredProcedure;
					_cmd.CommandText = "sp_executesql";

					_cmd.Parameters.Add(new SqlParameter("@stmt", String.Format("SELECT CONVERT(bigint, {0}) AS [Value], {1} AS [Key] FROM {2} ORDER BY {0}", valueColumnName, keyColumnName, _fullObjectName)));

					using (SqlDataReader _reader = _cmd.ExecuteReader())
					{
						while (_reader.Read())
						{
							_retValue.Add(new EnumData((string)_reader["Key"], (long)_reader["Value"]));
						}
					}
				}
			}
			return _retValue;
		}
	}
}
