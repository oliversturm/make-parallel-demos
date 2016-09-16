using Shielded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShieldedDemo {
  class Program {
    static void Main(string[] args) {
      var shieldedVal = new Shielded<int>(42);

      // this renders an exception - we're not in a transaction
      //shieldedVal.Modify((ref int x) => x = 100);

      // changes in transactions are possible
      Shield.InTransaction(() => {
        shieldedVal.Modify((ref int x) => x = 100);
      });
      Console.WriteLine($"Value is {shieldedVal.Value}");

      // RunToCommit leaves the commit decision for later
      using (var cont = Shield.RunToCommit(0, () => {
        shieldedVal.Value = 99;
      })) {
        //cont.Rollback();
        //cont.Commit();
      }
      Console.WriteLine($"Value is {shieldedVal.Value}");

      Shield.InTransaction(() => {
        shieldedVal.Modify((ref int x) => x = 777);
        Console.WriteLine($"Value outside the thread is {shieldedVal.Value}");

        var thread = new Thread(() => {
          // the thread gets its own isolated view of the value automatically,
          // so this will render the value from before the transaction
          Console.WriteLine($"Value inside the thread is {shieldedVal.Value}");
        });
        thread.Start();
        thread.Join();

        Console.WriteLine($"Value after the thread is {shieldedVal.Value}");
      });

    }
  }
}
