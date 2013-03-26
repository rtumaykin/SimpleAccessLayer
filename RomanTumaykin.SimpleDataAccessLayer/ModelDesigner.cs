using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RomanTumaykin.SimpleDataAccessLayer
{
	public partial class ModelDesigner : Form
	{
		private Dictionary<string, string> connectionStrings;
		private DalConfig config;
		private int currentTabControlIndex = 0;

		internal String ConnectionStringName
		{
			get
			{
				return appConnectionTab.ConnectionStringName;
			}
		}

		internal String ConnectionString
		{
			get
			{
				// Here I will need to build a different connection string
				if (appConnectionTab.ConnectionStringChoice == ApplicationConnectionTab.ConnectionStringChoiceType.Existing)
				{
					return connectionStrings[appConnectionTab.ConnectionStringName];
				}
				else
				{
					SqlConnectionStringBuilder _sb = null;
					_sb = new SqlConnectionStringBuilder();
					_sb.DataSource = appConnectionTab.ServerName;
					_sb.IntegratedSecurity = appConnectionTab.WindowsAuthentication;

					if (!appConnectionTab.WindowsAuthentication)
					{
						_sb.UserID = appConnectionTab.Username;
						_sb.Password = appConnectionTab.Password;
					}

					_sb.InitialCatalog = databaseSelectionTab.SelectedDatabase;
					_sb.AsynchronousProcessing = appConnectionTab.AsynchronousProcessing;

					return _sb.ConnectionString;
				}


			}
		}
		internal DalConfig Config
		{
			get
			{
				return config;
			}
		}



		public ModelDesigner(Dictionary<string, string> connectionStrings, DalConfig config)
		{
			this.connectionStrings = connectionStrings;
			this.config = config;

			InitializeComponent();

			CustomizeComponent();

			WireUpEventHandlers();
		}

		private void CustomizeComponent()
		{
			this.Text = "Model Designer - " + tabContainer.SelectedTab.Text;
			appConnectionTab.SetStaticData(this.connectionStrings, this.config);
			designerInformationTab.SetStaticData(this.config);
			enumsTab.SetStaticData(this.config);
			proceduresTab.SetStaticData(this.config);
		}

		private void WireUpEventHandlers()
		{
			tabContainer.SelectedIndexChanged += VisibleTabChanged;
			
			appConnectionTab.CanContinueChanged += OnCanContinue;
			designerInformationTab.CanContinueChanged += OnCanContinue;
			databaseSelectionTab.CanContinueChanged += OnCanContinue;
			enumsTab.CanContinueChanged += OnCanContinue;
			proceduresTab.CanContinueChanged += OnCanContinue;
		}

		private void OnCanContinue(object o, CanContinueEventArgs e)
		{
			Control _ctl = o as Control;

			// Make sure the control that raised this event is a child of a currently selected tab
			if (_ctl != null && tabContainer.SelectedTab.Contains(_ctl))
				nextButton.Enabled = (tabContainer.SelectedIndex < tabContainer.TabCount - 1) && e.canContinue;
		}


		void VisibleTabChanged(object sender, EventArgs e)
		{
			previousButton.Enabled = tabContainer.SelectedIndex > 0;
			this.Text = "Model Designer - " + tabContainer.SelectedTab.Text;

			// do only when clicked on "Next"
			if (this.currentTabControlIndex < tabContainer.SelectedIndex)
			{
				switch (tabContainer.SelectedTab.Name)
				{
					case "ApplicationConnection":
						break;

					case "DesignerConnection":
						break;

					case "DatabaseSelection":
						UpdateDatabaseSelectionData();
						break;

					case "Enums":
						UpdateEnumsData();
						break;

					case "Procedures":
						UpdateProceduresData();
						break;
				}
			}

			this.currentTabControlIndex = tabContainer.SelectedIndex;
			finishButton.Enabled = tabContainer.SelectedIndex == tabContainer.TabCount - 1;
		}

		private void UpdateProceduresData()
		{
			proceduresTab.UpdateData(GetDesignerConnectionString());
		}

		private void UpdateEnumsData()
		{
			enumsTab.UpdateData(GetDesignerConnectionString());
		}

		private String GetDesignerConnectionString()
		{
			// Here I will need to build a different connection string
			SqlConnectionStringBuilder _sb = null;
			if (appConnectionTab.ConnectionStringChoice == ApplicationConnectionTab.ConnectionStringChoiceType.Existing)
			{
				_sb = new SqlConnectionStringBuilder(connectionStrings[appConnectionTab.ConnectionStringName]);
			}
			else
			{
				_sb = new SqlConnectionStringBuilder();
				_sb.DataSource = appConnectionTab.ServerName;
			}

			_sb.IntegratedSecurity = designerInformationTab.WindowsAuthentication;

			if (!designerInformationTab.WindowsAuthentication)
			{
				_sb.UserID = designerInformationTab.Username;
				_sb.Password = designerInformationTab.Password;
			}
			if (tabContainer.SelectedTab.Name != "DatabaseSelection")
			{
				_sb.InitialCatalog = databaseSelectionTab.SelectedDatabase;
			}

			return _sb.ConnectionString;
		}

		private void UpdateDatabaseSelectionData()
		{
			List<string> _databasesCollection = new List<string>();

			Cursor _savedCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			String _currentConnectionString = GetDesignerConnectionString();

			String _selectedDatabase = new SqlConnectionStringBuilder(_currentConnectionString).InitialCatalog;

			try
			{
				using (SqlConnection _conn = new SqlConnection(_currentConnectionString))
				{
					_conn.Open();
					using (SqlCommand _cmd = _conn.CreateCommand())
					{
						_cmd.CommandType = CommandType.StoredProcedure;
						_cmd.CommandText = "sp_executesql";
						_cmd.Parameters.AddWithValue("@stmt", "SELECT [name] FROM [sys].[databases];");
						using (var _reader = _cmd.ExecuteReader())
						{
							while (_reader.Read())
							{
								string _databaseName = (String)_reader["name"];
								if ((appConnectionTab.ConnectionStringChoice == ApplicationConnectionTab.ConnectionStringChoiceType.Existing && _selectedDatabase == _databaseName) || appConnectionTab.ConnectionStringChoice == ApplicationConnectionTab.ConnectionStringChoiceType.New)
									_databasesCollection.Add(_databaseName);
							}
						}

					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Connection Error");
			}
			finally
			{
				Cursor.Current = _savedCursor;
			}



			this.databaseSelectionTab.UpdateData(_databasesCollection, appConnectionTab.ConnectionStringChoice == ApplicationConnectionTab.ConnectionStringChoiceType.New);
		}

		private void NextButton_Click(object sender, EventArgs e)
		{
			if (tabContainer.SelectedIndex < tabContainer.TabCount - 1)
			{
				tabContainer.SelectedIndex++;
			}
		}

		private void PreviousButton_Click(object sender, EventArgs e)
		{
			if (tabContainer.SelectedIndex > 0)
			{
				tabContainer.SelectedIndex--;
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{

		}

		private void finishButton_Click(object sender, EventArgs e)
		{
			DalConfig _config = new DalConfig()
			{
				ApplicationConnectionString = appConnectionTab.ConnectionStringName,
				DesignerConnection = new DesignerConnection()
				{
					Authentication = designerInformationTab.WindowsAuthentication ? new WindowsAuthentication() as Authentication : new SqlAuthentication(designerInformationTab.Username, designerInformationTab.Password) as Authentication
				},
				Namespace = designerInformationTab.Namespace,
				Enums = enumsTab.SelectedEnums,
				Procedures = proceduresTab.SelectedProcedures
			};

			this.config = _config;
		}
	}
}
