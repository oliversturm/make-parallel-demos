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
  public abstract class ElementSizeChangeActiveItem : GrabHandleActiveItem {
    public ElementSizeChangeActiveItem(PresentationModel presentationModel, RenderInfo renderInfo, System.Drawing.Rectangle rectangle)
      : base(presentationModel, rectangle) {
      this.renderInfo = renderInfo;
    }

    private RenderInfo renderInfo;
    public RenderInfo RenderInfo {
      get { return renderInfo; }
    }

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
      return ResizeElement(DragDeltaX, DragDeltaY);
    }

    protected abstract bool ResizeElement(int deltax, int deltay);
    protected abstract System.Drawing.Rectangle GetDragRect( );

    public override void DrawAdornments(Graphics gr) {
      base.DrawAdornments(gr);
      if (DragStarted) {
        gr.DrawRectangle(DragRectPen, GetDragRect( ));
      }
    }
  }

}
