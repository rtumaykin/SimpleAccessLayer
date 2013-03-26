namespace RomanTumaykin.SimpleDataAccessLayer
{
    partial class DesignerInformationTab
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
			this.label4 = new System.Windows.Forms.Label();
			this.namespaceTextBox = new System.Windows.Forms.TextBox();
			this.authenticationComboBox = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.userNameTextBox = new System.Windows.Forms.TextBox();
			this.userNameLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 148);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(145, 13);
			this.label4.TabIndex = 65;
			this.label4.Text = "Generated Code Namespace";
			// 
			// namespaceTextBox
			// 
			this.namespaceTextBox.Location = new System.Drawing.Point(154, 145);
			this.namespaceTextBox.Name = "namespaceTextBox";
			this.namespaceTextBox.Size = new System.Drawing.Size(312, 20);
			this.namespaceTextBox.TabIndex = 4;
			// 
			// authenticationComboBox
			// 
			this.authenticationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.authenticationComboBox.FormattingEnabled = true;
			this.authenticationComboBox.Items.AddRange(new object[] {
            "Windows authentication",
            "SQL Server authentication"});
			this.authenticationComboBox.Location = new System.Drawing.Point(237, 0);
			this.authenticationComboBox.Name = "authenticationComboBox";
			this.authenticationComboBox.Size = new System.Drawing.Size(229, 21);
			this.authenticationComboBox.TabIndex = 1;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(131, 3);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(75, 13);
			this.label7.TabIndex = 58;
			this.label7.Text = "Authentication";
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Location = new System.Drawing.Point(267, 66);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.PasswordChar = '*';
			this.passwordTextBox.Size = new System.Drawing.Size(199, 20);
			this.passwordTextBox.TabIndex = 3;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(131, 69);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(53, 13);
			this.label8.TabIndex = 59;
			this.label8.Text = "Password";
			// 
			// userNameTextBox
			// 
			this.userNameTextBox.Location = new System.Drawing.Point(267, 33);
			this.userNameTextBox.Name = "userNameTextBox";
			this.userNameTextBox.Size = new System.Drawing.Size(199, 20);
			this.userNameTextBox.TabIndex = 2;
			// 
			// userNameLabel
			// 
			this.userNameLabel.AutoSize = true;
			this.userNameLabel.Location = new System.Drawing.Point(131, 36);
			this.userNameLabel.Name = "userNameLabel";
			this.userNameLabel.Size = new System.Drawing.Size(60, 13);
			this.userNameLabel.TabIndex = 60;
			this.userNameLabel.Text = "User Name";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 13);
			this.label1.TabIndex = 66;
			this.label1.Text = "Designer Credentials";
			// 
			// DesignerInformationTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.namespaceTextBox);
			this.Controls.Add(this.authenticationComboBox);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.passwordTextBox);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.userNameTextBox);
			this.Controls.Add(this.userNameLabel);
			this.Name = "DesignerInformationTab";
			this.Size = new System.Drawing.Size(472, 173);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox authenticationComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox namespaceTextBox;
    }
}
