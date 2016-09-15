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
using System.Collections;

using Rendering;


namespace OliDTP {
  public class ElementMoveActiveItem : GrabHandleActiveItem {
    public ElementMoveActiveItem(PresentationModel presentationModel, RenderInfo renderInfo)
      : base(presentationModel, System.Drawing.Rectangle.Inflate(renderInfo.Rect, -2, -2)) {
      this.renderInfo = renderInfo;
    }

    private RenderInfo renderInfo;

    public override bool NotifyLeftMouseButtonClicked( ) {
      return false;
    }

    public override bool NotifyLeftMouseDragStart(int x, int y) {
      base.NotifyLeftMouseDragStart(x, y);
      return true;
    }

    public override bool NotifyLeftMouseDragMove(int x, int y) {
      base.NotifyLeftMouseDragMove(x, y);
      return true;
    }

    public override bool NotifyLeftMouseDragEnd(int x, int y) {
      base.NotifyLeftMouseDragEnd(x, y);
      return MoveElement(DragDeltaX, DragDeltaY);
    }

    public override void DrawAdornments(Graphics gr) {
      // no standard drawing for the mover
      //base.DrawAdornments(gr);
      if (DragStarted) {
        gr.DrawRectangle(DragRectPen, GetDragRect( ));
      }
    }

    private bool MoveElement(int deltax, int deltay) {
      float deltaxf = deltax / PresentationModel.DPIX, deltayf = deltay / PresentationModel.DPIY;
      var element = renderInfo.Element.Source;
      element.Location = new PointF(element.Location.X + deltaxf, element.Location.Y + deltayf);
      return true;
    }

    private System.Drawing.Rectangle GetDragRect( ) {
      return new System.Drawing.Rectangle(
        new Point(renderInfo.Rect.X + DragDeltaX, renderInfo.Rect.Y + DragDeltaY),
        renderInfo.Rect.Size);
    }

    public override Cursor MouseCursor {
      get { return Cursors.SizeAll; }
    }
  }
}
