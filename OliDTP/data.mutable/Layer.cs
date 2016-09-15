// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Data.Mutable {
  public class Layer {
    public Layer( ) {
      Elements = new List<Element>( );
    }

    public List<Element> Elements { get; set; }
    public int ZOrder { get; set; }
    public string Name { get; set; }
    public bool Visible { get; set; }

    public override string ToString( ) {
      return Name;
    }
  }
}
