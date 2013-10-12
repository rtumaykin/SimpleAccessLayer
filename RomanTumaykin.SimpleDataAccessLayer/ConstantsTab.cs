using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
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
			internal short Level { get; set; }
			internal bool IncludedAsParentOfSelectedChild { get; set; }
			internal bool IncludedExplicitly { get; set; }
			internal IList<int> ChildTables { get; set; } 
			internal Table()
			{
				ChildTables = new List<int>();
			}
		}
		
		private bool isLoading;
		private bool isRowUpdating;
		private string currentConnectionString;
		private DalConfig dalConfig;
		private readonly Dictionary<string, Constant> configConstantsCollection = new Dictionary<string, Constant>();

		internal List<Constant> SelectedConstants
		{
			get
			{
				return (from DataGridViewRow _row in constantsGrid.Rows
						where ((Table) _row.Tag).IncludedAsParentOfSelectedChild || ((Table) _row.Tag).IncludedExplicitly
						select new Constant
							{
								// Tag should always exist and should always be a Table type
								Schema = ((Table) _row.Tag).Schema, 
								TableName = ((Table) _row.Tag).TableName, 
								KeyColumn = (String) _row.Cells["KeyColumn"].Value, 
								ValueColumn = ((Table) _row.Tag).ValueColumn, 
								Alias = (String) _row.Cells["Alias"].Value,
								IsExplicitlySelected = ((Table)_row.Tag).IncludedExplicitly
							}).ToList();
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
			isLoading = false;
			isRowUpdating = false;
			currentConnectionString = "";
			dalConfig = null;

			InitializeComponent();

			WireUpEventHandlers();
		}

		internal void SetStaticData (DalConfig dalConfig)
		{
			this.dalConfig = dalConfig;
			PrepareConfigConstantsCollection();
		}

		private void PrepareConfigConstantsCollection()
		{
			foreach (Constant _constant in dalConfig.Constants)
			{
				configConstantsCollection.Add(QuoteName(_constant.Schema) + "." + QuoteName(_constant.TableName), _constant);
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
				PopulateConstantsGrid();
			}
		}

		private void WireUpEventHandlers()
		{
			constantsGrid.CellValueChanged += ConstantsGrid_CellValueChanged;
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
			if (isLoading)
				return;

			// This row is already updating 
			if (isRowUpdating)
				return;

			// if none of the above let's start row updating;
			isRowUpdating = true;
			try
			{
				DataGridViewRow _row = constantsGrid.Rows[e.RowIndex];

				// this was a check box
				if (constantsGrid.Columns[e.ColumnIndex].Name == "Generate")
				{
					if ((bool) _row.Cells["Generate"].Value)
					{
						((Table) _row.Tag).IncludedExplicitly = true;
						EnableColumns(_row, true, _row);
					}
					else
					{
						if (!((Table)_row.Tag).IncludedAsParentOfSelectedChild)
							EnableColumns(_row, false, _row);

						((Table)_row.Tag).IncludedExplicitly = false;
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

//			if (String.IsNullOrWhiteSpace((String)(row.Cells["ValueColumn"].Value)))
//				row.Cells["ValueColumn"].Value = ((DataGridViewComboBoxCell)row.Cells["ValueColumn"]).Items[0];
		}

		private void PopulateConstantsGrid()
		{
			const string _query = @"
				WITH Candidates AS (
					-- this gets the first seed set of rows
					-- only rows with single column primary keys are included
					SELECT 
						[i].[object_id] AS [TableId],
						[i].[index_id] AS [PK_IndexId],
						NULL AS [Parent_TableId],
						NULL AS [Parent_PK_IndexId],
						0 AS [Level]
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
							[Parent_PK_IndexId],
							[Level] + 1 AS [Level]
					FROM (
						SELECT 
							OBJECT_NAME(c.[object_id]) AS [TableName],
							OBJECT_NAME(p.[TableId]) AS [ParentTableName],
							[c].[object_id] AS [TableId],
							i.[index_id] AS [PK_IndexId],
							[p].[TableId] AS [Parent_TableId],
							[p].[PK_IndexId] AS [Parent_PK_IndexId],
							COUNT(*) OVER (PARTITION BY c.[object_id]) AS RowsPerIndex,
							MAX(p.[Level]) OVER (PARTITION BY 1) AS [Level]
						FROM (
								SELECT 
									c.[TableId],
									c.[PK_IndexId],	
									c.[Level],
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
						)) AS KeyColumnsXml,
						[c].[Level]
				FROM (
					SELECT  c.[TableId],
							c.[PK_IndexId],
							c.[Parent_TableId],
							c.[Parent_PK_IndexId],
							c.[Level],
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
				WHERE [CountPerTable] = 1
				ORDER BY [c].[Level] ASC;

";

			using (var _conn = new SqlConnection(currentConnectionString))
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
						this.constantsGrid.Rows.Clear();

						this.isLoading = true;
						try
						{
							while (_reader.Read())
							{
								AddRow(_reader.GetFieldValue<int>(0), _reader.GetFieldValue<string>(1),
									   _reader.GetFieldValue<string>(2), _reader.GetFieldValue<string>(4),
									   _reader.GetFieldValue<string>(5),
									   _reader.IsDBNull(3) ? (int?) null : _reader.GetFieldValue<int>(3),
									   _reader.GetFieldValue<int>(6));
							}
						}/*
						catch (Exception _e)
						{
							throw;
						}
						  */
						finally
						{
							this.isLoading = false;
						}
					}
				}
			}
		}

		private void AddRow(int tableId, string schemaName, string tableName, string valueColumn, string keyColumnsXml, int? parentTableId, int level)
		{
			// Add row to the dictionary of tables 

			var _keysDataSet = new DataSet();
			_keysDataSet.ReadXml(new StringReader(keyColumnsXml));
			var _columns = (from DataRow _dataRow in _keysDataSet.Tables["Column"].Rows select (String)_dataRow["ColumnName"]).ToList();

			var _table = new Table
				{
					TableId = tableId,
					Schema = schemaName,
					TableName = tableName,
					ValueColumn = valueColumn,
					KeyColumnNames = _columns,
					ParentTableId = parentTableId,
					Alias = "",
					Level = (short)level,
					IncludedExplicitly = false,
					IncludedAsParentOfSelectedChild = false
				};

			// The records come back in a sorted way 
			int _offset = level * 8;

			// Create new row and get the cell templates
			DataGridViewRow _row;

			if (parentTableId.HasValue)
			{
				var _index =
					(
						from DataGridViewRow _constantRow in this.constantsGrid.Rows
						where ((Table) _constantRow.Tag).TableId == parentTableId
						select _constantRow.Index + 1
					).FirstOrDefault();

				this.constantsGrid.Rows.Insert(_index, 1);

				_row = this.constantsGrid.Rows[_index];
			}
			else
			{
				_row = this.constantsGrid.Rows[this.constantsGrid.Rows.Add()];
			}
			
			// Associate additional info with the row
			_row.Tag = _table;

			DataGridViewTextBoxCell _sourceTableNameCell = (DataGridViewTextBoxCell)_row.Cells["SourceTableName"];
			DataGridViewComboBoxCell _keysCell = (DataGridViewComboBoxCell)_row.Cells["KeyColumn"];
			DataGridViewTextBoxCell _valuesCell = (DataGridViewTextBoxCell)_row.Cells["ValueColumn"];
			DataGridViewTextBoxCell _alias = (DataGridViewTextBoxCell)_row.Cells["Alias"];
			DataGridViewCheckBoxCell _generate = (DataGridViewCheckBoxCell)_row.Cells["Generate"];

			var _currentCellPadding = _sourceTableNameCell.Style.Padding;
			_sourceTableNameCell.Style.Padding = new Padding(_offset, _currentCellPadding.Top, _currentCellPadding.Right, _currentCellPadding.Bottom);

			_sourceTableNameCell.Value = (parentTableId == null ? "" : "- ") + QuoteName(schemaName) + "." + QuoteName(tableName);
			foreach (var _column in _columns)
			{
				_keysCell.Items.Add(_column);
			}
		   
			_valuesCell.Value = valueColumn;

			string _quotedName = QuoteName(schemaName) + "." + QuoteName(tableName);
			bool _isConstantInConfig = configConstantsCollection.ContainsKey(_quotedName);
			_alias.Value = _isConstantInConfig ? configConstantsCollection[_quotedName].Alias : "";
			_generate.Value = _isConstantInConfig && configConstantsCollection[_quotedName].IsExplicitlySelected;

			_table.Alias = (string)_alias.Value;

			// only run enable on explicitly enabled rows and on the disabled rows too :)
			if ((bool) _generate.Value || !_isConstantInConfig)
				EnableColumns(_row, _isConstantInConfig, _row);

		}

		private void EnableColumns(DataGridViewRow row, bool enable, DataGridViewRow rootRow)
		{
			// recursive part

			var _rowData = (Table) row.Tag;
			var _rootRowData = (Table) rootRow.Tag;

			if (_rowData.TableId == _rootRowData.TableId)
			{
				// If the current row and the root row are the same then the Explicit Included value is the same as the enable value
				_rowData.IncludedExplicitly = enable;
			}
			else
			{
				if (enable)
				{
					_rowData.IncludedAsParentOfSelectedChild = true;
					if (!_rowData.ChildTables.Contains(_rootRowData.TableId))
						_rowData.ChildTables.Add(_rootRowData.TableId);
				}
				else
				{
					// to avoid any mistakes
					while (_rowData.ChildTables.Contains(_rootRowData.TableId))
						_rowData.ChildTables.Remove(_rootRowData.TableId);

					if (_rowData.ChildTables.Count == 0)
						_rowData.IncludedAsParentOfSelectedChild = false;
				}
			}

			// if both flags are false - then disable, else enable
			bool _ultimateEnable = _rowData.IncludedAsParentOfSelectedChild || _rowData.IncludedExplicitly;

			// apply style to imitate disabled/enabled look
			foreach (var _cell in row.Cells)
			{
				var _gridCell = _cell as DataGridViewCell;
				_gridCell.Style.BackColor = _ultimateEnable ? Color.White : Color.Silver;
				_gridCell.Style.ForeColor = _ultimateEnable ? Color.Black : Color.DimGray;

				switch (_gridCell.OwningColumn.Name)
				{
					case "KeyColumn":
						_gridCell.ReadOnly = !_ultimateEnable;
						if (!_ultimateEnable)
						{
							_gridCell.Value = "";
						}
						else
						{
							if (String.IsNullOrEmpty(_gridCell.Value as string))
							{
								// First see if this table is in the saved config
								string _quotedName = QuoteName(_rowData.Schema) + "." + QuoteName(_rowData.TableName);
								if (configConstantsCollection.ContainsKey(_quotedName))
								{
									var _constant = configConstantsCollection[_quotedName];
									_gridCell.Value = _constant.KeyColumn;
								}
								else // if not then fall back to the first column name
								{
									_gridCell.Value = _rowData.KeyColumnNames[0];
								}

							}

						}
						break;

					case "Alias":
						_gridCell.ReadOnly = !_ultimateEnable;
						if (!_ultimateEnable)
						{
							_gridCell.Value = "";
						}
						else
						{
							if (String.IsNullOrEmpty(_gridCell.Value as string))
							{
								// First see if this table is in the saved config
								string _quotedName = QuoteName(_rowData.Schema) + "." + QuoteName(_rowData.TableName);
								if (configConstantsCollection.ContainsKey(_quotedName))
								{
									var _constant = configConstantsCollection[_quotedName];
									_gridCell.Value = _constant.Alias;
								}
							}

						}
						break;

				}
			}

			if (_rowData.ParentTableId.HasValue)
			{
				var _parentRow = (from DataGridViewRow _constantRow in this.constantsGrid.Rows
								  where ((Table)_constantRow.Tag).TableId == _rowData.ParentTableId.Value
								  select _constantRow
							).FirstOrDefault();
				
				EnableColumns(_parentRow, enable, rootRow);
			}
		}

		private string QuoteName (string name)
		{
			if (name == null)
				return null;
			return ("[" + name.Replace("]", "]]") + "]");
		}

		private void constantsGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			//http://msdn.microsoft.com/en-us/library/system.windows.forms.datagridview.cellvaluechanged.aspx
			if (constantsGrid.IsCurrentCellDirty && constantsGrid.CurrentCell is DataGridViewCheckBoxCell)
			{
				constantsGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
	}
}
