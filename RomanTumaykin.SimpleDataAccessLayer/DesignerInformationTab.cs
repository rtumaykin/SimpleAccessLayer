using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RomanTumaykin.SimpleDataAccessLayer
{
	public partial class DesignerInformationTab : UserControl
	{
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
		public string Namespace
		{
			get
			{
				return namespaceTextBox.Text;
			}
		}


		private String savedUserNameText = "", savedPassword = "";
		private bool? savedCanContinue = null;
		private int? savedAuthenticationMethodIndex = null;

		private DalConfig dalConfig = null;

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

		public DesignerInformationTab()
		{
			InitializeComponent();

			WireUpEventHandlers();

		}

		internal void SetStaticData(DalConfig config)
		{
			// this is called after all components of this control are initialized, so it is safe to populate all data
			this.dalConfig = config;

			PopulateFormFields();
		}

		private void PopulateFormFields()
		{
			authenticationComboBox.SelectedIndex = dalConfig.DesignerConnection.Authentication is WindowsAuthentication ? 0 : 1;

			if (dalConfig.DesignerConnection.Authentication is SqlAuthentication)
			{
				SqlAuthentication _auth = dalConfig.DesignerConnection.Authentication as SqlAuthentication;
				userNameTextBox.Text = _auth.UserName;
				passwordTextBox.Text = _auth.Password;
			}

			namespaceTextBox.Text = dalConfig.Namespace;
		}


		private void WireUpEventHandlers()
		{
			authenticationComboBox.SelectedIndexChanged += OnAuthenticationMethodChange;
			authenticationComboBox.SelectedIndexChanged += SetNextButtonEnabledState;
			userNameTextBox.TextChanged += SetNextButtonEnabledState;
			passwordTextBox.TextChanged += SetNextButtonEnabledState;
			namespaceTextBox.TextChanged += SetNextButtonEnabledState;
			this.VisibleChanged += OnShow;
		}

		private void OnShow(object sender, EventArgs e)
		{
			if (!this.DesignMode && this.Visible)
			{
				savedCanContinue = null;
				SetNextButtonEnabledState(sender, e);
			}
		}

		private void SetNextButtonEnabledState(object sender, EventArgs e)
		{
			bool _canContinue = false;
			if ((authenticationComboBox.SelectedIndex == 0 || (authenticationComboBox.SelectedIndex == 1 && !String.IsNullOrWhiteSpace(userNameTextBox.Text) && !String.IsNullOrWhiteSpace(passwordTextBox.Text))) && !String.IsNullOrWhiteSpace(namespaceTextBox.Text))
			{
				_canContinue = true;
			}
			// only fire if it changed
			if (savedCanContinue == null || _canContinue != savedCanContinue.Value)
			{
				savedCanContinue = _canContinue;
				if (CanContinueChanged != null)
					CanContinueChanged(this, new CanContinueEventArgs(_canContinue));
			}
		}

		private void OnAuthenticationMethodChange(object sender, EventArgs e)
		{
			// change only if the new method is different from the previous one.
			// If you drop down and then choose again the same item, this event foires anyway even if the index did not change
			if (savedAuthenticationMethodIndex != null && authenticationComboBox.SelectedIndex == savedAuthenticationMethodIndex)
				return;

			savedAuthenticationMethodIndex = authenticationComboBox.SelectedIndex;

			if (authenticationComboBox.SelectedIndex == 0)
			{
				savedPassword = passwordTextBox.Text;
				savedUserNameText = userNameTextBox.Text;

				userNameLabel.Text = "User Name";
				userNameTextBox.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
				passwordTextBox.Text = "";
				userNameTextBox.Enabled = passwordTextBox.Enabled = false;
			}
			else
			{
				userNameTextBox.Text = savedUserNameText;
				passwordTextBox.Text = savedPassword;

				userNameLabel.Text = "Login";
				userNameTextBox.Enabled = passwordTextBox.Enabled = true;
			}
		}
	}
}
