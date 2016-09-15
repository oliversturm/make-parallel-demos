// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Data.Mutable {
  abstract public class Element {
    public Element() {
      Source = this;
    }
    public PointF Location { get; set; }
    public SizeF Size { get; set; }
    public int ZOrder { get; set; }
    public Element Source { get; set; }
  }
}