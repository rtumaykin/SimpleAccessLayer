using System;
using System.Windows.Forms;
namespace RomanTumaykin.SimpleDataAccessLayer
{
    partial class ModelDesigner
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.previousButton = new System.Windows.Forms.Button();
			this.nextButton = new System.Windows.Forms.Button();
			this.tabContainer = new RomanTumaykin.SimpleDataAccessLayer.WizardTabControl();
			this.ApplicationConnection = new System.Windows.Forms.TabPage();
			this.appConnectionTab = new RomanTumaykin.SimpleDataAccessLayer.ApplicationConnectionTab();
			this.DesignerConnection = new System.Windows.Forms.TabPage();
			this.designerInformationTab = new RomanTumaykin.SimpleDataAccessLayer.DesignerInformationTab();
			this.DatabaseSelection = new System.Windows.Forms.TabPage();
			this.databaseSelectionTab = new RomanTumaykin.SimpleDataAccessLayer.DatabaseSelectionTab();
			this.Enums = new System.Windows.Forms.TabPage();
			this.enumsTab = new RomanTumaykin.SimpleDataAccessLayer.EnumsTab();
			this.Procedures = new System.Windows.Forms.TabPage();
			this.proceduresTab = new RomanTumaykin.SimpleDataAccessLayer.ProceduresTab();
			this.cancelButton = new System.Windows.Forms.Button();
			this.finishButton = new System.Windows.Forms.Button();
			this.tabContainer.SuspendLayout();
			this.ApplicationConnection.SuspendLayout();
			this.DesignerConnection.SuspendLayout();
			this.DatabaseSelection.SuspendLayout();
			this.Enums.SuspendLayout();
			this.Procedures.SuspendLayout();
			this.SuspendLayout();
			// 
			// previousButton
			// 
			this.previousButton.Enabled = false;
			this.previousButton.Location = new System.Drawing.Point(4, 307);
			this.previousButton.Name = "previousButton";
			this.previousButton.Size = new System.Drawing.Size(75, 23);
			this.previousButton.TabIndex = 1;
			this.previousButton.Text = "Previous";
			this.previousButton.UseVisualStyleBackColor = true;
			this.previousButton.Click += new System.EventHandler(this.PreviousButton_Click);
			// 
			// nextButton
			// 
			this.nextButton.Location = new System.Drawing.Point(85, 307);
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(75, 23);
			this.nextButton.TabIndex = 2;
			this.nextButton.Text = "Next";
			this.nextButton.UseVisualStyleBackColor = true;
			this.nextButton.Click += new System.EventHandler(this.NextButton_Click);
			// 
			// tabContainer
			// 
			this.tabContainer.Controls.Add(this.ApplicationConnection);
			this.tabContainer.Controls.Add(this.DesignerConnection);
			this.tabContainer.Controls.Add(this.DatabaseSelection);
			this.tabContainer.Controls.Add(this.Enums);
			this.tabContainer.Controls.Add(this.Procedures);
			this.tabContainer.Location = new System.Drawing.Point(0, 0);
			this.tabContainer.Name = "tabContainer";
			this.tabContainer.SelectedIndex = 0;
			this.tabContainer.Size = new System.Drawing.Size(883, 301);
			this.tabContainer.TabIndex = 0;
			// 
			// ApplicationConnection
			// 
			this.ApplicationConnection.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ApplicationConnection.Controls.Add(this.appConnectionTab);
			this.ApplicationConnection.Location = new System.Drawing.Point(4, 22);
			this.ApplicationConnection.Name = "ApplicationConnection";
			this.ApplicationConnection.Padding = new System.Windows.Forms.Padding(3);
			this.ApplicationConnection.Size = new System.Drawing.Size(875, 275);
			this.ApplicationConnection.TabIndex = 0;
			this.ApplicationConnection.Text = "Application Connection";
			// 
			// appConnectionTab
			// 
			this.appConnectionTab.Location = new System.Drawing.Point(190, 20);
			this.appConnectionTab.Name = "appConnectionTab";
			this.appConnectionTab.Size = new System.Drawing.Size(528, 249);
			this.appConnectionTab.TabIndex = 0;
			// 
			// DesignerConnection
			// 
			this.DesignerConnection.BackColor = System.Drawing.Color.WhiteSmoke;
			this.DesignerConnection.Controls.Add(this.designerInformationTab);
			this.DesignerConnection.ForeColor = System.Drawing.SystemColors.ControlText;
			this.DesignerConnection.Location = new System.Drawing.Point(4, 22);
			this.DesignerConnection.Name = "DesignerConnection";
			this.DesignerConnection.Size = new System.Drawing.Size(875, 275);
			this.DesignerConnection.TabIndex = 4;
			this.DesignerConnection.Text = "Designer Information";
			// 
			// designerInformationTab
			// 
			this.designerInformationTab.Location = new System.Drawing.Point(211, 47);
			this.designerInformationTab.Name = "designerInformationTab";
			this.designerInformationTab.Size = new System.Drawing.Size(472, 173);
			this.designerInformationTab.TabIndex = 0;
			// 
			// DatabaseSelection
			// 
			this.DatabaseSelection.BackColor = System.Drawing.Color.WhiteSmoke;
			this.DatabaseSelection.Controls.Add(this.databaseSelectionTab);
			this.DatabaseSelection.Location = new System.Drawing.Point(4, 22);
			this.DatabaseSelection.Name = "DatabaseSelection";
			this.DatabaseSelection.Padding = new System.Windows.Forms.Padding(3);
			this.DatabaseSelection.Size = new System.Drawing.Size(875, 275);
			this.DatabaseSelection.TabIndex = 3;
			this.DatabaseSelection.Text = "Target Database";
			// 
			// databaseSelectionTab
			// 
			this.databaseSelectionTab.Location = new System.Drawing.Point(213, 58);
			this.databaseSelectionTab.Name = "databaseSelectionTab";
			this.databaseSelectionTab.Size = new System.Drawing.Size(426, 150);
			this.databaseSelectionTab.TabIndex = 0;
			// 
			// Enums
			// 
			this.Enums.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Enums.Controls.Add(this.enumsTab);
			this.Enums.Location = new System.Drawing.Point(4, 22);
			this.Enums.Name = "Enums";
			this.Enums.Padding = new System.Windows.Forms.Padding(3);
			this.Enums.Size = new System.Drawing.Size(875, 275);
			this.Enums.TabIndex = 1;
			this.Enums.Text = "Enums";
			// 
			// enumsTab
			// 
			this.enumsTab.Location = new System.Drawing.Point(-2, 0);
			this.enumsTab.Name = "enumsTab";
			this.enumsTab.Size = new System.Drawing.Size(877, 279);
			this.enumsTab.TabIndex = 0;
			// 
			// Procedures
			// 
			this.Procedures.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Procedures.Controls.Add(this.proceduresTab);
			this.Procedures.Location = new System.Drawing.Point(4, 22);
			this.Procedures.Name = "Procedures";
			this.Procedures.Padding = new System.Windows.Forms.Padding(3);
			this.Procedures.Size = new System.Drawing.Size(875, 275);
			this.Procedures.TabIndex = 2;
			this.Procedures.Text = "Procedures";
			// 
			// proceduresTab
			// 
			this.proceduresTab.Location = new System.Drawing.Point(-1, 3);
			this.proceduresTab.Name = "proceduresTab";
			this.proceduresTab.Size = new System.Drawing.Size(876, 266);
			this.proceduresTab.TabIndex = 0;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(247, 307);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// finishButton
			// 
			this.finishButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.finishButton.Enabled = false;
			this.finishButton.Location = new System.Drawing.Point(166, 307);
			this.finishButton.Name = "finishButton";
			this.finishButton.Size = new System.Drawing.Size(75, 23);
			this.finishButton.TabIndex = 3;
			this.finishButton.Text = "Finish";
			this.finishButton.UseVisualStyleBackColor = true;
			this.finishButton.Click += new System.EventHandler(this.finishButton_Click);
			// 
			// ModelDesigner
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 342);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.nextButton);
			this.Controls.Add(this.finishButton);
			this.Controls.Add(this.previousButton);
			this.Controls.Add(this.tabContainer);
			this.Name = "ModelDesigner";
			this.Text = "Model Designer";
			this.tabContainer.ResumeLayout(false);
			this.ApplicationConnection.ResumeLayout(false);
			this.DesignerConnection.ResumeLayout(false);
			this.DatabaseSelection.ResumeLayout(false);
			this.Enums.ResumeLayout(false);
			this.Procedures.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion


        private WizardTabControl tabContainer;
        private System.Windows.Forms.TabPage ApplicationConnection;
        private System.Windows.Forms.TabPage Enums;
        private System.Windows.Forms.TabPage Procedures;
        private Button previousButton;
        private Button nextButton;
        private TabPage DatabaseSelection;
		private TabPage DesignerConnection;
		private EnumsTab enumsTab;
		private ApplicationConnectionTab appConnectionTab;
		private DesignerInformationTab designerInformationTab;
		private DatabaseSelectionTab databaseSelectionTab;
		private ProceduresTab proceduresTab;
		private Button cancelButton;
		private Button finishButton;

    }

    public class WizardTabControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode) // Hide tabs by trapping the TCM_ADJUSTRECT message
                m.Result = IntPtr.Zero;
            else
                base.WndProc(ref m);
        }
    }
}