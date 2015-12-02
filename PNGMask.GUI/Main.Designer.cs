namespace PNGMask.GUI
{
    partial class Main
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
            this.menu = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuActionInject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuActionInjectImage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuActionInjectText = new System.Windows.Forms.ToolStripMenuItem();
            this.menuActionInjectBinary = new System.Windows.Forms.ToolStripMenuItem();
            this.menuActionInjectIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuActionDumpOriginal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuActionDumpHidden = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabOriginal = new System.Windows.Forms.TabPage();
            this.lblNoFile = new System.Windows.Forms.Label();
            this.imgOriginal = new System.Windows.Forms.PictureBox();
            this.tabExtracted = new System.Windows.Forms.TabPage();
            this.hexHidden = new Clearbytes.HexView();
            this.imgHidden = new System.Windows.Forms.PictureBox();
            this.txtHidden = new System.Windows.Forms.TextBox();
            this.menu.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabOriginal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgOriginal)).BeginInit();
            this.tabExtracted.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgHidden)).BeginInit();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuAction,
            this.menuHelp});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(629, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileOpen,
            this.menuFileSave,
            this.toolStripMenuItem1,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "File";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size(148, 22);
            this.menuFileOpen.Text = "Open Image...";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuFileSave
            // 
            this.menuFileSave.Enabled = false;
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.Size = new System.Drawing.Size(148, 22);
            this.menuFileSave.Text = "Save Image...";
            this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 6);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Size = new System.Drawing.Size(148, 22);
            this.menuFileExit.Text = "Exit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // menuAction
            // 
            this.menuAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuActionInject,
            this.toolStripMenuItem2,
            this.menuActionDumpOriginal,
            this.menuActionDumpHidden});
            this.menuAction.Name = "menuAction";
            this.menuAction.Size = new System.Drawing.Size(54, 20);
            this.menuAction.Text = "Action";
            // 
            // menuActionInject
            // 
            this.menuActionInject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuActionInjectImage,
            this.menuActionInjectText,
            this.menuActionInjectBinary,
            this.menuActionInjectIndex});
            this.menuActionInject.Enabled = false;
            this.menuActionInject.Name = "menuActionInject";
            this.menuActionInject.Size = new System.Drawing.Size(163, 22);
            this.menuActionInject.Text = "Inject";
            // 
            // menuActionInjectImage
            // 
            this.menuActionInjectImage.Name = "menuActionInjectImage";
            this.menuActionInjectImage.Size = new System.Drawing.Size(135, 22);
            this.menuActionInjectImage.Text = "Image...";
            this.menuActionInjectImage.Click += new System.EventHandler(this.menuActionInjectImage_Click);
            // 
            // menuActionInjectText
            // 
            this.menuActionInjectText.Name = "menuActionInjectText";
            this.menuActionInjectText.Size = new System.Drawing.Size(135, 22);
            this.menuActionInjectText.Text = "Text...";
            this.menuActionInjectText.Click += new System.EventHandler(this.menuActionInjectText_Click);
            // 
            // menuActionInjectBinary
            // 
            this.menuActionInjectBinary.Name = "menuActionInjectBinary";
            this.menuActionInjectBinary.Size = new System.Drawing.Size(135, 22);
            this.menuActionInjectBinary.Text = "Binary file...";
            this.menuActionInjectBinary.Click += new System.EventHandler(this.menuActionInjectBinary_Click);
            // 
            // menuActionInjectIndex
            // 
            this.menuActionInjectIndex.Name = "menuActionInjectIndex";
            this.menuActionInjectIndex.Size = new System.Drawing.Size(135, 22);
            this.menuActionInjectIndex.Text = "Link index";
            this.menuActionInjectIndex.Click += new System.EventHandler(this.menuActionInjectIndex_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(160, 6);
            // 
            // menuActionDumpOriginal
            // 
            this.menuActionDumpOriginal.Enabled = false;
            this.menuActionDumpOriginal.Name = "menuActionDumpOriginal";
            this.menuActionDumpOriginal.Size = new System.Drawing.Size(163, 22);
            this.menuActionDumpOriginal.Text = "Extract Original...";
            this.menuActionDumpOriginal.Click += new System.EventHandler(this.menuActionDumpOriginal_Click);
            // 
            // menuActionDumpHidden
            // 
            this.menuActionDumpHidden.Enabled = false;
            this.menuActionDumpHidden.Name = "menuActionDumpHidden";
            this.menuActionDumpHidden.Size = new System.Drawing.Size(163, 22);
            this.menuActionDumpHidden.Text = "Extract Hidden...";
            this.menuActionDumpHidden.Click += new System.EventHandler(this.menuActionDumpHidden_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "Help";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.menuHelpAbout.Text = "About";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabOriginal);
            this.tabs.Controls.Add(this.tabExtracted);
            this.tabs.Enabled = false;
            this.tabs.Location = new System.Drawing.Point(-1, 24);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(633, 309);
            this.tabs.TabIndex = 1;
            // 
            // tabOriginal
            // 
            this.tabOriginal.BackColor = System.Drawing.SystemColors.Window;
            this.tabOriginal.Controls.Add(this.lblNoFile);
            this.tabOriginal.Controls.Add(this.imgOriginal);
            this.tabOriginal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabOriginal.Location = new System.Drawing.Point(4, 22);
            this.tabOriginal.Name = "tabOriginal";
            this.tabOriginal.Padding = new System.Windows.Forms.Padding(3);
            this.tabOriginal.Size = new System.Drawing.Size(625, 283);
            this.tabOriginal.TabIndex = 0;
            this.tabOriginal.Text = "Original";
            // 
            // lblNoFile
            // 
            this.lblNoFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNoFile.AutoSize = true;
            this.lblNoFile.BackColor = System.Drawing.SystemColors.Control;
            this.lblNoFile.Location = new System.Drawing.Point(269, 135);
            this.lblNoFile.Name = "lblNoFile";
            this.lblNoFile.Size = new System.Drawing.Size(87, 13);
            this.lblNoFile.TabIndex = 0;
            this.lblNoFile.Text = "No image loaded";
            // 
            // imgOriginal
            // 
            this.imgOriginal.BackColor = System.Drawing.SystemColors.Control;
            this.imgOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgOriginal.Location = new System.Drawing.Point(3, 3);
            this.imgOriginal.Name = "imgOriginal";
            this.imgOriginal.Size = new System.Drawing.Size(619, 277);
            this.imgOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgOriginal.TabIndex = 1;
            this.imgOriginal.TabStop = false;
            // 
            // tabExtracted
            // 
            this.tabExtracted.Controls.Add(this.txtHidden);
            this.tabExtracted.Controls.Add(this.hexHidden);
            this.tabExtracted.Controls.Add(this.imgHidden);
            this.tabExtracted.Location = new System.Drawing.Point(4, 22);
            this.tabExtracted.Name = "tabExtracted";
            this.tabExtracted.Padding = new System.Windows.Forms.Padding(3);
            this.tabExtracted.Size = new System.Drawing.Size(625, 283);
            this.tabExtracted.TabIndex = 1;
            this.tabExtracted.Text = "Extracted file";
            this.tabExtracted.UseVisualStyleBackColor = true;
            // 
            // hexHidden
            // 
            this.hexHidden.BackColor = System.Drawing.SystemColors.Window;
            this.hexHidden.Location = new System.Drawing.Point(115, 6);
            this.hexHidden.Name = "hexHidden";
            this.hexHidden.Size = new System.Drawing.Size(100, 100);
            this.hexHidden.TabIndex = 1;
            this.hexHidden.Text = "hexHidden";
            this.hexHidden.Visible = false;
            // 
            // imgHidden
            // 
            this.imgHidden.BackColor = System.Drawing.SystemColors.Control;
            this.imgHidden.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgHidden.Location = new System.Drawing.Point(9, 6);
            this.imgHidden.Name = "imgHidden";
            this.imgHidden.Size = new System.Drawing.Size(100, 100);
            this.imgHidden.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgHidden.TabIndex = 0;
            this.imgHidden.TabStop = false;
            this.imgHidden.Visible = false;
            // 
            // txtHidden
            // 
            this.txtHidden.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHidden.Location = new System.Drawing.Point(221, 6);
            this.txtHidden.Multiline = true;
            this.txtHidden.Name = "txtHidden";
            this.txtHidden.ReadOnly = true;
            this.txtHidden.Size = new System.Drawing.Size(100, 100);
            this.txtHidden.TabIndex = 2;
            this.txtHidden.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(629, 332);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "PNG Mask";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.tabOriginal.ResumeLayout(false);
            this.tabOriginal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgOriginal)).EndInit();
            this.tabExtracted.ResumeLayout(false);
            this.tabExtracted.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgHidden)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuAction;
        private System.Windows.Forms.ToolStripMenuItem menuActionInject;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuActionDumpOriginal;
        private System.Windows.Forms.ToolStripMenuItem menuActionDumpHidden;
        private System.Windows.Forms.ToolStripMenuItem menuActionInjectImage;
        private System.Windows.Forms.ToolStripMenuItem menuActionInjectText;
        private System.Windows.Forms.ToolStripMenuItem menuActionInjectIndex;
        private System.Windows.Forms.ToolStripMenuItem menuActionInjectBinary;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabOriginal;
        private System.Windows.Forms.TabPage tabExtracted;
        private System.Windows.Forms.Label lblNoFile;
        private System.Windows.Forms.PictureBox imgOriginal;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private System.Windows.Forms.PictureBox imgHidden;
        private Clearbytes.HexView hexHidden;
        private System.Windows.Forms.TextBox txtHidden;
    }
}

