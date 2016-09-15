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

namespace OliDTP {
  public enum ZoomMode {
    Percentage,
    WholePage,
    PageWidth,
    Disproportional
  }
}
