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

namespace RomanTumaykin.SimpleDataAccessLayer
{
	public partial class ProceduresTab : UserControl
	{
		private bool isLoading = false;
		private bool isRowUpdating = false;
		private string currentConnectionString = "";
		private DalConfig dalConfig = null;
		private Dictionary<string, Procedure> configProceduresCollection = new Dictionary<string, Procedure>();

		internal List<Procedure> SelectedProcedures
		{
			get
			{
				List<Procedure> _procedures = new List<Procedure>();
				foreach (DataGridViewRow _row in proceduresGrid.Rows)
				{
					if ((bool)_row.Cells["GenerateInterface"].Value)
					{
						_procedures.Add(new Procedure()
						{
							Schema = (String)_row.Cells["Schema"].Value,
							ProcedureName = (String)_row.Cells["ProcedureName"].Value,
							Alias = (String)_row.Cells["Alias"].Value
						});
					}
				}

				return _procedures;
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

		public ProceduresTab()
		{
			InitializeComponent();

			WireUpEventHandlers();
		}

		internal void SetStaticData(DalConfig dalConfig)
		{
			this.dalConfig = dalConfig;
			PrepareConfigProceduresCollection();
		}

		private void PrepareConfigProceduresCollection()
		{
			foreach (Procedure _procedure in dalConfig.Procedures)
			{
				configProceduresCollection.Add(QuoteName(_procedure.Schema) + "." + QuoteName(_procedure.ProcedureName), _procedure);
			}
		}

		internal void UpdateData(string connectionString)
		{
			bool _reloadRequired = false;

			if (String.IsNullOrWhiteSpace(this.currentConnectionString))
			{
				_reloadRequired = true;
			}
			else
			{
				SqlConnectionStringBuilder _sb = new SqlConnectionStringBuilder(connectionString);
				SqlConnectionStringBuilder _currentSb = new SqlConnectionStringBuilder(this.currentConnectionString);
				if (!(_sb.DataSource == _currentSb.DataSource && _sb.InitialCatalog == _currentSb.InitialCatalog))
				{
					_reloadRequired = true;
				}
			}

			currentConnectionString = connectionString;

			if (_reloadRequired)
			{
				PopulateProceduresGrid();
			}
		}

		private void WireUpEventHandlers()
		{
			this.proceduresGrid.CellValueChanged += ProceduresGrid_CellValueChanged;
			VisibleChanged += SetNextButtonState;
		}

		private void SetNextButtonState(object sender, EventArgs e)
		{
			if (CanContinueChanged != null)
				CanContinueChanged(this, new CanContinueEventArgs(true));
		}

		void ProceduresGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			// this is happening within the same thread
			if (isLoading)
				return;

			// This row is already updating 
			if (isRowUpdating)
				return;

			// if none of the above let's start row updating;
			isRowUpdating = true;
			try
			{
				DataGridViewRow _row = proceduresGrid.Rows[e.RowIndex];

				// this was a check box
				if (proceduresGrid.Columns[e.ColumnIndex].Name == "GenerateInterface")
				{
					// if it was set to true, then need to make sure all columns are selected
					if (!((bool)((DataGridViewCheckBoxCell)(_row.Cells[e.ColumnIndex])).Value))
					{
						// remove all data from the row
						_row.Cells["Alias"].Value = "";
					}
				}
				else
				{
					// Generate is already checked - do nothing
					if (((DataGridViewCheckBoxCell)(_row.Cells["GenerateInterface"])).Value == null || (bool)((DataGridViewCheckBoxCell)(_row.Cells["GenerateInterface"])).Value)
						return;
					else
					{
						((DataGridViewCheckBoxCell)(_row.Cells["GenerateInterface"])).Value = true;
					}
				}
			}
			finally
			{
				isRowUpdating = false;
			}
		}
		
		private void PopulateProceduresGrid()
		{
			String _query = @"
				SELECT 
					OBJECT_SCHEMA_NAME([object_id]) AS SchemaName, 
					OBJECT_NAME([object_id]) AS ProcedureName
				FROM [sys].[objects] o
				WHERE [type] = 'P'
				ORDER BY 
					OBJECT_SCHEMA_NAME([object_id]) ASC, 
					OBJECT_NAME([object_id]) ASC;";

			using (SqlConnection _conn = new SqlConnection(currentConnectionString))
			{
				_conn.Open();
				using (SqlCommand _cmd = _conn.CreateCommand())
				{
					_cmd.CommandType = CommandType.StoredProcedure;
					_cmd.CommandText = "sys.sp_executesql";
					_cmd.Parameters.AddWithValue("@stmt", _query);

					using (SqlDataReader _reader = _cmd.ExecuteReader())
					{
						// since this happens only when connection server and database changes, I can wipe old items
						this.proceduresGrid.Rows.Clear();

						this.isLoading = true;
						try
						{
							while (_reader.Read())
							{
								AddRow(_reader.GetFieldValue<string>(0), _reader.GetFieldValue<string>(1));
							}
						}
						finally
						{
							this.isLoading = false;
						}
					}
				}
			}

		}

		private void AddRow(string procedureSchema, string procedureName)
		{
			// Create new row and get the cell templates
			DataGridViewRow _row = this.proceduresGrid.Rows[this.proceduresGrid.Rows.Add()];

			DataGridViewTextBoxCell _procedureSchemaCell = (DataGridViewTextBoxCell)_row.Cells["Schema"];
			DataGridViewTextBoxCell _procedureNameCell = (DataGridViewTextBoxCell)_row.Cells["ProcedureName"];
			DataGridViewTextBoxCell _alias = (DataGridViewTextBoxCell)_row.Cells["Alias"];
			DataGridViewCheckBoxCell _generate = (DataGridViewCheckBoxCell)_row.Cells["GenerateInterface"];

			string _quotedName = QuoteName(procedureSchema) + "." + QuoteName(procedureName);

			_procedureSchemaCell.Value = procedureSchema;
			_procedureNameCell.Value = procedureName;
			bool _isEnumInConfig = configProceduresCollection.ContainsKey(_quotedName);
			_alias.Value = _isEnumInConfig ? configProceduresCollection[_quotedName].Alias : "";
			_generate.Value = _isEnumInConfig;
		}
		private string QuoteName(string name)
		{
			if (name == null)
				return null;
			else
				return ("[" + name.Replace("]", "]]") + "]");
		}
	}
}
