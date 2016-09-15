// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Data.Mutable;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Extensibility;
using Extensibility.Python;

namespace OliDTP {
  public partial class MainForm : Form {
    public MainForm( ) {
      InitializeComponent( );

      extensionManager = new ExtensionManager( );
      pythonExtensionManager = new PythonExtensionManager(extensionManager);
      pythonExtensionManager.SearchPaths = new List<string>(pythonExtensionManager.SearchPaths) { @"..\..\python" }.ToArray( );
      pythonExtensionManager.UpdateExtensions( );
      UpdateExtensionMenu( );

      InitDocument( );
      UpdateLayerList( );

      presentationModel = new PresentationModel(document, ZoomMode.WholePage, 100.0f, contentPanel.Width, contentPanel.Height);
      contentPanel.PresentationModel = presentationModel;
      
      zoomSelection.SelectedIndex = 0;
    }


    Document document;
    PresentationModel presentationModel;

    ExtensionManager extensionManager;
    PythonExtensionManager pythonExtensionManager;

    private void InitDocument( ) {
      document = new Document {
        Size = new SizeF(12, 12),
        Layers = new List<Layer> {
      		          new Layer {
                      Name = "Layer 1",
                      Visible = true,
      		            ZOrder = 0,
      		            Elements = new List<Element>{
      		              new Data.Mutable.Rectangle{
      		                Location = new PointF(3,3),
      		                Size = new SizeF(4,3),
      		                ZOrder = 0
      		              },
      		              new Data.Mutable.Ellipse{
      		                Location = new PointF(2,2),
      		                Size = new SizeF(4,8),
      		                ZOrder = 0
      		              }
                      }
                    },
                    new Layer{
                      Name = "Layer 2",
                      Visible = true,
                      ZOrder = 1,
      		            Elements = new List<Element>{
      		              new Data.Mutable.Rectangle{
      		                Location = new PointF(5,5),
      		                Size = new SizeF(2.8f, 6),
      		                ZOrder = 0
      		              },
                        new Data.Mutable.BitmapImage{
                          Location = new PointF(3.5f, 3.5f),
                          Size = new SizeF(2.5f, 1.5f),
                          ZOrder = -1,
                          Filename=@"..\..\..\images\postbox.jpg"
                        },
                        new Data.Mutable.BitmapImage{
                          Location = new PointF(3.5f, 2.5f),
                          Size = new SizeF(2.5f, 1.5f),
                          ZOrder = 1,
                          Filename=@"..\..\..\images\beef1.jpg"
                        },
                        new Data.Mutable.BitmapImage{
                          Location = new PointF(3.5f, 1.5f),
                          Size = new SizeF(3.5f, 1.5f),
                          ZOrder = 2,
                          Filename=@"..\..\..\images\defender.jpg"
                        },
                        new Data.Mutable.BitmapImage{
                          Location = new PointF(1.5f, 3.5f),
                          Size = new SizeF(4.5f, 1f),
                          ZOrder = 3,
                          Filename=@"..\..\..\images\island.jpg"
                        }

                      }
                    }
      		        }
      };
    }


    private void UpdateLayerList( ) {
      layerList.Items.Clear( );
      foreach (var layer in document.Layers.OrderByDescending(l=>l.ZOrder))
        layerList.Items.Add(layer, layer.Visible);
    }


    private void SetPercentageZoom(float percentage) {
      presentationModel.SuspendUpdate( );
      try {
        presentationModel.ZoomMode = ZoomMode.Percentage;
        presentationModel.ZoomPercentage = percentage;
        zoomPercentage.Text = String.Format("{0} %", (int) percentage);
      }
      finally {
        presentationModel.ResumeUpdate(true);        
      }
    }

    private void zoomSelection_SelectedIndexChanged(object sender, EventArgs e) {
      presentationModel.SuspendUpdate( );

      try {
        switch (zoomSelection.SelectedIndex) {
          case 0:
            // Whole page
            presentationModel.ZoomMode = ZoomMode.WholePage;
            break;
          case 1:
            // Page width
            presentationModel.ZoomMode = ZoomMode.PageWidth;
            break;

          case 2:
            // Disproportional
            presentationModel.ZoomMode = ZoomMode.Disproportional;
            break;

          case 3:
            // Percentage
            presentationModel.ZoomMode = ZoomMode.Percentage;
            presentationModel.ZoomPercentage = GetPercentage( );
            break;

          case 4:
            // 100%
            SetPercentageZoom(100.0f);
            break;
          case 5:
            //80%
            SetPercentageZoom(80.0f);
            break;
          case 6:
            //60%
            SetPercentageZoom(60.0f);
            break;
          case 7:
            //40%
            SetPercentageZoom(40.0f);
            break;
          case 8:
            //20%
            SetPercentageZoom(20.0f);
            break;
          case 9:
            //200%
            SetPercentageZoom(200.0f);
            break;
          case 10:
            //500%
            SetPercentageZoom(500.0f);
            break;
          case 11:
            //1000%
            SetPercentageZoom(1000.0f);
            break;
        }
      }
      finally {
        presentationModel.ResumeUpdate(true);
      }
    }

    private void zoomPercentage_Validating(object sender, CancelEventArgs e) {
      e.Cancel = !Regex.IsMatch(zoomPercentage.Text, @"^\d+\s*%?$");
    }

    float GetPercentage( ) {
      var match = Regex.Match(zoomPercentage.Text, @"^(\d+)\s*%?$");
      if (match.Success) {
        return Convert.ToSingle(match.Groups[1].Value);
      }
      else
        return 100.0f;
    }

    private void addRectButton_Click(object sender, EventArgs e) {
      AddDefaultRectangle( );
    }

    private void addEllipseButton_Click(object sender, EventArgs e) {
      AddDefaultEllipse( );
    }

    private void addImageButton_Click(object sender, EventArgs e) {
      AddDefaultImage( );
    }

    private void AddDefaultRectangle( ) {
      presentationModel.AddDocumentElement(new Data.Mutable.Rectangle {
        Location = GetDefaultNewItemPos( ),
        Size = GetDefaultNewItemSize( )
      });
    }

    private void AddDefaultEllipse( ) {
      presentationModel.AddDocumentElement(new Data.Mutable.Ellipse {
        Location = GetDefaultNewItemPos( ),
        Size = GetDefaultNewItemSize( )
      });
    }

    private void AddDefaultImage( ) {
      using (var dlg = new OpenFileDialog()) {
        dlg.Filter = "All Files|*.*";
        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
          presentationModel.AddDocumentElement(new Data.Mutable.BitmapImage {
            Location = GetDefaultNewItemPos( ),
            Size = GetDefaultNewItemSize( ),
            Filename = dlg.FileName
          });
        }
      }
    }

    private void layerList_SelectedIndexChanged(object sender, EventArgs e) {
      presentationModel.ActiveLayer = layerList.Items[layerList.SelectedIndex] as Layer;
    }


    private PointF GetDefaultNewItemPos( ) {
      return new PointF(document.Size.Width / 10, document.Size.Height / 10);
    }

    private SizeF GetDefaultNewItemSize( ) {
      return new SizeF(document.Size.Width / 5, document.Size.Height / 5);
    }

    private void layerList_ItemCheck(object sender, ItemCheckEventArgs e) {
      var layer = layerList.Items[e.Index] as Layer;
      if (presentationModel != null && layer != null)
        presentationModel.SetLayerVisibility(layer, e.NewValue == CheckState.Checked);
    }

    private void UpdateExtensionMenu( ) {
      extensionsMenu.DropDownItems.Clear( );

      foreach (var docAction in extensionManager.Extensions.OfType<IDocumentAction>()) {
        try {
          var name = ((IAction) docAction).Name;
          var menuItem = extensionsMenu.DropDownItems.Add(name);
          menuItem.Tag = docAction;
          menuItem.Click += new EventHandler(docActionExtensionMenuItem_Click);
        }
        catch (MissingMemberException) {
          // Nothing much I can do here - I should log this etc etc, but this is a demo.
          throw;
        }
      }
    }

    void docActionExtensionMenuItem_Click(object sender, EventArgs e) {
      var item = sender as ToolStripItem;
      if (item != null) {
        var action = item.Tag as IDocumentAction;
        if (action != null) {
          try {
            if (action.Execute(document)) {
              presentationModel.UpdateRenderImage( );
              UpdateLayerList( );
            }
          }
          catch (MissingMemberException) {
            // Nothing much I can do here - I should log this etc etc, but this is a demo.
            throw;
          }
        }
      }
    }

  }
}