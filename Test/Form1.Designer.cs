namespace Test
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.credentialsDialog1 = new Vanara.Windows.Forms.CredentialsDialog();
			this.folderBrowserDialog1 = new Vanara.Windows.Forms.FolderBrowserDialog();
			this.inputDialog1 = new Vanara.Windows.Forms.InputDialog();
			this.taskDialog1 = new Vanara.Windows.Forms.TaskDialog();
			this.aclEditorDialog1 = new Vanara.Windows.Forms.AccessControlEditorDialog();
			this.commandLink1 = new Vanara.Windows.Forms.CommandLink();
			this.glassExtenderProvider1 = new Vanara.Windows.Forms.GlassExtenderProvider();
			this.disabledItemComboBox1 = new Vanara.Windows.Forms.DisabledItemComboBox();
			this.themedImageButton1 = new Vanara.Windows.Forms.ThemedImageDraw();
			this.themedTableLayoutPanel1 = new Vanara.Windows.Forms.ThemedTableLayoutPanel();
			this.customDrawButton2 = new Test.CustomDrawButton();
			this.customDrawButton1 = new Test.CustomDrawButton();
			this.hoverPanel = new Test.CustomButtonControl();
			this.splitButton1 = new Vanara.Windows.Forms.SplitButton();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.initiateShutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.blockShutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.abortShutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lockWorkstationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.ipAddressBox1 = new Vanara.Windows.Forms.IPAddressBox();
			this.themedPanel1 = new Vanara.Windows.Forms.ThemedPanel();
			this.themedLabel1 = new Vanara.Windows.Forms.ThemedLabel();
			this.vistaControlExtender1 = new Vanara.Windows.Forms.VistaControlExtender();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.themedTableLayoutPanel1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.themedPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.listView1.GridLines = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.Location = new System.Drawing.Point(13, 45);
			this.listView1.Name = "listView1";
			this.listView1.ShowItemToolTips = true;
			this.listView1.Size = new System.Drawing.Size(467, 322);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Test";
			this.columnHeader1.Width = 91;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Params";
			this.columnHeader2.Width = 175;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Results";
			this.columnHeader3.Width = 179;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(219, 69);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(58, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "Dialogs";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.themedTableLayoutPanel1.SetColumnSpan(this.comboBox1, 4);
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(219, 13);
			this.vistaControlExtender1.SetMinVisibleItems(this.comboBox1, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(261, 21);
			this.comboBox1.TabIndex = 1;
			// 
			// credentialsDialog1
			// 
			this.credentialsDialog1.AuthenticationPackage = ((uint)(0u));
			// 
			// folderBrowserDialog1
			// 
			this.folderBrowserDialog1.BrowseOption = Vanara.Windows.Forms.FolderBrowserDialogOptions.FoldersAndFiles;
			this.folderBrowserDialog1.Caption = "My Caption";
			this.folderBrowserDialog1.Description = "Some longer text";
			// 
			// aclEditorDialog1
			// 
			this.aclEditorDialog1.Flags = ((Vanara.PInvoke.AclUI.SI_OBJECT_INFO_Flags)((((((Vanara.PInvoke.AclUI.SI_OBJECT_INFO_Flags.SI_EDIT_OWNER | Vanara.PInvoke.AclUI.SI_OBJECT_INFO_Flags.SI_EDIT_AUDITS) 
            | Vanara.PInvoke.AclUI.SI_OBJECT_INFO_Flags.SI_CONTAINER) 
            | Vanara.PInvoke.AclUI.SI_OBJECT_INFO_Flags.SI_ADVANCED) 
            | Vanara.PInvoke.AclUI.SI_OBJECT_INFO_Flags.SI_EDIT_EFFECTIVE) 
            | Vanara.PInvoke.AclUI.SI_OBJECT_INFO_Flags.SI_VIEW_ONLY)));
			this.aclEditorDialog1.ObjectName = "C:\\HP";
			this.aclEditorDialog1.ResourceType = System.Security.AccessControl.ResourceType.FileObject;
			// 
			// commandLink1
			// 
			this.commandLink1.Location = new System.Drawing.Point(13, 13);
			this.commandLink1.Name = "commandLink1";
			this.commandLink1.NoteText = "For big button access";
			this.themedTableLayoutPanel1.SetRowSpan(this.commandLink1, 3);
			this.commandLink1.Size = new System.Drawing.Size(200, 61);
			this.commandLink1.TabIndex = 0;
			this.commandLink1.Text = "Command link";
			// 
			// disabledItemComboBox1
			// 
			this.themedTableLayoutPanel1.SetColumnSpan(this.disabledItemComboBox1, 3);
			this.disabledItemComboBox1.FormattingEnabled = true;
			this.disabledItemComboBox1.Location = new System.Drawing.Point(219, 40);
			this.vistaControlExtender1.SetMinVisibleItems(this.disabledItemComboBox1, 0);
			this.disabledItemComboBox1.Name = "disabledItemComboBox1";
			this.disabledItemComboBox1.Size = new System.Drawing.Size(184, 21);
			this.disabledItemComboBox1.TabIndex = 2;
			// 
			// themedImageButton1
			// 
			this.themedImageButton1.AutoEllipsis = false;
			this.themedImageButton1.Location = new System.Drawing.Point(1, 1);
			this.themedImageButton1.Name = "themedImageButton1";
			this.themedImageButton1.Size = new System.Drawing.Size(30, 30);
			this.themedImageButton1.StyleClass = "Navigation";
			this.themedImageButton1.SupportGlass = true;
			this.themedImageButton1.TabIndex = 0;
			// 
			// themedTableLayoutPanel1
			// 
			this.themedTableLayoutPanel1.ColumnCount = 5;
			this.themedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.themedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.themedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.themedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.themedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.themedTableLayoutPanel1.Controls.Add(this.commandLink1, 0, 0);
			this.themedTableLayoutPanel1.Controls.Add(this.comboBox1, 1, 0);
			this.themedTableLayoutPanel1.Controls.Add(this.disabledItemComboBox1, 1, 1);
			this.themedTableLayoutPanel1.Controls.Add(this.customDrawButton2, 4, 2);
			this.themedTableLayoutPanel1.Controls.Add(this.button1, 1, 2);
			this.themedTableLayoutPanel1.Controls.Add(this.customDrawButton1, 3, 2);
			this.themedTableLayoutPanel1.Controls.Add(this.hoverPanel, 2, 2);
			this.themedTableLayoutPanel1.Controls.Add(this.splitButton1, 4, 1);
			this.themedTableLayoutPanel1.Controls.Add(this.linkLabel1, 1, 3);
			this.themedTableLayoutPanel1.Controls.Add(this.ipAddressBox1, 0, 3);
			this.themedTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.themedTableLayoutPanel1.Location = new System.Drawing.Point(0, 378);
			this.themedTableLayoutPanel1.Name = "themedTableLayoutPanel1";
			this.themedTableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
			this.themedTableLayoutPanel1.RowCount = 4;
			this.themedTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.themedTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.themedTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.themedTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.themedTableLayoutPanel1.Size = new System.Drawing.Size(492, 135);
			this.themedTableLayoutPanel1.StyleClass = "AEROWIZARD";
			this.themedTableLayoutPanel1.StylePart = 4;
			this.themedTableLayoutPanel1.TabIndex = 16;
			// 
			// customDrawButton2
			// 
			this.customDrawButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.customDrawButton2.AutoEllipsis = false;
			this.customDrawButton2.Location = new System.Drawing.Point(422, 69);
			this.customDrawButton2.Name = "customDrawButton2";
			this.customDrawButton2.Size = new System.Drawing.Size(58, 23);
			this.customDrawButton2.TabIndex = 7;
			this.customDrawButton2.Text = "Add IP";
			this.customDrawButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.customDrawButton2.Click += new System.EventHandler(this.customDrawButton2_Click);
			// 
			// customDrawButton1
			// 
			this.customDrawButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.customDrawButton1.AutoEllipsis = false;
			this.customDrawButton1.Location = new System.Drawing.Point(358, 69);
			this.customDrawButton1.Name = "customDrawButton1";
			this.customDrawButton1.Size = new System.Drawing.Size(58, 23);
			this.customDrawButton1.TabIndex = 6;
			this.customDrawButton1.Text = "custBtn";
			this.customDrawButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			// 
			// hoverPanel
			// 
			this.hoverPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.hoverPanel.AutoEllipsis = false;
			this.hoverPanel.BackColor = System.Drawing.SystemColors.HotTrack;
			this.hoverPanel.CornerRadius = 10;
			this.hoverPanel.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.hoverPanel.Location = new System.Drawing.Point(283, 69);
			this.hoverPanel.Name = "hoverPanel";
			this.hoverPanel.Size = new System.Drawing.Size(69, 23);
			this.hoverPanel.TabIndex = 5;
			this.hoverPanel.Text = "TaskDlg";
			this.hoverPanel.Click += new System.EventHandler(this.customDrawControl1_Click);
			// 
			// splitButton1
			// 
			this.splitButton1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.splitButton1.Location = new System.Drawing.Point(422, 40);
			this.splitButton1.Name = "splitButton1";
			this.splitButton1.Size = new System.Drawing.Size(58, 23);
			this.splitButton1.SplitMenuStrip = this.contextMenuStrip1;
			this.splitButton1.TabIndex = 3;
			this.splitButton1.Text = "Split";
			this.splitButton1.UseVisualStyleBackColor = true;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.initiateShutdownToolStripMenuItem,
            this.blockShutdownToolStripMenuItem,
            this.abortShutdownToolStripMenuItem,
            this.exitWindowsToolStripMenuItem,
            this.lockWorkstationToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(167, 114);
			// 
			// initiateShutdownToolStripMenuItem
			// 
			this.initiateShutdownToolStripMenuItem.Name = "initiateShutdownToolStripMenuItem";
			this.initiateShutdownToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.initiateShutdownToolStripMenuItem.Text = "Initiate shutdown";
			this.initiateShutdownToolStripMenuItem.Click += new System.EventHandler(this.initiateShutdownToolStripMenuItem_Click);
			// 
			// blockShutdownToolStripMenuItem
			// 
			this.blockShutdownToolStripMenuItem.Name = "blockShutdownToolStripMenuItem";
			this.blockShutdownToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.blockShutdownToolStripMenuItem.Text = "Block shutdown";
			this.blockShutdownToolStripMenuItem.Click += new System.EventHandler(this.blockShutdownToolStripMenuItem_Click);
			// 
			// abortShutdownToolStripMenuItem
			// 
			this.abortShutdownToolStripMenuItem.Name = "abortShutdownToolStripMenuItem";
			this.abortShutdownToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.abortShutdownToolStripMenuItem.Text = "Abort shutdown";
			this.abortShutdownToolStripMenuItem.Click += new System.EventHandler(this.abortShutdownToolStripMenuItem_Click);
			// 
			// exitWindowsToolStripMenuItem
			// 
			this.exitWindowsToolStripMenuItem.Name = "exitWindowsToolStripMenuItem";
			this.exitWindowsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.exitWindowsToolStripMenuItem.Text = "Logoff";
			this.exitWindowsToolStripMenuItem.Click += new System.EventHandler(this.exitWindowsToolStripMenuItem_Click);
			// 
			// lockWorkstationToolStripMenuItem
			// 
			this.lockWorkstationToolStripMenuItem.Name = "lockWorkstationToolStripMenuItem";
			this.lockWorkstationToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.lockWorkstationToolStripMenuItem.Text = "Lock workstation";
			this.lockWorkstationToolStripMenuItem.Click += new System.EventHandler(this.lockWorkstationToolStripMenuItem_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(219, 95);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(55, 13);
			this.linkLabel1.TabIndex = 9;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "linkLabel1";
			// 
			// ipAddressBox1
			// 
			this.ipAddressBox1.Location = new System.Drawing.Point(12, 97);
			this.ipAddressBox1.Margin = new System.Windows.Forms.Padding(2);
			this.ipAddressBox1.Name = "ipAddressBox1";
			this.ipAddressBox1.Size = new System.Drawing.Size(106, 23);
			this.ipAddressBox1.TabIndex = 10;
			this.ipAddressBox1.Text = "192.168.0.1";
			// 
			// themedPanel1
			// 
			this.themedPanel1.Controls.Add(this.themedLabel1);
			this.themedPanel1.Controls.Add(this.themedImageButton1);
			this.themedPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.themedPanel1.Location = new System.Drawing.Point(0, 0);
			this.themedPanel1.Name = "themedPanel1";
			this.themedPanel1.Size = new System.Drawing.Size(492, 32);
			this.themedPanel1.StyleClass = "AEROWIZARD";
			this.themedPanel1.StylePart = 1;
			this.themedPanel1.SupportGlass = true;
			this.themedPanel1.TabIndex = 17;
			// 
			// themedLabel1
			// 
			this.themedLabel1.AutoSize = true;
			this.themedLabel1.Image = global::Test.Properties.Resources._109_AllAnnotations_Error_16x16_72;
			this.themedLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.themedLabel1.Location = new System.Drawing.Point(37, 8);
			this.themedLabel1.Name = "themedLabel1";
			this.themedLabel1.Size = new System.Drawing.Size(90, 16);
			this.themedLabel1.StyleClass = "AEROWIZARD";
			this.themedLabel1.StylePart = 1;
			this.themedLabel1.StyleState = 1;
			this.themedLabel1.TabIndex = 15;
			this.themedLabel1.Text = "themedLabel1";
			this.themedLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.themedLabel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(492, 513);
			this.Controls.Add(this.themedPanel1);
			this.Controls.Add(this.themedTableLayoutPanel1);
			this.Controls.Add(this.listView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.glassExtenderProvider1.SetGlassMargins(this, new System.Windows.Forms.Padding(0, 32, 0, 0));
			this.Name = "Form1";
			this.Text = "Form1";
			this.Activated += new System.EventHandler(this.Form1_Activated);
			this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Shown += new System.EventHandler(this.Form1_Shown);
			this.themedTableLayoutPanel1.ResumeLayout(false);
			this.themedTableLayoutPanel1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.themedPanel1.ResumeLayout(false);
			this.themedPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private Vanara.Windows.Forms.CredentialsDialog credentialsDialog1;
		private Vanara.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private Vanara.Windows.Forms.InputDialog inputDialog1;
		private Vanara.Windows.Forms.TaskDialog taskDialog1;
		private Vanara.Windows.Forms.AccessControlEditorDialog aclEditorDialog1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox comboBox1;
		private CustomButtonControl hoverPanel;
		private CustomDrawButton customDrawButton1;
		private CustomDrawButton customDrawButton2;
		private Vanara.Windows.Forms.CommandLink commandLink1;
		private Vanara.Windows.Forms.GlassExtenderProvider glassExtenderProvider1;
		private Vanara.Windows.Forms.DisabledItemComboBox disabledItemComboBox1;
		private Vanara.Windows.Forms.ThemedImageDraw themedImageButton1;
		private Vanara.Windows.Forms.ThemedTableLayoutPanel themedTableLayoutPanel1;
		private Vanara.Windows.Forms.ThemedPanel themedPanel1;
		private Vanara.Windows.Forms.ThemedLabel themedLabel1;
		private Vanara.Windows.Forms.SplitButton splitButton1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private Vanara.Windows.Forms.VistaControlExtender vistaControlExtender1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private Vanara.Windows.Forms.IPAddressBox ipAddressBox1;
		private System.Windows.Forms.ToolStripMenuItem abortShutdownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitWindowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem initiateShutdownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem blockShutdownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lockWorkstationToolStripMenuItem;
	}
}

