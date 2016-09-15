// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Mutable;

namespace Extensibility {
  public interface IAction {
    string Name { get; }
  }
}
