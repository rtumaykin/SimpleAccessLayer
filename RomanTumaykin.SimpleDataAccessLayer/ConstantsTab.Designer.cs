namespace RomanTumaykin.SimpleDataAccessLayer
{
    partial class ConstantsTab
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
            this.constantsGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceTableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KeyColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Generate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.constantsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // constantsGrid
            // 
            this.constantsGrid.AllowUserToAddRows = false;
            this.constantsGrid.AllowUserToDeleteRows = false;
            this.constantsGrid.AllowUserToOrderColumns = true;
            this.constantsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.constantsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.constantsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.constantsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SourceTableName,
            this.KeyColumn,
            this.ValueColumn,
            this.Alias,
            this.Generate});
            this.constantsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.constantsGrid.Location = new System.Drawing.Point(3, 3);
            this.constantsGrid.MultiSelect = false;
            this.constantsGrid.Name = "constantsGrid";
            this.constantsGrid.Size = new System.Drawing.Size(872, 265);
            this.constantsGrid.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 150F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Source Table Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 249;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Value Column";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 165;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Alias";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 166;
            // 
            // SourceTableName
            // 
            this.SourceTableName.FillWeight = 150F;
            this.SourceTableName.HeaderText = "Source Table Name";
            this.SourceTableName.Name = "SourceTableName";
            this.SourceTableName.ReadOnly = true;
            // 
            // KeyColumn
            // 
            this.KeyColumn.AutoComplete = false;
            this.KeyColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KeyColumn.HeaderText = "Key Column";
            this.KeyColumn.Name = "KeyColumn";
            this.KeyColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ValueColumn
            // 
            this.ValueColumn.HeaderText = "Value Column";
            this.ValueColumn.Name = "ValueColumn";
            this.ValueColumn.ReadOnly = true;
            // 
            // Alias
            // 
            this.Alias.HeaderText = "Alias";
            this.Alias.Name = "Alias";
            this.Alias.ReadOnly = true;
            // 
            // Generate
            // 
            this.Generate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Generate.FillWeight = 50F;
            this.Generate.HeaderText = "Generate";
            this.Generate.Name = "Generate";
            // 
            // ConstantsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.constantsGrid);
            this.Name = "ConstantsTab";
            this.Size = new System.Drawing.Size(878, 269);
            ((System.ComponentModel.ISupportInitialize)(this.constantsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView constantsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourceTableName;
        private System.Windows.Forms.DataGridViewComboBoxColumn KeyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Generate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

    }
}
