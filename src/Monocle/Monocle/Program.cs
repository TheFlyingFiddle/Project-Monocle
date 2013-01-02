using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Utils;
using Editor;


namespace Monocle
{
    class Program
    {
        public static void Main(string[] args)
        {
            MessageSender.CacheAssemblieMessages(typeof(Program).Assembly);
            
            var fsm = new FSMEditor().BuildFSM();
            fsm.Start();

            while (true)
            {
                fsm.SendMessage("HandleCommand", Console.ReadLine());
            }
        }
    }
}
