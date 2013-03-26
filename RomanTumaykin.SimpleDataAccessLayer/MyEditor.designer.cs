namespace RomanTumaykin.SimpleDataAccessLayer
{
    partial class MyEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.labelConfigFileConnectionStringName = new System.Windows.Forms.Label();
			this.labelServerName = new System.Windows.Forms.Label();
			this.labelDatabaseName = new System.Windows.Forms.Label();
			this.labelNamespace = new System.Windows.Forms.Label();
			this.labelObjects = new System.Windows.Forms.Label();
			this.Edit = new System.Windows.Forms.Button();
			this.ConnectionStringName = new System.Windows.Forms.Label();
			this.Server = new System.Windows.Forms.Label();
			this.Database = new System.Windows.Forms.Label();
			this.Namespace = new System.Windows.Forms.Label();
			this.EnumsGridView = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ProcsGrid = new System.Windows.Forms.DataGridView();
			this.Schema = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SourceTable = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.KeyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ProcedureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.EnumsGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ProcsGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// labelConfigFileConnectionStringName
			// 
			this.labelConfigFileConnectionStringName.AutoSize = true;
			this.labelConfigFileConnectionStringName.Location = new System.Drawing.Point(4, 26);
			this.labelConfigFileConnectionStringName.Name = "labelConfigFileConnectionStringName";
			this.labelConfigFileConnectionStringName.Size = new System.Drawing.Size(206, 13);
			this.labelConfigFileConnectionStringName.TabIndex = 0;
			this.labelConfigFileConnectionStringName.Text = "Configuration File Connection String Name";
			// 
			// labelServerName
			// 
			this.labelServerName.AutoSize = true;
			this.labelServerName.Location = new System.Drawing.Point(4, 50);
			this.labelServerName.Name = "labelServerName";
			this.labelServerName.Size = new System.Drawing.Size(103, 13);
			this.labelServerName.TabIndex = 1;
			this.labelServerName.Text = "SQL Server Address";
			// 
			// labelDatabaseName
			// 
			this.labelDatabaseName.AutoSize = true;
			this.labelDatabaseName.Location = new System.Drawing.Point(4, 75);
			this.labelDatabaseName.Name = "labelDatabaseName";
			this.labelDatabaseName.Size = new System.Drawing.Size(84, 13);
			this.labelDatabaseName.TabIndex = 2;
			this.labelDatabaseName.Text = "Database Name";
			// 
			// labelNamespace
			// 
			this.labelNamespace.AutoSize = true;
			this.labelNamespace.Location = new System.Drawing.Point(4, 100);
			this.labelNamespace.Name = "labelNamespace";
			this.labelNamespace.Size = new System.Drawing.Size(92, 13);
			this.labelNamespace.TabIndex = 3;
			this.labelNamespace.Text = "Code Namespace";
			// 
			// labelObjects
			// 
			this.labelObjects.AutoSize = true;
			this.labelObjects.Location = new System.Drawing.Point(4, 123);
			this.labelObjects.Name = "labelObjects";
			this.labelObjects.Size = new System.Drawing.Size(127, 13);
			this.labelObjects.TabIndex = 5;
			this.labelObjects.Text = "List of Generated Objects";
			// 
			// Edit
			// 
			this.Edit.Location = new System.Drawing.Point(0, 0);
			this.Edit.Name = "Edit";
			this.Edit.Size = new System.Drawing.Size(131, 23);
			this.Edit.TabIndex = 6;
			this.Edit.Text = "Edit Model Info";
			this.Edit.UseVisualStyleBackColor = true;
			this.Edit.Click += new System.EventHandler(this.Edit_Click);
			// 
			// ConnectionStringName
			// 
			this.ConnectionStringName.AutoSize = true;
			this.ConnectionStringName.Location = new System.Drawing.Point(279, 26);
			this.ConnectionStringName.Name = "ConnectionStringName";
			this.ConnectionStringName.Size = new System.Drawing.Size(11, 13);
			this.ConnectionStringName.TabIndex = 7;
			this.ConnectionStringName.Text = "*";
			// 
			// Server
			// 
			this.Server.AutoSize = true;
			this.Server.Location = new System.Drawing.Point(279, 50);
			this.Server.Name = "Server";
			this.Server.Size = new System.Drawing.Size(11, 13);
			this.Server.TabIndex = 8;
			this.Server.Text = "*";
			// 
			// Database
			// 
			this.Database.AutoSize = true;
			this.Database.Location = new System.Drawing.Point(279, 75);
			this.Database.Name = "Database";
			this.Database.Size = new System.Drawing.Size(11, 13);
			this.Database.TabIndex = 9;
			this.Database.Text = "*";
			// 
			// Namespace
			// 
			this.Namespace.AutoSize = true;
			this.Namespace.Location = new System.Drawing.Point(279, 100);
			this.Namespace.Name = "Namespace";
			this.Namespace.Size = new System.Drawing.Size(11, 13);
			this.Namespace.TabIndex = 10;
			this.Namespace.Text = "*";
			// 
			// EnumsGridView
			// 
			this.EnumsGridView.AllowUserToAddRows = false;
			this.EnumsGridView.AllowUserToDeleteRows = false;
			this.EnumsGridView.AllowUserToOrderColumns = true;
			this.EnumsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.EnumsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.EnumsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Schema,
            this.SourceTable,
            this.Alias,
            this.KeyColumn,
            this.ValueColumn});
			this.EnumsGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.EnumsGridView.Location = new System.Drawing.Point(0, 161);
			this.EnumsGridView.Name = "EnumsGridView";
			this.EnumsGridView.ReadOnly = true;
			this.EnumsGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.EnumsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.EnumsGridView.Size = new System.Drawing.Size(869, 176);
			this.EnumsGridView.TabIndex = 11;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 145);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Enums";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 352);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "Procedures";
			// 
			// ProcsGrid
			// 
			this.ProcsGrid.AllowUserToAddRows = false;
			this.ProcsGrid.AllowUserToDeleteRows = false;
			this.ProcsGrid.AllowUserToOrderColumns = true;
			this.ProcsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.ProcsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ProcsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.ProcedureName,
            this.dataGridViewTextBoxColumn2});
			this.ProcsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.ProcsGrid.Location = new System.Drawing.Point(0, 368);
			this.ProcsGrid.Name = "ProcsGrid";
			this.ProcsGrid.ReadOnly = true;
			this.ProcsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ProcsGrid.Size = new System.Drawing.Size(869, 210);
			this.ProcsGrid.TabIndex = 14;
			// 
			// Schema
			// 
			this.Schema.FillWeight = 75F;
			this.Schema.HeaderText = "Source Table Schema";
			this.Schema.Name = "Schema";
			this.Schema.ReadOnly = true;
			// 
			// SourceTable
			// 
			this.SourceTable.HeaderText = "Source Table Name";
			this.SourceTable.Name = "SourceTable";
			this.SourceTable.ReadOnly = true;
			// 
			// Alias
			// 
			this.Alias.FillWeight = 50F;
			this.Alias.HeaderText = "Alias";
			this.Alias.Name = "Alias";
			this.Alias.ReadOnly = true;
			// 
			// KeyColumn
			// 
			this.KeyColumn.HeaderText = "Key Column Name";
			this.KeyColumn.Name = "KeyColumn";
			this.KeyColumn.ReadOnly = true;
			// 
			// ValueColumn
			// 
			this.ValueColumn.HeaderText = "Value Column Name";
			this.ValueColumn.Name = "ValueColumn";
			this.ValueColumn.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.FillWeight = 75F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Procedure Schema";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// ProcedureName
			// 
			this.ProcedureName.HeaderText = "Procedure Name";
			this.ProcedureName.Name = "ProcedureName";
			this.ProcedureName.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.HeaderText = "Alias";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			// 
			// MyEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.ProcsGrid);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.EnumsGridView);
			this.Controls.Add(this.Namespace);
			this.Controls.Add(this.Database);
			this.Controls.Add(this.Server);
			this.Controls.Add(this.ConnectionStringName);
			this.Controls.Add(this.Edit);
			this.Controls.Add(this.labelObjects);
			this.Controls.Add(this.labelNamespace);
			this.Controls.Add(this.labelDatabaseName);
			this.Controls.Add(this.labelServerName);
			this.Controls.Add(this.labelConfigFileConnectionStringName);
			this.Name = "MyEditor";
			this.Size = new System.Drawing.Size(872, 581);
			((System.ComponentModel.ISupportInitialize)(this.EnumsGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ProcsGrid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelConfigFileConnectionStringName;
        private System.Windows.Forms.Label labelServerName;
        private System.Windows.Forms.Label labelDatabaseName;
        private System.Windows.Forms.Label labelNamespace;
		private System.Windows.Forms.Label labelObjects;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		internal System.Windows.Forms.Button Edit;
		private System.Windows.Forms.Label ConnectionStringName;
		private System.Windows.Forms.Label Server;
		private System.Windows.Forms.Label Database;
		private System.Windows.Forms.Label Namespace;
		private System.Windows.Forms.DataGridView EnumsGridView;
		private System.Windows.Forms.DataGridView ProcsGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn Schema;
		private System.Windows.Forms.DataGridViewTextBoxColumn SourceTable;
		private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
		private System.Windows.Forms.DataGridViewTextBoxColumn KeyColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn ValueColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn ProcedureName;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}
