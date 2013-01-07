using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Utils;
using System.Threading.Tasks;

namespace Monocle
{
    class Program
    {
        public static void Main(string[] args)
        {
            GameLoop loop = new GameLoop(60, false);
            loop.Update += (x) => Console.WriteLine(x.Elapsed.TotalSeconds);
            loop.Render += (x) => Console.WriteLine("Dance Monkey Dance!");

            Task.Factory.StartNew(() => loop.StartLoop());
            
            Thread.Sleep(50000000);
        }
    }
}
