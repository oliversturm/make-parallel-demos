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
  public class CornerGrabHandle : ElementSizeChangeActiveItem {
    public CornerGrabHandle(PresentationModel presentationModel,
      RenderInfo renderInfo,
      System.Drawing.Rectangle selectionRectangle,
      CornerGrabHandleType cornerGrabHandleType) :
      base(presentationModel, renderInfo, CalcGrabHandleRect(selectionRectangle, cornerGrabHandleType)) {
      this.cornerGrabHandleType = cornerGrabHandleType;
    }

    static System.Drawing.Rectangle CalcGrabHandleRect(System.Drawing.Rectangle selectionRectangle,
      CornerGrabHandleType cornerGrabHandleType) {
      // I don't want to set these here, but the compiler is too dumb to understand
      // that they are guaranteed to be set by the following switch block.
      int x = 0, y = 0;
      switch (cornerGrabHandleType) {
        case CornerGrabHandleType.TopLeft:
          x = selectionRectangle.Left;
          y = selectionRectangle.Top;
          break;
        case CornerGrabHandleType.TopRight:
          x = selectionRectangle.Right;
          y = selectionRectangle.Top;
          break;
        case CornerGrabHandleType.BottomLeft:
          x = selectionRectangle.Left;
          y = selectionRectangle.Bottom;
          break;
        case CornerGrabHandleType.BottomRight:
          x = selectionRectangle.Right;
          y = selectionRectangle.Bottom;
          break;
      }
      return new System.Drawing.Rectangle(x - 4, y - 4, 8, 8);
    }

    private CornerGrabHandleType cornerGrabHandleType;
    public CornerGrabHandleType CornerGrabHandleType {
      get { return cornerGrabHandleType; }
    }

    public override Cursor MouseCursor {
      get {
        return cornerGrabHandleType == CornerGrabHandleType.BottomLeft || cornerGrabHandleType == CornerGrabHandleType.TopRight ?
          Cursors.SizeNESW : Cursors.SizeNWSE;
      }
    }

    protected override bool ResizeElement(int deltax, int deltay) {
      float deltaxf = deltax / PresentationModel.DPIX, deltayf = deltay / PresentationModel.DPIY;
      var element = RenderInfo.Element.Source;
      switch (cornerGrabHandleType) {
        case CornerGrabHandleType.TopLeft:
          element.Location = new PointF(element.Location.X + deltaxf, element.Location.Y + deltayf);
          element.Size = new SizeF(element.Size.Width - deltaxf, element.Size.Height - deltayf);
          break;
        case CornerGrabHandleType.TopRight:
          element.Location = new PointF(element.Location.X, element.Location.Y + deltayf);
          element.Size = new SizeF(element.Size.Width + deltaxf, element.Size.Height - deltayf);
          break;
        case CornerGrabHandleType.BottomLeft:
          element.Location = new PointF(element.Location.X + deltaxf, element.Location.Y);
          element.Size = new SizeF(element.Size.Width - deltaxf, element.Size.Height + deltayf);
          break;
        case CornerGrabHandleType.BottomRight:
          element.Size = new SizeF(element.Size.Width + deltaxf, element.Size.Height + deltayf);
          break;
      }
      return true;
    }

    protected override System.Drawing.Rectangle GetDragRect( ) {
      var rirect = RenderInfo.Rect;
      switch (cornerGrabHandleType) {
        case CornerGrabHandleType.TopLeft:
          return new System.Drawing.Rectangle(
            rirect.X + DragDeltaX, rirect.Y + DragDeltaY,
            rirect.Width - DragDeltaX, rirect.Height - DragDeltaY);
        case CornerGrabHandleType.TopRight:
          return new System.Drawing.Rectangle(
            rirect.X, rirect.Y + DragDeltaY,
            rirect.Width + DragDeltaX, rirect.Height - DragDeltaY
            );
        case CornerGrabHandleType.BottomLeft:
          return new System.Drawing.Rectangle(
            rirect.X + DragDeltaX, rirect.Y,
            rirect.Width - DragDeltaX, rirect.Height + DragDeltaY
            );
        case CornerGrabHandleType.BottomRight:
          return new System.Drawing.Rectangle(
            rirect.X, rirect.Y,
            rirect.Width + DragDeltaX, rirect.Height + DragDeltaY
            );
        default:
          // Why is C# too dumb to find that I don't need a default case
          // because each of the cases in the enum already has a return
          // statement? I don't know...
          return System.Drawing.Rectangle.Empty;
      }
    }
  }
}
