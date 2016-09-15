// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime.Types;
using System.Diagnostics;

namespace Extensibility.Python {
  public class PythonExtensionManager {
    public PythonExtensionManager(ExtensionManager extensionManager) {
      this.extensionManager = extensionManager;
      searchPaths = new[] { "." };
    }

    ExtensionManager extensionManager;

    List<IAction> extensions;

    private string[] searchPaths;
    public string[] SearchPaths {
      get { return searchPaths; }
      set {
        searchPaths = value;
      }
    }

    private static ScriptEngine pythonEngine;
    public static ScriptEngine PythonEngine {
      get {
        if (pythonEngine == null) {
          pythonEngine = IronPython.Hosting.Python.CreateEngine( );
          pythonEngine.Runtime.LoadAssembly(typeof(IronPython.Modules.ComplexMath).Assembly);
          pythonEngine.Runtime.LoadAssembly(typeof(System.Drawing.PointF).Assembly);
          pythonEngine.Runtime.LoadAssembly(typeof(PythonPlugin).Assembly);
          pythonEngine.Runtime.LoadAssembly(typeof(Data.Mutable.Document).Assembly);
        }
        return pythonEngine; 
      }
    }
    
    public void UpdateExtensions( ) {
      if (extensions != null) {
        extensionManager.RemoveExtensions(extensions);
        extensions = null;
      }

      extensions = new List<IAction>( );
      foreach (string searchPath in searchPaths) {
        foreach (string filename in Directory.EnumerateFiles(searchPath, "*.py", SearchOption.TopDirectoryOnly)) {
          try {
            var script = PythonEngine.ExecuteFile(filename);
            var pythonTypes = script.GetItems( ).Select(kv => kv.Value).OfType<PythonType>( ).ToList( );
            foreach (var pythonType in pythonTypes) {
              var clrType = pythonType.__clrtype__( );
              if (!clrType.IsAbstract && typeof(PythonAction).IsAssignableFrom(clrType)) {
                // Amazingly, it is possible that the object I get created here does *not* implement
                // all the abstract members of the base classes correctly. Weird but true. I still
                // do the IsAbstract check above, because it helps distinguishing the newly
                // implemented types from the script from those that have been imported as base
                // class types.
                var instance = PythonEngine.Operations.Invoke(pythonType) as IAction;
                extensions.Add(instance);
              }
            }
          }
          catch {
            // All sorts of things could go wrong here, and they should be caught and logged properly.
            // But hey - this is a demo! :-)
            throw;
          }
        }
      }

      extensionManager.AddExtensions(extensions);
    }
  }
}
