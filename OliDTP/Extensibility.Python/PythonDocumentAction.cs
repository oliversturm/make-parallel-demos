// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Scripting.Hosting;

namespace Extensibility.Python {
  public abstract class PythonDocumentAction : PythonAction, IDocumentAction {
    bool IDocumentAction.Execute(Data.Mutable.Document document) {
      lock (document.Lock) {
        return Execute(document);
      }
    }

    public abstract bool Execute(Data.Mutable.Document document);
  }
}
