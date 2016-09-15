// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Mutable;

namespace Extensibility {
  public class ExtensionManager {
    public ExtensionManager( ) {
      extensions = new List<IAction>( );  
    }

    List<IAction> extensions;
    public List<IAction> Extensions {
      get { return extensions; }
    }

    public void AddExtensions(IEnumerable<IAction> extensions) {
      this.extensions.AddRange(extensions);
    }

    public void RemoveExtensions(IEnumerable<IAction> extensions) {
      foreach (IAction extension in extensions)
        this.extensions.Remove(extension);
    }
  }
}