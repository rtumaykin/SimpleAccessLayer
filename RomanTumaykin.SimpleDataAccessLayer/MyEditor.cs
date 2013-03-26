using System;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using VSLangProj;
using System.IO;
using System.Text;

namespace RomanTumaykin.SimpleDataAccessLayer
{
	public partial class MyEditor : UserControl
	{
		private const int GetOleInterfaceCommandId = 1084;
		private SimpleDataAccessLayerConfigFileEditorPackage package;
		private DalConfig config = null;
		private Dictionary<string, string> connectionStrings;
		string fileName = null;

		internal DalConfig Config
		{
			get
			{
				return config;
			}
		}

		public MyEditor()
		{
			//this.package = package;
			InitializeComponent();
		}

		private Dictionary<string, string> InitializeConectionStringsCollection(SimpleDataAccessLayerConfigFileEditorPackage package, string fileName)
		{
			EnvDTE.Project _project = package.GetEnvDTE().Solution.FindProjectItem(fileName).ContainingProject;

			string _configurationFilename = null;
			System.Configuration.Configuration _configuration = null;
			// examine each project item's filename looking for app.config or web.config
			foreach (EnvDTE.ProjectItem item in _project.ProjectItems)
			{
				if (Regex.IsMatch(item.Name, "(app|web).config", RegexOptions.IgnoreCase))
				{
					// TODO: try this with linked files. is the filename pointing to the source?
					_configurationFilename = item.get_FileNames(0);
					break;
				}
			}

			Dictionary<string, string> _returnValue = new Dictionary<string, string>();

			if (!string.IsNullOrEmpty(_configurationFilename))
			{
				// found it, map it and expose salient members as properties
				ExeConfigurationFileMap _configFile = null;
				_configFile = new ExeConfigurationFileMap();
				_configFile.ExeConfigFilename = _configurationFilename;
				_configuration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(_configFile, ConfigurationUserLevel.None);

				foreach (ConnectionStringSettings _connStringSettings in _configuration.ConnectionStrings.ConnectionStrings)
				{
					_returnValue.Add(_connStringSettings.Name, _connStringSettings.ConnectionString);
				}
			}

			return _returnValue;
		}

		private void Edit_Click(object sender, EventArgs e)
		{
			ModelDesigner _modelDesignerDialog = new ModelDesigner(this.connectionStrings, this.config);
			DialogResult _dialogResult = _modelDesignerDialog.ShowDialog();
			if (_dialogResult == DialogResult.OK)
			{
				this.config = _modelDesignerDialog.Config;
				String _connectionStringName = _modelDesignerDialog.ConnectionStringName;
				if (!connectionStrings.ContainsKey(_connectionStringName))
				{
					connectionStrings.Add(_connectionStringName, _modelDesignerDialog.ConnectionString);
					//AddConnectionStringToProject(_connectionStringName, _modelDesignerDialog.ConnectionString);
				}
			}

			InitControls();
		}

		internal void SaveConfig(string fileName)
		{
			DataContractSerializer _ser = new DataContractSerializer(typeof(DalConfig));
			XmlWriterSettings _settings = new XmlWriterSettings { Indent = true, Encoding=Encoding.Unicode };
			using (XmlWriter _writer = XmlWriter.Create(fileName, _settings))
			{
				_ser.WriteObject(_writer, this.config);
			}
				// by now the connection string is already in the collection
			AddConnectionStringToProject(config.ApplicationConnectionString, this.connectionStrings[config.ApplicationConnectionString]);
			EnvDTE.ProjectItem _dalProjectItem = package.GetEnvDTE().Solution.FindProjectItem(fileName);
			EnvDTE.ProjectItems _dalProjectItemChildren = _dalProjectItem.ProjectItems;
			foreach (EnvDTE.ProjectItem _item in _dalProjectItemChildren)
			{
				// there is only one child item with this extension
				if (_item.Name.ToUpper().EndsWith(".tt".ToUpper()))
				{
					VSProjectItem _pi = _item.Object as VSProjectItem;

					var _prop = _item.Properties.OfType<EnvDTE.Property>().FirstOrDefault(p => p.Name == "CustomTool");

					if (_prop != null && _pi != null)
						_pi.RunCustomTool();
				}
			}


		}

		private void AddConnectionStringToProject(string connectionStringName, string connectionString)
		{
			EnvDTE.Project _project = package.GetEnvDTE().Solution.FindProjectItem(fileName).ContainingProject;

			string _configurationFilename = null;
			System.Configuration.Configuration _configuration = null;
			// examine each project item's filename looking for app.config or web.config
			foreach (EnvDTE.ProjectItem _item in _project.ProjectItems)
			{
				if (Regex.IsMatch(_item.Name, "(app|web).config", RegexOptions.IgnoreCase))
				{
					// TODO: try this with linked files. is the filename pointing to the source?
					_configurationFilename = _item.get_FileNames(0);
					break;
				}
			}

			if (string.IsNullOrEmpty(_configurationFilename))
			{
				DialogResult _createConfigFileResult = MessageBox.Show("The configuration file for this project does not exist. A new app.config file will be created in the project root in order to save the connection strings. Please make sure you copy the connection strings to the application which will be using this project.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				_configurationFilename = Path.Combine(Path.GetDirectoryName(_project.FullName), "app.config");

				String[] _configText = new String[] { "<?xml version=\"1.0\" encoding=\"utf-8\" ?>", "<configuration>", "</configuration>" };
				File.WriteAllLines(_configurationFilename, _configText);
				_project.ProjectItems.AddFromFile(_configurationFilename);
			}
			// found it, map it and expose salient members as properties
			ExeConfigurationFileMap _configFile = null;
			_configFile = new ExeConfigurationFileMap();
			_configFile.ExeConfigFilename = _configurationFilename;
			_configuration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(_configFile, ConfigurationUserLevel.None);
			foreach (ConnectionStringSettings _connStringSettings in _configuration.ConnectionStrings.ConnectionStrings)
			{
				if (_connStringSettings.Name == connectionStringName)
					return;
			}
			_configuration.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(connectionStringName, connectionString));
			_configuration.Save();
		}

		public void Init(DalConfig config, string fileName, SimpleDataAccessLayerConfigFileEditorPackage package)
		{
			connectionStrings = InitializeConectionStringsCollection(package, fileName);
			this.package = package;
			this.config = config;
			this.fileName = fileName;

			if (config == null)
			{
				HandleInvalidFileFormat();
			}
			else
			{
				InitControls();
			}
		}

		private void InitControls()
		{
			ConnectionStringName.Text = this.config.ApplicationConnectionString;
			Namespace.Text = config.Namespace;

			if (!String.IsNullOrWhiteSpace(this.config.ApplicationConnectionString))
			{
				string _connectionString = connectionStrings[this.config.ApplicationConnectionString];
				SqlConnectionStringBuilder _sb = new SqlConnectionStringBuilder(_connectionString);

				Server.Text = _sb.DataSource;
				Database.Text = _sb.InitialCatalog;
			}
			EnumsGridView.Rows.Clear();
			ProcsGrid.Rows.Clear();

			foreach (Procedure _procedure in this.config.Procedures)
			{
				ProcsGrid.Rows.Add(new object[] { _procedure.Schema, _procedure.ProcedureName, _procedure.Alias });
			}

			foreach (Enum _enum in this.config.Enums)
			{
				EnumsGridView.Rows.Add(new object[] { _enum.Schema, _enum.TableName, _enum.Alias, _enum.KeyColumn, _enum.ValueColumn });
			}
		}

		void HandleInvalidFileFormat()
		{
			ConnectionStringName.Text = "";
			Server.Text = "";
			Database.Text = "";
			Namespace.Text = "";
			EnumsGridView.Rows.Clear();
			ProcsGrid.Rows.Clear();

			DialogResult _result = MessageBox.Show("Invalid file format. Ignore and continue? Warning! All data in the file will be overwritten!", "Invalid file format", MessageBoxButtons.YesNo);

			Edit.Enabled = false;
		}
	}
}
