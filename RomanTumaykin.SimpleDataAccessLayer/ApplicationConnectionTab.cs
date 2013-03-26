using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;


namespace RomanTumaykin.SimpleDataAccessLayer
{
	public partial class ApplicationConnectionTab : UserControl
	{
		public enum ConnectionStringChoiceType
		{
			New,
			Existing
		}
#region Public Properties

		public ConnectionStringChoiceType ConnectionStringChoice
		{
			get
			{
				return connectionString_Existing.Checked ? ConnectionStringChoiceType.Existing : ConnectionStringChoiceType.New;
			}
		}
		public string ConnectionStringName
		{
			get
			{
				return connectionString_Existing.Checked ? existingConnectionStringComboBox.Text : newConnectionStringTextBox.Text;
			}
		}

		public string ServerName
		{
			get
			{
				return serverNameComboBox.Text;
			}
		}

		public bool WindowsAuthentication
		{
			get
			{
				return authenticationComboBox.SelectedIndex == 0;
			}
		}
		public string Username
		{
			get
			{
				return userNameTextBox.Text;
			}
		}
		public string Password
		{
			get
			{
				return passwordTextBox.Text;
			}
		}
		public bool AsynchronousProcessing
		{
			get
			{
				return asynchronousComboBox.SelectedIndex == 0;
			}
		}

#endregion

		private Dictionary<string, string> connectionStrings = null;
		private DalConfig dalConfig = null;

		private String savedUserNameText = "", savedPassword = "", savedSqlServerName = "", savedNewConnectionStringName = "";
		int savedAuthenticationIndex = 0, savedAsyncChoice = 0;
		private bool? savedCanContinue = null;

		/// <summary>
		/// Declare the delegate that will be used to notify parent container
		/// </summary>
		/// <param name="o"></param>
		/// <param name="e"></param>
		public delegate void CanContinueHandler (object o, CanContinueEventArgs e);

		/// <summary>
		/// Declare the event
		/// </summary>
		public event CanContinueHandler CanContinueChanged;
		private bool serverDropdownInitialized = false;

		/// <summary>
		/// Public constructor
		/// </summary>
		public ApplicationConnectionTab()
		{
			// This will create all child controls
			InitializeComponent();

			// Once child controls have been created, I can attach event handlers to them
			WireUpEventHandlers();
		}

		private void WireUpEventHandlers()
		{
			// Local handlers
			this.connectionString_Existing.CheckedChanged += OnChangeConnectionStringChoice;
			this.connectionString_New.CheckedChanged += OnChangeConnectionStringChoice;

			authenticationComboBox.SelectedIndexChanged += OnAuthenticationMethodChange;
			existingConnectionStringComboBox.SelectedIndexChanged += PopulateFieldsFromConnectionString;

			serverNameComboBox.DropDown += PopulateSqlServersDropdown;

			// notifications to the main form
			connectionString_Existing.CheckedChanged += SetNextButtonEnabledState;
			connectionString_New.CheckedChanged += SetNextButtonEnabledState;
			// Will need to disable "existing" if there is no connection strings in the config file
			// once I do so, if the "existing" option is selected, I don't need to check any further. Therefore no handler 
			// is necessary for the "existing name" dropdown.
			newConnectionStringTextBox.TextChanged += SetNextButtonEnabledState;
			serverNameComboBox.TextChanged += SetNextButtonEnabledState;
			authenticationComboBox.SelectedIndexChanged += SetNextButtonEnabledState;
			userNameTextBox.TextChanged += SetNextButtonEnabledState;
			passwordTextBox.TextChanged += SetNextButtonEnabledState;
			this.VisibleChanged += OnShow;
		}

		/// <summary>
		/// Sets the initial data - existing connection strings and current config
		/// </summary>
		/// <param name="package"></param>
		internal void SetStaticData (Dictionary<string, string> connectionStrings, DalConfig config)
		{
			// this is called after all components of this control are initialized, so it is safe to populate all data
			this.connectionStrings = connectionStrings;
			this.dalConfig = config;

			PopulateFormFields();
		}

		private void PopulateFormFields()
		{
			PopulateExistingConnectionStringDropdown();

			InitializeConnectionStringRadioButtonChoice();

		}

		private void PopulateExistingConnectionStringDropdown()
		{
			foreach (String _connectionString in connectionStrings.Keys.ToList<String>())
			{
				int _index = existingConnectionStringComboBox.Items.Add(_connectionString);

				if (this.dalConfig.ApplicationConnectionString == _connectionString)
				{
					existingConnectionStringComboBox.SelectedIndex = _index;
				}
			}
		}

		private void InitializeConnectionStringRadioButtonChoice()
		{
			if (existingConnectionStringComboBox.Items.Count > 0)
			{
				// this needs to be enabled first
				if (existingConnectionStringComboBox.SelectedIndex < 0)
				{
					existingConnectionStringComboBox.SelectedIndex = 0;
				}
				// this will fire the event
				connectionString_Existing.Checked = true;
			}
			else
			{
				connectionString_New.Checked = true;
				// disable since there is no connection string in the config file
				connectionString_Existing.Enabled = false;
			}
		}


		private void PopulateFieldsFromConnectionString(object sender, EventArgs e)
		{
			if (connectionString_Existing.Checked && existingConnectionStringComboBox.Enabled)
			{
				SqlConnectionStringBuilder _cb = new SqlConnectionStringBuilder(connectionStrings[existingConnectionStringComboBox.Text]);
				newConnectionStringTextBox.Text = existingConnectionStringComboBox.Text;
				authenticationComboBox.SelectedIndex = _cb.IntegratedSecurity ? 0 : 1;
				serverNameComboBox.Text = _cb.DataSource;
				userNameLabel.Text = _cb.IntegratedSecurity ? "User Name" : "Login";
				userNameTextBox.Text = _cb.IntegratedSecurity ? "" : _cb.UserID;
				passwordTextBox.Text = _cb.IntegratedSecurity ? "" : _cb.Password;
				asynchronousComboBox.SelectedIndex = _cb.AsynchronousProcessing ? 0 : 1;
			}
		}

		private void OnShow(object sender, EventArgs e)
		{
			if (!this.DesignMode && this.Visible)
			{
				savedCanContinue = null;
				SetNextButtonEnabledState(sender, e);
			}
		}


		private void PopulateSqlServersDropdown(object sender, EventArgs e)
		{
			if (!serverDropdownInitialized)
			{

				// capture what's now typed in the box
				string _typedServerName = serverNameComboBox.Text;

				Cursor _currentCursor = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;

				var _serverList = SmoApplication.EnumAvailableSqlServers();

				if (_serverList.Rows.Count > 0)
				{
					// Load server names into combo box
					foreach (DataRow dr in _serverList.Rows)
					{
						//only add if it doesn't exist
						if (serverNameComboBox.FindStringExact((String)dr["Name"]) == -1)
							serverNameComboBox.Items.Add(dr["Name"]);
					}
				}

				//Registry for local
				RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
				String[] instances = (String[])rk.GetValue("InstalledInstances");
				if (instances != null && instances.Length > 0)
				{
					foreach (String element in instances)
					{
						String name = "";
						//only add if it doesn't exist
						if (element == "MSSQLSERVER")
							name = System.Environment.MachineName;
						else
							name = System.Environment.MachineName + @"\" + element;

						if (serverNameComboBox.FindStringExact(name) == -1)
							serverNameComboBox.Items.Add(name);
					}
				}

				serverNameComboBox.Text = _typedServerName;
				Cursor.Current = _currentCursor;
			}
			serverDropdownInitialized = true;
		}

		private void SetNextButtonEnabledState(object sender, EventArgs e)
		{
			bool _canContinue = false;
			if (connectionString_Existing.Checked || (!String.IsNullOrWhiteSpace(newConnectionStringTextBox.Text) && !String.IsNullOrWhiteSpace(serverNameComboBox.Text) && (authenticationComboBox.SelectedIndex == 0 || (authenticationComboBox.SelectedIndex == 1 && !String.IsNullOrWhiteSpace(userNameTextBox.Text) && !String.IsNullOrWhiteSpace(passwordTextBox.Text)))))
			{
				_canContinue = true;
			}
			// only fire if it changed
			if (savedCanContinue == null ||  _canContinue != savedCanContinue.Value)
			{
				savedCanContinue = _canContinue;
				if (CanContinueChanged != null)
					CanContinueChanged(this, new CanContinueEventArgs(_canContinue));
			}
		}

		private void OnAuthenticationMethodChange(object sender, EventArgs e)
		{
			if (authenticationComboBox.SelectedIndex == 0)
			{
				// only if the control is enabled. If it is disabled then it this shouldn't save/restore
				if (authenticationComboBox.Enabled)
				{
					savedPassword = passwordTextBox.Text;
					savedUserNameText = userNameTextBox.Text;
				}
				userNameLabel.Text = "User Name";
				userNameTextBox.Text = passwordTextBox.Text = "";
				userNameTextBox.Enabled = passwordTextBox.Enabled = false;
			}
			else
			{
				if (authenticationComboBox.Enabled)
				{
					userNameTextBox.Text = savedUserNameText;
					passwordTextBox.Text = savedPassword;
				}
				userNameLabel.Text = "Login";
				userNameTextBox.Enabled = passwordTextBox.Enabled = connectionString_New.Checked;
			}

		}

		private void OnChangeConnectionStringChoice(object sender, EventArgs e)
		{
			RadioButton _selectedConnectionStringOption = sender as RadioButton;
			if (_selectedConnectionStringOption == null || (_selectedConnectionStringOption != connectionString_New && _selectedConnectionStringOption != connectionString_Existing))
			{
				throw new Exception("Passed object is neither one of 2 available radio button choices");
			}

			// Only want to know the checked radiobutton
			if (!_selectedConnectionStringOption.Checked)
				return;

			if (_selectedConnectionStringOption == connectionString_New)
			{
				// restore saved data:
				newConnectionStringTextBox.Text = savedNewConnectionStringName;
				authenticationComboBox.SelectedIndex = savedAuthenticationIndex;
				serverNameComboBox.Text = savedSqlServerName;
				userNameTextBox.Text = savedUserNameText;
				passwordTextBox.Text = savedPassword;
				asynchronousComboBox.SelectedIndex = savedAsyncChoice;

				existingConnectionStringComboBox.Enabled = false;
				newConnectionStringTextBox.Enabled = serverNameComboBox.Enabled = authenticationComboBox.Enabled = asynchronousComboBox.Enabled = true;
				OnAuthenticationMethodChange(null, null);
			}
			else
			{
				existingConnectionStringComboBox.Enabled = true;
				// All other fields are populated from the connection string, but are disabled. Previous data is saved and should be restored when the selection changes
				savedNewConnectionStringName = newConnectionStringTextBox.Text;
				savedAuthenticationIndex = authenticationComboBox.SelectedIndex < 0 ? 0 : authenticationComboBox.SelectedIndex;
				savedSqlServerName = serverNameComboBox.Text;
				savedUserNameText = userNameTextBox.Text;
				savedPassword = passwordTextBox.Text;
				savedAsyncChoice = asynchronousComboBox.SelectedIndex < 0 ? 0 : asynchronousComboBox.SelectedIndex;

				newConnectionStringTextBox.Enabled = serverNameComboBox.Enabled = authenticationComboBox.Enabled = userNameTextBox.Enabled = passwordTextBox.Enabled = asynchronousComboBox.Enabled = false;
				PopulateFieldsFromConnectionString(null, null);
				// should add here a failover partner and async setting, and 
			}
			
		}

	}

	public class CanContinueEventArgs : EventArgs
	{
		public bool canContinue;
		public bool CanContinue
		{
			get
			{
				return canContinue;
			}
		}

		public CanContinueEventArgs(bool canContinue)
		{
			this.canContinue = canContinue;
		}
	}


}
