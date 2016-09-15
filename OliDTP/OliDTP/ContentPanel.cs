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

namespace OliDTP {
  public class ContentPanel : Panel {
    public ContentPanel( )
      : base( ) {
      SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
    }

    private PresentationModel presentationModel;
    public PresentationModel PresentationModel {
      get { return presentationModel; }
      set {
        if (!object.ReferenceEquals(presentationModel, value)) {
          if (presentationModel != null) {
            presentationModel.BitmapChanged -= new EventHandler(presentationModel_BitmapChanged);
          }

          presentationModel = value;
          
          if (presentationModel != null) {
            presentationModel.BitmapChanged += new EventHandler(presentationModel_BitmapChanged);
          }
        }
      }
    }

    void presentationModel_BitmapChanged(object sender, EventArgs e) {
      if (presentationModel.ZoomMode == ZoomMode.PageWidth ||
        presentationModel.ZoomMode == ZoomMode.Percentage)
        AutoScrollMinSize = new Size(
          presentationModel.Bitmap.Size.Width - 1,
          presentationModel.Bitmap.Size.Height - 1);
      else
        AutoScrollMinSize = Size.Empty;

      Invalidate( );
    }

    protected override void OnPaint(PaintEventArgs e) {
      // One would think that the AutoScrollPosition would be positive values - 
      // i.e. the point within the logical surface that is currently my visible
      // (0/0) according to scrolling. But it's not - it's negative values,
      // something like "how far above and left of the visual (0/0) is the 
      // logical (0/0) right now". Fair enough, but important to know.

      if (presentationModel != null) {
        e.Graphics.DrawImageUnscaled(presentationModel.Bitmap,
          AutoScrollPosition.X,
          AutoScrollPosition.Y);
      }
    }

    protected override void OnResize(EventArgs eventargs) {
      base.OnResize(eventargs);

      if (presentationModel != null) {
        presentationModel.SuspendUpdate( );
        try {
          presentationModel.ViewContainerWidth = ClientSize.Width;
          presentationModel.ViewContainerHeight = ClientSize.Height;
        }
        finally {
          presentationModel.ResumeUpdate(presentationModel.ZoomMode != ZoomMode.Percentage);
        }
      }
    }

    // these variables relate only to the left mouse button
    int mouseDownX = -1, mouseDownY = -1;
    bool mouseDown = false;
    bool dragStarted = false;

    protected override void OnMouseDown(MouseEventArgs e) {
      base.OnMouseDown(e);
      if (e.Button == MouseButtons.Left) {
        mouseDownX = e.X;
        mouseDownY = e.Y;
        mouseDown = true;
      }
    }

    protected override void OnMouseUp(MouseEventArgs e) {
      base.OnMouseUp(e);

      if (presentationModel != null) {
        if (e.Button == MouseButtons.Left) {
          // One would think that the AutoScrollPosition would be positive values - 
          // i.e. the point within the logical surface that is currently my visible
          // (0/0) according to scrolling. But it's not - it's negative values,
          // something like "how far above and left of the visual (0/0) is the 
          // logical (0/0) right now". Fair enough, but important to know.

          if (dragStarted) {
            presentationModel.NotifyLeftMouseDragEnd(
              e.X + (-AutoScrollPosition.X),
              e.Y + (-AutoScrollPosition.Y));
            dragStarted = false;
          }
          else
          presentationModel.NotifyLeftMouseButtonClicked(
            e.X + (-AutoScrollPosition.X),
            e.Y + (-AutoScrollPosition.Y));
        }
      }

      mouseDown = false;
    }

    protected override void OnMouseMove(MouseEventArgs e) {
      base.OnMouseMove(e);

      const int DRAGDELTA = 3;

      if (presentationModel != null) {
        if (mouseDown) {
          if (dragStarted) {
            dragStarted = presentationModel.NotifyLeftMouseDragMove(e.X + (-AutoScrollPosition.X),
                e.Y + (-AutoScrollPosition.Y));
          }
          else {
            if (Math.Abs(e.X - mouseDownX) > DRAGDELTA || Math.Abs(e.Y - mouseDownY) > DRAGDELTA) {
              dragStarted = presentationModel.NotifyLeftMouseDragStart(mouseDownX + (-AutoScrollPosition.X),
                mouseDownY + (-AutoScrollPosition.Y));
            }
          }
        }

        Cursor = presentationModel.CalcMouseCursor(
              e.X + (-AutoScrollPosition.X),
              e.Y + (-AutoScrollPosition.Y));
      }
    }
  }
}
