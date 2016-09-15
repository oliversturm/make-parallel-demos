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
  public class BorderGrabHandle : ElementSizeChangeActiveItem {
    public BorderGrabHandle(PresentationModel presentationModel,
      RenderInfo renderInfo,
      System.Drawing.Rectangle selectionRectangle,
      BorderGrabHandleType borderGrabHandleType) :
      base(presentationModel, renderInfo, CalcGrabHandleRect(selectionRectangle, borderGrabHandleType)) {
      this.borderGrabHandleType = borderGrabHandleType;
    }

    static System.Drawing.Rectangle CalcGrabHandleRect(System.Drawing.Rectangle selectionRectangle,
      BorderGrabHandleType borderGrabHandleType) {
      // I don't want to set these here, but the compiler is too dumb to understand
      // that they are guaranteed to be set by the following switch block.
      int x = 0, y = 0;
      switch (borderGrabHandleType) {
        case BorderGrabHandleType.Top:
          x = selectionRectangle.Left + (selectionRectangle.Width / 2);
          y = selectionRectangle.Top;
          break;
        case BorderGrabHandleType.Bottom:
          x = selectionRectangle.Left + (selectionRectangle.Width / 2);
          y = selectionRectangle.Bottom;
          break;
        case BorderGrabHandleType.Left:
          x = selectionRectangle.Left;
          y = selectionRectangle.Top + (selectionRectangle.Height / 2);
          break;
        case BorderGrabHandleType.Right:
          x = selectionRectangle.Right;
          y = selectionRectangle.Top + (selectionRectangle.Height / 2);
          break;
      }
      return new System.Drawing.Rectangle(x - 3, y - 3, 6, 6);
    }

    private BorderGrabHandleType borderGrabHandleType;
    public BorderGrabHandleType BorderGrabHandleType {
      get { return borderGrabHandleType; }
    }

    public override Cursor MouseCursor {
      get {
        return borderGrabHandleType == BorderGrabHandleType.Bottom || borderGrabHandleType == BorderGrabHandleType.Top ?
          Cursors.SizeNS : Cursors.SizeWE;
      }
    }

    protected override bool ResizeElement(int deltax, int deltay) {
      float deltaxf = deltax / PresentationModel.DPIX, deltayf = deltay / PresentationModel.DPIY;
      var element = RenderInfo.Element.Source;
      switch (borderGrabHandleType) {
        case BorderGrabHandleType.Top:
          element.Location = new PointF(element.Location.X, element.Location.Y + deltayf);
          element.Size = new SizeF(element.Size.Width, element.Size.Height - deltayf);
          break;
        case BorderGrabHandleType.Bottom:
          element.Size = new SizeF(element.Size.Width, element.Size.Height + deltayf);
          break;
        case BorderGrabHandleType.Left:
          element.Location = new PointF(element.Location.X + deltaxf, element.Location.Y);
          element.Size = new SizeF(element.Size.Width - deltaxf, element.Size.Height);
          break;
        case BorderGrabHandleType.Right:
          element.Size = new SizeF(element.Size.Width + deltaxf, element.Size.Height);
          break;
      }
      return true;
    }

    protected override System.Drawing.Rectangle GetDragRect( ) {
      var rirect = RenderInfo.Rect;
      switch (borderGrabHandleType) {
        case BorderGrabHandleType.Top:
          return new System.Drawing.Rectangle(
            rirect.X, rirect.Y + DragDeltaY,
            rirect.Width, rirect.Height - DragDeltaY);
        case BorderGrabHandleType.Bottom:
          return new System.Drawing.Rectangle(
            rirect.X, rirect.Y,
            rirect.Width, rirect.Height + DragDeltaY);
        case BorderGrabHandleType.Left:
          return new System.Drawing.Rectangle(
            rirect.X + DragDeltaX, rirect.Y,
            rirect.Width - DragDeltaX, rirect.Height);
        case BorderGrabHandleType.Right:
          return new System.Drawing.Rectangle(
            rirect.X, rirect.Y,
            rirect.Width + DragDeltaX, rirect.Height);
        default:
          // Why is C# too dumb to find that I don't need a default case
          // because each of the cases in the enum already has a return
          // statement? I don't know...
          return System.Drawing.Rectangle.Empty;
      }
    }
  }
}
