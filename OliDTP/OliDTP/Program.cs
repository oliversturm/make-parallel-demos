// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OliDTP {
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main( ) {
      WinAPIHelpers.SetProcessDPIAware( );

      Application.EnableVisualStyles( );
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm( ));
    }
  }
}
