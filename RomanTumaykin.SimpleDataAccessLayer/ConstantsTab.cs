using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace RomanTumaykin.SimpleDataAccessLayer
{

	public partial class ConstantsTab : UserControl
	{
		/// <summary>
		/// Class for columns data deserealization
		/// </summary>
		[CollectionDataContract(Namespace = "", Name = "Columns", ItemName = "Column")]
		private class Columns : List<string> { }

		/// <summary>
		/// Class to contain table hierarchy
		/// </summary>
		private class Table
		{
			internal int TableId { get; set; }
			internal string Schema { get; set; }
			internal string TableName { get; set; }
			internal string ValueColumn { get; set; }
			internal IList<string> KeyColumnNames { get; set; }
			internal int? ParentTableId { get; set; }
			internal string Alias { get; set; }
		}
		
		private bool _isLoading;
		private bool _isRowUpdating;
		private string _currentConnectionString;
		private DalConfig _dalConfig;
		private readonly Dictionary<string, Constant> configConstantsCollection = new Dictionary<string, Constant>();
		private Dictionary<int, Table> _tables; 

		internal List<Constant> SelectedConstants
		{
			get
			{
				List<Constant> constants = new List<Constant>();
				foreach (DataGridViewRow row in constantsGrid.Rows)
				{
					if ((bool)row.Cells["Generate"].Value)
					{
						constants.Add(new Constant()
							{
								Schema = (String)row.Cells["Schema"].Value,
								TableName = (String)row.Cells["TableName"].Value,
								KeyColumn = (String)row.Cells["KeyColumn"].Value,
								ValueColumn = (String)row.Cells["ValueColumn"].Value,
								Alias = (String)row.Cells["Alias"].Value
							});
					}
				}

				return constants;
			}
		}

		/// <summary>
		/// Declare the delegate that will be used to notify parent container
		/// </summary>
		/// <param name="o"></param>
		/// <param name="e"></param>
		public delegate void CanContinueHandler(object o, CanContinueEventArgs e);

		/// <summary>
		/// Declare the event
		/// </summary>
		public event CanContinueHandler CanContinueChanged;


		public ConstantsTab()
		{
			_isLoading = false;
			_isRowUpdating = false;
			_currentConnectionString = "";
			_dalConfig = null;
			_tables = new Dictionary<int, Table>();

			InitializeComponent();

			WireUpEventHandlers();
		}

		internal void SetStaticData (DalConfig dalConfig)
		{
			this._dalConfig = dalConfig;
			PrepareConfigConstantsCollection();
		}

		private void PrepareConfigConstantsCollection()
		{
			foreach (Constant constant in _dalConfig.Constants)
			{
				configConstantsCollection.Add(QuoteName(constant.Schema) + "." + QuoteName(constant.TableName), constant);
			}
		}

		internal void UpdateData(string connectionString)
		{
			bool reloadRequired = false;

			if (String.IsNullOrWhiteSpace(this._currentConnectionString))
			{
				reloadRequired = true;
			}
			else
			{
				SqlConnectionStringBuilder _sb = new SqlConnectionStringBuilder(connectionString);
				SqlConnectionStringBuilder _currentSb = new SqlConnectionStringBuilder(this._currentConnectionString);
				if (!(_sb.DataSource == _currentSb.DataSource && _sb.InitialCatalog == _currentSb.InitialCatalog))
				{
					reloadRequired = true;
				}
			}

			_currentConnectionString = connectionString;

			if (reloadRequired)
			{
				PopulateConstantsGrid();
			}
		}

		private void WireUpEventHandlers()
		{
			this.constantsGrid.CellValueChanged += ConstantsGrid_CellValueChanged;
			VisibleChanged += SetNextButtonState;
		}

		private void SetNextButtonState(object sender, EventArgs e)
		{
			if (CanContinueChanged != null)
				CanContinueChanged(this, new CanContinueEventArgs(true));
		}

		void ConstantsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			// this is happening within the same thread
			if (_isLoading)
				return;

			// This row is already updating 
			if (_isRowUpdating)
				return;

			// if none of the above let's start row updating;
			_isRowUpdating = true;
			try
			{
				DataGridViewRow row = constantsGrid.Rows[e.RowIndex];

				// this was a check box
				if (constantsGrid.Columns[e.ColumnIndex].Name == "Generate")
				{
					// if it was set to true, then need to make sure all columns are selected
					if ((bool)((DataGridViewCheckBoxCell)(row.Cells[e.ColumnIndex])).Value)
					{
						SetDefaultsForDropDownCells(row);
					}
					else
					{
						// remove all data from the row
						row.Cells["KeyColumn"].Value = row.Cells["ValueColumn"].Value = row.Cells["Alias"].Value = "";
					}
				}
				else
				{
					// Generate is already checked - do nothing
					if (((DataGridViewCheckBoxCell)(row.Cells["Generate"])).Value == null || (bool)((DataGridViewCheckBoxCell)(row.Cells["Generate"])).Value)
						return;
					else
					{
						SetDefaultsForDropDownCells(row);

						((DataGridViewCheckBoxCell)(row.Cells["Generate"])).Value = true;
					}
				}
			}
			finally
			{
				_isRowUpdating = false;
			}
		}

		private static void SetDefaultsForDropDownCells(DataGridViewRow row)
		{
			if (String.IsNullOrWhiteSpace((String)(row.Cells["KeyColumn"].Value)))
				row.Cells["KeyColumn"].Value = ((DataGridViewComboBoxCell)row.Cells["KeyColumn"]).Items[0];

//			if (String.IsNullOrWhiteSpace((String)(row.Cells["ValueColumn"].Value)))
//				row.Cells["ValueColumn"].Value = ((DataGridViewComboBoxCell)row.Cells["ValueColumn"]).Items[0];
		}

		private void PopulateConstantsGrid()
		{
			String _query = @"
				WITH Candidates AS (
					-- this gets the first seed set of rows
					-- only rows with single column primary keys are included
					SELECT 
						[i].[object_id] AS [TableId],
						[i].[index_id] AS [PK_IndexId],
						NULL AS [Parent_TableId],
						NULL AS [Parent_PK_IndexId]
					FROM [sys].[indexes] i
						INNER JOIN [sys].[index_columns] ic
							ON	[ic].[object_id] = [i].[object_id]
								AND [ic].[index_id] = [i].[index_id]
						CROSS APPLY(
							SELECT TOP 1 _c.[object_id]
							FROM sys.[columns] _c
								LEFT OUTER JOIN [sys].[index_columns] _ic
									ON	[_ic].[object_id] = [_c].[object_id]
										AND [_ic].[column_id] = [_c].[column_id]
										AND [_ic].[index_id] = i.[index_id]
							WHERE 
								_c.[object_id] = i.[object_id]
								AND _c.[system_type_id] IN (167, 175, 231, 239)
				--				AND _ic.[column_id] IS NULL
								-- at this point I don't need to make sure that the column is not a part of PK 
								-- since in the single column pk table it is allowed to have Key and Value the same
						) c
						INNER JOIN sys.[objects] o
							ON	[o].[object_id] = [i].[object_id]
						INNER JOIN [sys].[columns] col	
							ON	[col].[object_id] = [ic].[object_id]
								AND [col].[column_id] = [ic].[column_id]
					WHERE 
						[i].[is_primary_key] = 1
						AND [i].[is_disabled] = 0
						AND [o].[type] IN ('U', 'V')
						-- text or numeric
						AND [col].[system_type_id] IN (35, 36, 48, 52, 56, 59, 60, 62, 99, 104, 106, 108, 122, 127, 167, 175, 231, 239, 241, 231)
					GROUP BY 
						[i].[object_id],
						[i].[index_id]
					HAVING COUNT(*) = 1
					UNION ALL
					SELECT  [TableId],
							[PK_IndexId],
							[Parent_TableId],
							[Parent_PK_IndexId]
					FROM (
						SELECT 
							OBJECT_NAME(c.[object_id]) AS [TableName],
							OBJECT_NAME(p.[TableId]) AS [ParentTableName],
							[c].[object_id] AS [TableId],
							i.[index_id] AS [PK_IndexId],
							[p].[TableId] AS [Parent_TableId],
							[p].[PK_IndexId] AS [Parent_PK_IndexId],
							COUNT(*) OVER (PARTITION BY c.[object_id]) AS RowsPerIndex
						FROM (
								SELECT 
									c.[TableId],
									c.[PK_IndexId],	
									CONVERT(varbinary(MAX), 
										(
											SELECT CONVERT(varchar(max), CONVERT(binary(4), [column_id]), 2)
											FROM [sys].[index_columns] 
											WHERE 
												[object_id] = [c].[TableId]
												AND [index_id] = [c].[PK_IndexId]
											ORDER BY [column_id] ASC
											FOR XML PATH('')
										), 2
									) AS PK_Footprint
								FROM Candidates c
							) p
							INNER JOIN (
								SELECT  [fk].[parent_object_id] AS [object_id],
										[fk].[object_id] AS [foreign_key_id],
										[fk].[referenced_object_id],
										CONVERT(varbinary(MAX), 
											(
												SELECT CONVERT(varchar(max), CONVERT(binary(4), [parent_column_id]), 2)
												FROM [sys].[foreign_key_columns] 
												WHERE 
													[object_id] = [fk].[object_id]
													AND [parent_object_id] = [fk].[parent_object_id]
													AND [referenced_object_id] = [fk].[referenced_object_id]
												ORDER BY [parent_column_id] ASC
												FOR XML PATH('')
											), 2
										) AS footprint,
										CONVERT(varbinary(MAX), 
											(
												SELECT CONVERT(varchar(max), CONVERT(binary(4), [referenced_column_id]), 2)
												FROM [sys].[foreign_key_columns] 
												WHERE 
													[object_id] = [fk].[object_id]
													AND [parent_object_id] = [fk].[parent_object_id]
													AND [referenced_object_id] = [fk].[referenced_object_id]
												ORDER BY [referenced_column_id] ASC
												FOR XML PATH('')
											), 2
										) AS [referenced_object_footprint]
								FROM [sys].[foreign_keys] fk
							) c
								ON	c.[referenced_object_id] = p.[TableId]
									AND c.[referenced_object_footprint] = p.[PK_Footprint]
							INNER JOIN (
								SELECT
									i.[object_id],
									i.[index_id],	
									CONVERT(varbinary(MAX), 
										(
											SELECT CONVERT(varchar(max), CONVERT(binary(4), [column_id]), 2)
											FROM [sys].[index_columns] 
											WHERE 
												[object_id] = [i].[object_id]
												AND [index_id] = [i].[index_id]
											ORDER BY [column_id] ASC
											FOR XML PATH('')
										), 2
									) AS PK_Footprint
									FROM [sys].[indexes] i
									WHERE
										[i].[is_primary_key] = 1
										AND [i].[is_disabled] = 0
							) i
								ON	i.[object_id] = c.[object_id]
									AND SUBSTRING (i.[PK_Footprint], 1, LEN(i.[PK_Footprint]) - 4) = c.[footprint]
							-- make sure that the rightmost index column is a numeric or text value
							INNER JOIN sys.[columns] col
								ON	[col].[object_id] = [i].[object_id]
									AND col.[column_id] = CONVERT(int, SUBSTRING(i.[PK_Footprint], LEN(i.[PK_Footprint]) - 3, 4)) -- -3 here because position starts with 1
						WHERE col.[system_type_id] IN (35, 36, 48, 52, 56, 59, 60, 62, 99, 104, 106, 108, 122, 127, 167, 175, 231, 239, 241, 231)
					) ch
					WHERE [RowsPerIndex] = 1
				)
				SELECT  [TableId],
						OBJECT_SCHEMA_NAME([TableId]) AS [SchemaName],
						OBJECT_NAME([TableId]) AS [TableName],
						[Parent_TableId],
						[col].[name] AS Value_ColumnName,
						CONVERT(xml, (
							SELECT 
								_c.[column_id] AS ColumnId,
								_c.[name] AS ColumnName
							FROM [sys].[columns] _c
								LEFT OUTER JOIN [sys].[index_columns] _ic
										ON	_ic.[object_id] = _c.[object_id]
											AND _ic.[column_id] = _c.[column_id]
											AND _ic.[index_id] = c.[PK_IndexId]
							WHERE 
								_c.[object_id] = c.[TableId]
								AND _c.[system_type_id] IN (167, 175, 231, 239)
								AND _ic.[column_id] IS NULL
							FOR XML PATH('Column'), ROOT ('KeyColumns'), ELEMENTS
						)) AS KeyColumnsXml
				FROM (
					SELECT  c.[TableId],
							c.[PK_IndexId],
							c.[Parent_TableId],
							c.[Parent_PK_IndexId],
							COUNT(*) OVER (PARTITION BY [TableId]) AS CountPerTable
					FROM [Candidates] c
				) c
					CROSS APPLY (
						SELECT TOP 1 [column_id]
						FROM sys.[index_columns]
						WHERE
							[object_id] = c.[TableId]
							AND [index_id] = c.[PK_IndexId]
						ORDER BY [key_ordinal] DESC
					) ic
					INNER JOIN [sys].[columns] col
						ON	[col].[object_id] = c.[TableId]
							AND [col].[column_id] = ic.[column_id]
				WHERE [CountPerTable] = 1;";

			using (var conn = new SqlConnection(_currentConnectionString))
			{
				conn.Open();
				using (SqlCommand cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = "sys.sp_executesql";
					cmd.Parameters.AddWithValue("@stmt", _query);

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// since this happens only when connection server and database changes, I can wipe old items
						this.constantsGrid.Rows.Clear();

						this._isLoading = true;
						try
						{
							while (reader.Read())
							{
								AddRow(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1),
									   reader.GetFieldValue<string>(2), reader.GetFieldValue<string>(4),
									   reader.GetFieldValue<string>(5),
									   reader.IsDBNull(3) ? (int?) null : reader.GetFieldValue<int>(3));
							}
						}
						catch (Exception e)
						{
							throw;
						}
						finally
						{
							this._isLoading = false;
						}
					}
				}
			}
		}

		private void AddRow(int tableId, string schemaName, string tableName, string valueColumn, string keyColumnsXml, int? parentTableId)
		{
			// Add row to the dictionary of tables 

			var keysDataSet = new DataSet();
			keysDataSet.ReadXml(new StringReader(keyColumnsXml));
			var columns = (from DataRow dataRow in keysDataSet.Tables["Column"].Rows select (String)dataRow["ColumnName"]).ToList();

			var table = new Table()
				{
					TableId = tableId,
					Schema = schemaName,
					TableName = tableName,
					ValueColumn = valueColumn,
					KeyColumnNames = columns,
					ParentTableId = parentTableId,
					Alias = ""
				};

			_tables.Add(tableId, table);

			// Create new row and get the cell templates
			DataGridViewRow _row = this.constantsGrid.Rows[this.constantsGrid.Rows.Add()];
			DataGridViewTextBoxCell _sourceTableNameCell = (DataGridViewTextBoxCell)_row.Cells["SourceTableName"];
			DataGridViewComboBoxCell _keysCell = (DataGridViewComboBoxCell)_row.Cells["KeyColumn"];
			DataGridViewTextBoxCell _valuesCell = (DataGridViewTextBoxCell)_row.Cells["ValueColumn"];
			DataGridViewTextBoxCell _alias = (DataGridViewTextBoxCell)_row.Cells["Alias"];
			DataGridViewCheckBoxCell _generate = (DataGridViewCheckBoxCell)_row.Cells["Generate"];

			_sourceTableNameCell.Value = QuoteName(schemaName) + "." + QuoteName(tableName);
			foreach (var column in columns)
			{
				_keysCell.Items.Add(column);
			}
		   
			_valuesCell.Value = valueColumn;

			string _quotedName = QuoteName(schemaName) + "." + QuoteName(tableName);
			bool _isConstantInConfig = configConstantsCollection.ContainsKey(_quotedName);
			_alias.Value = _isConstantInConfig ? configConstantsCollection[_quotedName].Alias : "";
			_generate.Value = _isConstantInConfig;

			table.Alias = (string)_alias.Value;

			if (_isConstantInConfig)
			{
				_keysCell.Value = configConstantsCollection[_quotedName].KeyColumn;
			}
		}

		private string QuoteName (string name)
		{
			if (name == null)
				return null;
			else
				return ("[" + name.Replace("]", "]]") + "]");
		}
	}
}
