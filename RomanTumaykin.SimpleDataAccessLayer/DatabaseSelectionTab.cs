using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace RomanTumaykin.SimpleDataAccessLayer
{
	public partial class DatabaseSelectionTab : UserControl
	{
		private List<String> databasesCollection = new List<String>();

		public string SelectedDatabase
		{
			get
			{
				return databaseNameComboBox.Text;
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

		public DatabaseSelectionTab()
		{
			InitializeComponent();
		}

		internal void UpdateData(List<String> databasesCollection, bool isChoosingAllowed)
		{
			this.databasesCollection = databasesCollection;
			UpdateDatabasesDropdown(isChoosingAllowed);
		}

		private void UpdateDatabasesDropdown(bool isChoosingAllowed)
		{
			string _currentDatabaseText = databaseNameComboBox.Text;

			databaseNameComboBox.Items.Clear();
			databaseNameComboBox.Items.AddRange(databasesCollection.ToArray());

			int _newIndex = isChoosingAllowed ? databaseNameComboBox.Items.IndexOf(_currentDatabaseText) : 0;
			if (databaseNameComboBox.Items.Count > 0)
				databaseNameComboBox.SelectedIndex = _newIndex < 0 ? 0 : _newIndex;

			SetNextButtonEnabledState(databaseNameComboBox.Items.Count > 0);
		}

		private void SetNextButtonEnabledState(bool enable)
		{
			if (CanContinueChanged != null)
				CanContinueChanged(this, new CanContinueEventArgs(enable));
		}

	}
}