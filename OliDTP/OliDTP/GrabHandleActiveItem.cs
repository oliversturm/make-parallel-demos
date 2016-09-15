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

namespace OliDTP {
  public abstract class GrabHandleActiveItem : ActiveItem {
    public GrabHandleActiveItem(PresentationModel presentationModel, System.Drawing.Rectangle rectangle) : base(presentationModel, rectangle) { }

    protected virtual void DrawHandle(Graphics gr, System.Drawing.Rectangle rect) {
      gr.FillRectangle(Brushes.White, rect);
      gr.DrawRectangle(Pens.Black, rect);
    }

    public override void DrawAdornments(Graphics gr) {
      DrawHandle(gr, this.Rectangle);
    }
  }
}
