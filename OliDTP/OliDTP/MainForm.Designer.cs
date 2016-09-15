namespace OliDTP {
  partial class MainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose( );
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( ) {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.toolStrip1 = new System.Windows.Forms.ToolStrip( );
      this.zoomSelection = new System.Windows.Forms.ToolStripComboBox( );
      this.zoomPercentage = new System.Windows.Forms.ToolStripTextBox( );
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator( );
      this.addRectButton = new System.Windows.Forms.ToolStripButton( );
      this.addEllipseButton = new System.Windows.Forms.ToolStripButton( );
      this.addImageButton = new System.Windows.Forms.ToolStripButton( );
      this.statusStrip1 = new System.Windows.Forms.StatusStrip( );
      this.outerSplitContainer = new System.Windows.Forms.SplitContainer( );
      this.layerList = new System.Windows.Forms.CheckedListBox( );
      this.contentPanel = new OliDTP.ContentPanel( );
      this.menuStrip1 = new System.Windows.Forms.MenuStrip( );
      this.extensionsMenu = new System.Windows.Forms.ToolStripMenuItem( );
      this.toolStrip1.SuspendLayout( );
      ((System.ComponentModel.ISupportInitialize) (this.outerSplitContainer)).BeginInit( );
      this.outerSplitContainer.Panel1.SuspendLayout( );
      this.outerSplitContainer.Panel2.SuspendLayout( );
      this.outerSplitContainer.SuspendLayout( );
      this.menuStrip1.SuspendLayout( );
      this.SuspendLayout( );
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomSelection,
            this.zoomPercentage,
            this.toolStripSeparator1,
            this.addRectButton,
            this.addEllipseButton,
            this.addImageButton});
      this.toolStrip1.Location = new System.Drawing.Point(0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(677, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // zoomSelection
      // 
      this.zoomSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.zoomSelection.Items.AddRange(new object[] {
            "Whole Page",
            "Page Width",
            "Disproportional",
            "Percentage",
            "100%",
            "80%",
            "60%",
            "40%",
            "20%",
            "200%",
            "500%",
            "1000%"});
      this.zoomSelection.Name = "zoomSelection";
      this.zoomSelection.Size = new System.Drawing.Size(121, 25);
      this.zoomSelection.SelectedIndexChanged += new System.EventHandler(this.zoomSelection_SelectedIndexChanged);
      // 
      // zoomPercentage
      // 
      this.zoomPercentage.Name = "zoomPercentage";
      this.zoomPercentage.Size = new System.Drawing.Size(50, 25);
      this.zoomPercentage.Text = "100%";
      this.zoomPercentage.Validating += new System.ComponentModel.CancelEventHandler(this.zoomPercentage_Validating);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // addRectButton
      // 
      this.addRectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.addRectButton.Image = ((System.Drawing.Image) (resources.GetObject("addRectButton.Image")));
      this.addRectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addRectButton.Name = "addRectButton";
      this.addRectButton.Size = new System.Drawing.Size(55, 22);
      this.addRectButton.Text = "Add Rect";
      this.addRectButton.Click += new System.EventHandler(this.addRectButton_Click);
      // 
      // addEllipseButton
      // 
      this.addEllipseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.addEllipseButton.Image = ((System.Drawing.Image) (resources.GetObject("addEllipseButton.Image")));
      this.addEllipseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addEllipseButton.Name = "addEllipseButton";
      this.addEllipseButton.Size = new System.Drawing.Size(62, 22);
      this.addEllipseButton.Text = "Add Ellipse";
      this.addEllipseButton.Click += new System.EventHandler(this.addEllipseButton_Click);
      // 
      // addImageButton
      // 
      this.addImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.addImageButton.Image = ((System.Drawing.Image) (resources.GetObject("addImageButton.Image")));
      this.addImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addImageButton.Name = "addImageButton";
      this.addImageButton.Size = new System.Drawing.Size(63, 22);
      this.addImageButton.Text = "Add Image";
      this.addImageButton.Click += new System.EventHandler(this.addImageButton_Click);
      // 
      // statusStrip1
      // 
      this.statusStrip1.Location = new System.Drawing.Point(0, 502);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(677, 22);
      this.statusStrip1.TabIndex = 2;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // outerSplitContainer
      // 
      this.outerSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.outerSplitContainer.Location = new System.Drawing.Point(0, 49);
      this.outerSplitContainer.Name = "outerSplitContainer";
      // 
      // outerSplitContainer.Panel1
      // 
      this.outerSplitContainer.Panel1.Controls.Add(this.layerList);
      this.outerSplitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
      // 
      // outerSplitContainer.Panel2
      // 
      this.outerSplitContainer.Panel2.Controls.Add(this.contentPanel);
      this.outerSplitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.outerSplitContainer.Size = new System.Drawing.Size(677, 453);
      this.outerSplitContainer.SplitterDistance = 150;
      this.outerSplitContainer.TabIndex = 3;
      // 
      // layerList
      // 
      this.layerList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.layerList.FormattingEnabled = true;
      this.layerList.Location = new System.Drawing.Point(0, 0);
      this.layerList.Name = "layerList";
      this.layerList.Size = new System.Drawing.Size(150, 453);
      this.layerList.TabIndex = 0;
      this.layerList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.layerList_ItemCheck);
      this.layerList.SelectedIndexChanged += new System.EventHandler(this.layerList_SelectedIndexChanged);
      // 
      // contentPanel
      // 
      this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.contentPanel.Location = new System.Drawing.Point(0, 0);
      this.contentPanel.Name = "contentPanel";
      this.contentPanel.PresentationModel = null;
      this.contentPanel.Size = new System.Drawing.Size(523, 453);
      this.contentPanel.TabIndex = 0;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extensionsMenu});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(677, 24);
      this.menuStrip1.TabIndex = 4;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // extensionsMenu
      // 
      this.extensionsMenu.Name = "extensionsMenu";
      this.extensionsMenu.Size = new System.Drawing.Size(71, 20);
      this.extensionsMenu.Text = "Extensions";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(677, 524);
      this.Controls.Add(this.outerSplitContainer);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "MainForm";
      this.Text = "OliDTP";
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout( );
      this.outerSplitContainer.Panel1.ResumeLayout(false);
      this.outerSplitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize) (this.outerSplitContainer)).EndInit( );
      this.outerSplitContainer.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout( );
      this.ResumeLayout(false);
      this.PerformLayout( );

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.SplitContainer outerSplitContainer;
    private System.Windows.Forms.ToolStripComboBox zoomSelection;
    private System.Windows.Forms.ToolStripTextBox zoomPercentage;
    private ContentPanel contentPanel;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton addRectButton;
    private System.Windows.Forms.ToolStripButton addEllipseButton;
    private System.Windows.Forms.ToolStripButton addImageButton;
    private System.Windows.Forms.CheckedListBox layerList;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem extensionsMenu;
  }
}

