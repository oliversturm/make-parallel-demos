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
using System.Drawing.Drawing2D;

namespace OliDTP {
  public abstract class ActiveItem {
    public ActiveItem(PresentationModel presentationModel, System.Drawing.Rectangle rectangle) {
      this.presentationModel = presentationModel;
      this.rectangle = rectangle;
    }

    public abstract void DrawAdornments(Graphics gr);
    public abstract Cursor MouseCursor { get; }
    public abstract bool NotifyLeftMouseButtonClicked( );

    private System.Drawing.Rectangle rectangle;
    public virtual System.Drawing.Rectangle Rectangle {
      get { return rectangle; }
    }

    private PresentationModel presentationModel;
    public virtual PresentationModel PresentationModel {
      get {
        return presentationModel;
      }
    }

    static Pen dragRectPen;
    public static Pen DragRectPen {
      get {
        if (dragRectPen == null) {
          dragRectPen = new Pen(Color.Black);
          dragRectPen.DashStyle = DashStyle.Dot;
        }
        return dragRectPen;
      }
    }

    int dragStartX, dragStartY, dragCurrentX, dragCurrentY, dragDeltaX, dragDeltaY;
    protected int DragStartX {
      get { return dragStartX; }
    }
    protected int DragStartY {
      get { return dragStartY; }
    }
    protected int DragCurrentX {
      get { return dragCurrentX; }
    }
    protected int DragCurrentY {
      get { return dragCurrentY; }
    }
    protected int DragDeltaX {
      get { return dragDeltaX; }
    }
    protected int DragDeltaY {
      get { return dragDeltaY; }
    }

    private bool dragStarted;
    protected bool DragStarted {
      get { return dragStarted; }
    }
    

    public virtual bool NotifyLeftMouseDragStart(int x, int y) {
      dragStartX = x;
      dragStartY = y;
      StoreCurrentCoords(x, y);
      dragStarted = true;
      return false;
    }

    private void StoreCurrentCoords(int x, int y) {
      dragCurrentX = x;
      dragCurrentY = y;
      dragDeltaX = dragCurrentX - dragStartX;
      dragDeltaY = dragCurrentY - dragStartY;
    }

    public virtual bool NotifyLeftMouseDragMove(int x, int y) {
      StoreCurrentCoords(x, y);
      return false;
    }

    public virtual bool NotifyLeftMouseDragEnd(int x, int y) {
      StoreCurrentCoords(x, y);
      dragStarted = false;
      return false;
    }

  }
}
