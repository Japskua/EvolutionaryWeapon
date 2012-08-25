namespace XNAPlatformerLevelEditor
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
            this.spliter = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.contentPathTextBox = new System.Windows.Forms.TextBox();
            this.levelsToolStrip = new System.Windows.Forms.ToolStrip();
            this.addLevelButton = new System.Windows.Forms.ToolStripButton();
            this.removeLevelButton = new System.Windows.Forms.ToolStripButton();
            this.levelsListBox = new System.Windows.Forms.ListBox();
            this.paintingToolStrip = new System.Windows.Forms.ToolStrip();
            this.paintButton = new System.Windows.Forms.ToolStripButton();
            this.eraseButton = new System.Windows.Forms.ToolStripButton();
            this.fillButton = new System.Windows.Forms.ToolStripButton();
            this.textureListView = new System.Windows.Forms.ListView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileDisplay1 = new XNAPlatformerLevelEditor.TileDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.spliter)).BeginInit();
            this.spliter.Panel1.SuspendLayout();
            this.spliter.Panel2.SuspendLayout();
            this.spliter.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.levelsToolStrip.SuspendLayout();
            this.paintingToolStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spliter
            // 
            this.spliter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spliter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spliter.Location = new System.Drawing.Point(0, 24);
            this.spliter.Name = "spliter";
            // 
            // spliter.Panel1
            // 
            this.spliter.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.spliter.Panel1MinSize = 300;
            // 
            // spliter.Panel2
            // 
            this.spliter.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.spliter.Panel2MinSize = 100;
            this.spliter.Size = new System.Drawing.Size(1008, 706);
            this.spliter.SplitterDistance = 725;
            this.spliter.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.vScrollBar1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.hScrollBar1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tileDisplay1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(725, 706);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar1.Location = new System.Drawing.Point(705, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(20, 686);
            this.vScrollBar1.TabIndex = 0;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 686);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(705, 20);
            this.hScrollBar1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.contentPathTextBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.levelsToolStrip, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.levelsListBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.paintingToolStrip, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textureListView, 0, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 374F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(279, 706);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // contentPathTextBox
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.contentPathTextBox, 2);
            this.contentPathTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentPathTextBox.Location = new System.Drawing.Point(3, 3);
            this.contentPathTextBox.Name = "contentPathTextBox";
            this.contentPathTextBox.ReadOnly = true;
            this.contentPathTextBox.Size = new System.Drawing.Size(273, 20);
            this.contentPathTextBox.TabIndex = 0;
            this.contentPathTextBox.Visible = false;
            // 
            // levelsToolStrip
            // 
            this.levelsToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.levelsToolStrip, 2);
            this.levelsToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.levelsToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.levelsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLevelButton,
            this.removeLevelButton});
            this.levelsToolStrip.Location = new System.Drawing.Point(0, 286);
            this.levelsToolStrip.Name = "levelsToolStrip";
            this.levelsToolStrip.Size = new System.Drawing.Size(279, 39);
            this.levelsToolStrip.TabIndex = 2;
            this.levelsToolStrip.Text = "toolStrip1";
            // 
            // addLevelButton
            // 
            this.addLevelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addLevelButton.Image = global::XNAPlatformerLevelEditor.Properties.Resources.layer_add;
            this.addLevelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addLevelButton.Name = "addLevelButton";
            this.addLevelButton.Size = new System.Drawing.Size(36, 36);
            this.addLevelButton.ToolTipText = "Add a level to the list";
            this.addLevelButton.Click += new System.EventHandler(this.addLevelButton_Click);
            // 
            // removeLevelButton
            // 
            this.removeLevelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeLevelButton.Image = global::XNAPlatformerLevelEditor.Properties.Resources.layer_delete;
            this.removeLevelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeLevelButton.Name = "removeLevelButton";
            this.removeLevelButton.Size = new System.Drawing.Size(36, 36);
            this.removeLevelButton.ToolTipText = "Remove a level from the list";
            this.removeLevelButton.Click += new System.EventHandler(this.removeLevelButton_Click);
            // 
            // levelsListBox
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.levelsListBox, 2);
            this.levelsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.levelsListBox.FormattingEnabled = true;
            this.levelsListBox.Location = new System.Drawing.Point(3, 83);
            this.levelsListBox.Name = "levelsListBox";
            this.levelsListBox.Size = new System.Drawing.Size(273, 193);
            this.levelsListBox.TabIndex = 1;
            this.levelsListBox.SelectedIndexChanged += new System.EventHandler(this.levelsListBox_SelectedIndexChanged);
            // 
            // paintingToolStrip
            // 
            this.paintingToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.paintingToolStrip, 2);
            this.paintingToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.paintingToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.paintingToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paintButton,
            this.eraseButton,
            this.fillButton});
            this.paintingToolStrip.Location = new System.Drawing.Point(0, 30);
            this.paintingToolStrip.Name = "paintingToolStrip";
            this.paintingToolStrip.Size = new System.Drawing.Size(279, 39);
            this.paintingToolStrip.TabIndex = 3;
            this.paintingToolStrip.Text = "toolStrip2";
            // 
            // paintButton
            // 
            this.paintButton.Checked = true;
            this.paintButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.paintButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintButton.Image = global::XNAPlatformerLevelEditor.Properties.Resources.paint;
            this.paintButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintButton.Name = "paintButton";
            this.paintButton.Size = new System.Drawing.Size(36, 36);
            this.paintButton.ToolTipText = "Paint";
            this.paintButton.Click += new System.EventHandler(this.paintButton_Click);
            // 
            // eraseButton
            // 
            this.eraseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.eraseButton.Image = global::XNAPlatformerLevelEditor.Properties.Resources.eraser;
            this.eraseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.eraseButton.Name = "eraseButton";
            this.eraseButton.Size = new System.Drawing.Size(36, 36);
            this.eraseButton.ToolTipText = "Erase";
            this.eraseButton.Click += new System.EventHandler(this.eraseButton_Click);
            // 
            // fillButton
            // 
            this.fillButton.CheckOnClick = true;
            this.fillButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fillButton.Image = global::XNAPlatformerLevelEditor.Properties.Resources.fill;
            this.fillButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fillButton.Name = "fillButton";
            this.fillButton.Size = new System.Drawing.Size(36, 36);
            this.fillButton.ToolTipText = "Fill?";
            // 
            // textureListView
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.textureListView, 2);
            this.textureListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureListView.Location = new System.Drawing.Point(3, 335);
            this.textureListView.MultiSelect = false;
            this.textureListView.Name = "textureListView";
            this.textureListView.Size = new System.Drawing.Size(273, 368);
            this.textureListView.TabIndex = 4;
            this.textureListView.UseCompatibleStateImageBehavior = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Image = global::XNAPlatformerLevelEditor.Properties.Resources.file;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.ToolTipText = "Open an existing level and add it to the level list.";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::XNAPlatformerLevelEditor.Properties.Resources.exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showGridToolStripMenuItem});
            this.viewToolStripMenuItem.Image = global::XNAPlatformerLevelEditor.Properties.Resources.view;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showGridToolStripMenuItem
            // 
            this.showGridToolStripMenuItem.Checked = true;
            this.showGridToolStripMenuItem.CheckOnClick = true;
            this.showGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            this.showGridToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.showGridToolStripMenuItem.Text = "Show Grid";
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileDisplay1.Location = new System.Drawing.Point(3, 3);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(699, 680);
            this.tileDisplay1.TabIndex = 2;
            this.tileDisplay1.Text = "tileDisplay1";
            this.tileDisplay1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tileDisplay1_MouseDown);
            this.tileDisplay1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tileDisplay1_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.spliter);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "Form1";
            this.Text = "XNA Platformer Level Editor";
            this.spliter.Panel1.ResumeLayout(false);
            this.spliter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spliter)).EndInit();
            this.spliter.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.levelsToolStrip.ResumeLayout(false);
            this.levelsToolStrip.PerformLayout();
            this.paintingToolStrip.ResumeLayout(false);
            this.paintingToolStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer spliter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private TileDisplay tileDisplay1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox contentPathTextBox;
        private System.Windows.Forms.ListBox levelsListBox;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
        private System.Windows.Forms.ToolStrip levelsToolStrip;
        private System.Windows.Forms.ToolStripButton addLevelButton;
        private System.Windows.Forms.ToolStrip paintingToolStrip;
        private System.Windows.Forms.ToolStripButton paintButton;
        private System.Windows.Forms.ToolStripButton eraseButton;
        private System.Windows.Forms.ToolStripButton fillButton;
        private System.Windows.Forms.ToolStripButton removeLevelButton;
        private System.Windows.Forms.ListView textureListView;
    }
}

