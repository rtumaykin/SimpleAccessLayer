namespace RomanTumaykin.SimpleDataAccessLayer
{
    partial class ApplicationConnectionTab
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationConnectionTab));
			this.label2 = new System.Windows.Forms.Label();
			this.newConnectionStringTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.connectionString_New = new System.Windows.Forms.RadioButton();
			this.existingConnectionStringComboBox = new System.Windows.Forms.ComboBox();
			this.connectionString_Existing = new System.Windows.Forms.RadioButton();
			this.authenticationComboBox = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.serverNameComboBox = new System.Windows.Forms.ComboBox();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.userNameTextBox = new System.Windows.Forms.TextBox();
			this.userNameLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.asynchronousComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(146, 13);
			this.label2.TabIndex = 76;
			this.label2.Text = "Application Connection String";
			// 
			// newConnectionStringTextBox
			// 
			this.newConnectionStringTextBox.Location = new System.Drawing.Point(299, 52);
			this.newConnectionStringTextBox.Name = "newConnectionStringTextBox";
			this.newConnectionStringTextBox.Size = new System.Drawing.Size(229, 20);
			this.newConnectionStringTextBox.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(193, 55);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 74;
			this.label3.Text = "Name:";
			// 
			// connectionString_New
			// 
			this.connectionString_New.AutoSize = true;
			this.connectionString_New.Location = new System.Drawing.Point(176, 25);
			this.connectionString_New.Name = "connectionString_New";
			this.connectionString_New.Size = new System.Drawing.Size(81, 17);
			this.connectionString_New.TabIndex = 3;
			this.connectionString_New.TabStop = true;
			this.connectionString_New.Text = "Create New";
			this.connectionString_New.UseVisualStyleBackColor = true;
			// 
			// existingConnectionStringComboBox
			// 
			this.existingConnectionStringComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.existingConnectionStringComboBox.FormattingEnabled = true;
			this.existingConnectionStringComboBox.Location = new System.Drawing.Point(299, 0);
			this.existingConnectionStringComboBox.Name = "existingConnectionStringComboBox";
			this.existingConnectionStringComboBox.Size = new System.Drawing.Size(229, 21);
			this.existingConnectionStringComboBox.TabIndex = 2;
			// 
			// connectionString_Existing
			// 
			this.connectionString_Existing.AutoSize = true;
			this.connectionString_Existing.Location = new System.Drawing.Point(176, 1);
			this.connectionString_Existing.Name = "connectionString_Existing";
			this.connectionString_Existing.Size = new System.Drawing.Size(100, 17);
			this.connectionString_Existing.TabIndex = 1;
			this.connectionString_Existing.TabStop = true;
			this.connectionString_Existing.Text = "Choose Existing";
			this.connectionString_Existing.UseVisualStyleBackColor = true;
			// 
			// authenticationComboBox
			// 
			this.authenticationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.authenticationComboBox.FormattingEnabled = true;
			this.authenticationComboBox.Items.AddRange(new object[] {
            "Windows authentication",
            "SQL Server authentication"});
			this.authenticationComboBox.Location = new System.Drawing.Point(299, 115);
			this.authenticationComboBox.Name = "authenticationComboBox";
			this.authenticationComboBox.Size = new System.Drawing.Size(229, 21);
			this.authenticationComboBox.TabIndex = 6;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(193, 118);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(75, 13);
			this.label5.TabIndex = 63;
			this.label5.Text = "Authentication";
			// 
			// serverNameComboBox
			// 
			this.serverNameComboBox.FormattingEnabled = true;
			this.serverNameComboBox.Location = new System.Drawing.Point(299, 83);
			this.serverNameComboBox.Name = "serverNameComboBox";
			this.serverNameComboBox.Size = new System.Drawing.Size(229, 21);
			this.serverNameComboBox.TabIndex = 5;
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Location = new System.Drawing.Point(329, 181);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.PasswordChar = '*';
			this.passwordTextBox.Size = new System.Drawing.Size(199, 20);
			this.passwordTextBox.TabIndex = 8;
			// 
			// PasswordLabel
			// 
			this.PasswordLabel.AutoSize = true;
			this.PasswordLabel.Location = new System.Drawing.Point(193, 184);
			this.PasswordLabel.Name = "PasswordLabel";
			this.PasswordLabel.Size = new System.Drawing.Size(53, 13);
			this.PasswordLabel.TabIndex = 64;
			this.PasswordLabel.Text = "Password";
			// 
			// userNameTextBox
			// 
			this.userNameTextBox.Location = new System.Drawing.Point(329, 148);
			this.userNameTextBox.Name = "userNameTextBox";
			this.userNameTextBox.Size = new System.Drawing.Size(199, 20);
			this.userNameTextBox.TabIndex = 7;
			// 
			// userNameLabel
			// 
			this.userNameLabel.AutoSize = true;
			this.userNameLabel.Location = new System.Drawing.Point(193, 151);
			this.userNameLabel.Name = "userNameLabel";
			this.userNameLabel.Size = new System.Drawing.Size(60, 13);
			this.userNameLabel.TabIndex = 65;
			this.userNameLabel.Text = "User Name";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(193, 86);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 13);
			this.label1.TabIndex = 66;
			this.label1.Text = "SQL Server name";
			// 
			// textBox1
			// 
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(6, 55);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(143, 136);
			this.textBox1.TabIndex = 77;
			this.textBox1.TabStop = false;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(193, 218);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(74, 13);
			this.label4.TabIndex = 78;
			this.label4.Text = "Asynchronous";
			// 
			// asynchronousComboBox
			// 
			this.asynchronousComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.asynchronousComboBox.FormattingEnabled = true;
			this.asynchronousComboBox.Items.AddRange(new object[] {
            "Yes",
            "No"});
			this.asynchronousComboBox.Location = new System.Drawing.Point(329, 215);
			this.asynchronousComboBox.Name = "asynchronousComboBox";
			this.asynchronousComboBox.Size = new System.Drawing.Size(199, 21);
			this.asynchronousComboBox.TabIndex = 9;
			// 
			// ApplicationConnectionTab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.asynchronousComboBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.newConnectionStringTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.connectionString_New);
			this.Controls.Add(this.existingConnectionStringComboBox);
			this.Controls.Add(this.connectionString_Existing);
			this.Controls.Add(this.authenticationComboBox);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.serverNameComboBox);
			this.Controls.Add(this.passwordTextBox);
			this.Controls.Add(this.PasswordLabel);
			this.Controls.Add(this.userNameTextBox);
			this.Controls.Add(this.userNameLabel);
			this.Controls.Add(this.label1);
			this.Name = "ApplicationConnectionTab";
			this.Size = new System.Drawing.Size(528, 240);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox newConnectionStringTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton connectionString_New;
        private System.Windows.Forms.ComboBox existingConnectionStringComboBox;
        private System.Windows.Forms.RadioButton connectionString_Existing;
        private System.Windows.Forms.ComboBox authenticationComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox serverNameComboBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox asynchronousComboBox;

    }
}
