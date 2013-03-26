namespace RomanTumaykin.SimpleDataAccessLayer
{
    partial class DatabaseSelectionTab
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
			this.label6 = new System.Windows.Forms.Label();
			this.databaseNameComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(32, 68);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(82, 13);
			this.label6.TabIndex = 38;
			this.label6.Text = "Database name";
			// 
			// databaseNameComboBox
			// 
			this.databaseNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.databaseNameComboBox.FormattingEnabled = true;
			this.databaseNameComboBox.Location = new System.Drawing.Point(165, 65);
			this.databaseNameComboBox.Name = "databaseNameComboBox";
			this.databaseNameComboBox.Size = new System.Drawing.Size(229, 21);
			this.databaseNameComboBox.TabIndex = 1;
			// 
			// DatabaseSelectionTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label6);
			this.Controls.Add(this.databaseNameComboBox);
			this.Name = "DatabaseSelectionTab";
			this.Size = new System.Drawing.Size(426, 150);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox databaseNameComboBox;
    }
}
