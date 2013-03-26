using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;

namespace RomanTumaykin.SimpleDataAccessLayer
{
	public partial class EnumsTab : UserControl
	{
		private bool isLoading = false;
		private bool isRowUpdating = false;
		private string currentConnectionString = "";
		private DalConfig dalConfig = null;
		private Dictionary<string, Enum> configEnumsCollection = new Dictionary<string,Enum>();

		internal List<Enum> SelectedEnums
		{
			get
			{
				List<Enum> _enums = new List<Enum>();
				foreach (DataGridViewRow _row in enumsGrid.Rows)
				{
					if ((bool)_row.Cells["GenerateInterface"].Value)
					{
						_enums.Add(new Enum()
							{
								Schema = (String)_row.Cells["Schema"].Value,
								TableName = (String)_row.Cells["TableName"].Value,
								KeyColumn = (String)_row.Cells["KeyColumn"].Value,
								ValueColumn = (String)_row.Cells["ValueColumn"].Value,
								Alias = (String)_row.Cells["Alias"].Value
							});
					}
				}

				return _enums;
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


		public EnumsTab()
		{
			InitializeComponent();

			WireUpEventHandlers();
		}

		internal void SetStaticData (DalConfig dalConfig)
		{
			this.dalConfig = dalConfig;
			PrepareConfigEnumsCollection();
		}

		private void PrepareConfigEnumsCollection()
		{
			foreach (Enum _enum in dalConfig.Enums)
			{
				configEnumsCollection.Add(QuoteName(_enum.Schema) + "." + QuoteName(_enum.TableName), _enum);
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
				PopulateEnumsGrid();
			}
		}

		private void WireUpEventHandlers()
		{
			this.enumsGrid.CellValueChanged += EnumsGrid_CellValueChanged;
			VisibleChanged += SetNextButtonState;
		}

		private void SetNextButtonState(object sender, EventArgs e)
		{
			if (CanContinueChanged != null)
				CanContinueChanged(this, new CanContinueEventArgs(true));
		}

		void EnumsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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
				DataGridViewRow _row = enumsGrid.Rows[e.RowIndex];

				// this was a check box
				if (enumsGrid.Columns[e.ColumnIndex].Name == "GenerateInterface")
				{
					// if it was set to true, then need to make sure all columns are selected
					if ((bool)((DataGridViewCheckBoxCell)(_row.Cells[e.ColumnIndex])).Value)
					{
						SetDefaultsForDropDownCells(_row);
					}
					else
					{
						// remove all data from the row
						_row.Cells["KeyColumn"].Value = _row.Cells["ValueColumn"].Value = _row.Cells["Alias"].Value = "";
					}
				}
				else
				{
					// Generate is already checked - do nothing
					if (((DataGridViewCheckBoxCell)(_row.Cells["GenerateInterface"])).Value == null || (bool)((DataGridViewCheckBoxCell)(_row.Cells["GenerateInterface"])).Value)
						return;
					else
					{
						SetDefaultsForDropDownCells(_row);

						((DataGridViewCheckBoxCell)(_row.Cells["GenerateInterface"])).Value = true;
					}
				}
			}
			finally
			{
				isRowUpdating = false;
			}
		}

		private static void SetDefaultsForDropDownCells(DataGridViewRow row)
		{
			if (String.IsNullOrWhiteSpace((String)(row.Cells["KeyColumn"].Value)))
				row.Cells["KeyColumn"].Value = ((DataGridViewComboBoxCell)row.Cells["KeyColumn"]).Items[0];

			if (String.IsNullOrWhiteSpace((String)(row.Cells["ValueColumn"].Value)))
				row.Cells["ValueColumn"].Value = ((DataGridViewComboBoxCell)row.Cells["ValueColumn"]).Items[0];
		}

		private void PopulateEnumsGrid()
		{
			String _query = @"
				SELECT	[SchemaName],
						[TableName],
						CONVERT(xml, [ValueColumnsXml]) AS [ValueColumnsXml],
						CONVERT(xml, [KeyColumnsXml]) AS [KeyColumnsXml]
				FROM (
					SELECT 
						OBJECT_SCHEMA_NAME([object_id]) AS SchemaName,
						OBJECT_NAME([object_id]) AS TableName, 
						[object_id], 
					(
						SELECT [column_id] AS Value, [name] AS [Key]
						FROM [sys].[columns] ValueColumns
						WHERE 
							[object_id] = o.[object_id]
							AND [system_type_id] IN (48, 52, 56, 104, 127)
						FOR XML AUTO, ROOT('data')
					) AS ValueColumnsXml,
					(
						SELECT [column_id] AS Value, [name] AS [Key]
						FROM [sys].[columns] KeyColumns
						WHERE 
							[object_id] = o.[object_id]
							AND [system_type_id] IN (167, 175, 231, 239)
						FOR XML AUTO, ROOT('data')
					) AS KeyColumnsXml
					FROM [sys].[objects] o
					WHERE [type] IN ('U', 'V')
				) x
				WHERE [ValueColumnsXml] IS NOT NULL AND [KeyColumnsXml] IS NOT NULL
				ORDER BY 
					[SchemaName] ASC,
					[TableName] ASC;";

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
						this.enumsGrid.Rows.Clear();

						this.isLoading = true;
						try
						{
							while (_reader.Read())
							{
								AddRow(_reader.GetFieldValue<string>(0), _reader.GetFieldValue<string>(1), _reader.GetFieldValue<string>(2), _reader.GetFieldValue<string>(3));
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

		private void AddRow(string schemaName, string tableName, string valueColumnsXml, string keyColumnsXml)
		{
			// Create new row and get the cell templates
			DataGridViewRow _row = this.enumsGrid.Rows[this.enumsGrid.Rows.Add()];
			DataGridViewTextBoxCell _tableSchemaCell = (DataGridViewTextBoxCell)_row.Cells["Schema"];
			DataGridViewTextBoxCell _tableNameCell = (DataGridViewTextBoxCell)_row.Cells["TableName"];
			DataGridViewComboBoxCell _keysCell = (DataGridViewComboBoxCell)_row.Cells["KeyColumn"];
			DataGridViewComboBoxCell _valuesCell = (DataGridViewComboBoxCell)_row.Cells["ValueColumn"];
			DataGridViewTextBoxCell _alias = (DataGridViewTextBoxCell)_row.Cells["Alias"];
			DataGridViewCheckBoxCell _generate = (DataGridViewCheckBoxCell)_row.Cells["GenerateInterface"];

			_tableSchemaCell.Value = schemaName;
			_tableNameCell.Value = tableName;
			string _quotedName = QuoteName(schemaName) + "." + QuoteName(tableName);
			bool _isEnumInConfig = configEnumsCollection.ContainsKey(_quotedName);
			_alias.Value = _isEnumInConfig ? configEnumsCollection[_quotedName].Alias : "";
			_generate.Value = _isEnumInConfig;

			DataSet _keysDataSet = new DataSet();
			_keysDataSet.ReadXml(new StringReader(keyColumnsXml));

			foreach (DataRow _dataRow in _keysDataSet.Tables["KeyColumns"].Rows)
			{
				string _key = (String)_dataRow["Key"];
				int _index = _keysCell.Items.Add(_key);
			}

			DataSet _valuesDataSet = new DataSet();
			_valuesDataSet.ReadXml(new StringReader(valueColumnsXml));

			foreach (DataRow _dataRow in _valuesDataSet.Tables["ValueColumns"].Rows)
			{
				_valuesCell.Items.Add((String)_dataRow["Key"]);
			}

			if (_isEnumInConfig)
			{
				_keysCell.Value = configEnumsCollection[_quotedName].KeyColumn;
				_valuesCell.Value = configEnumsCollection[_quotedName].ValueColumn;
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
