// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Data.Mutable {
  public class Document {
    public Document( ) {
      Layers = new List<Layer>( );
    }

    public readonly object Lock = new object( );
    public SizeF Size { get; set; }
    public List<Layer> Layers { get; set; }
  }
}
