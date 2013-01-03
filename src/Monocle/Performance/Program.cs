using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Performance
{
    class Program
    {
        public static void Main(string[] args)
        {
            string line;
            while ((line = Console.ReadLine()) != "exit")
            {
                string[] command = line.Split(' ');
                if (command.Length == 3)
                {
                    try
                    {
                        new MessagePerformance().Run(long.Parse(command[1]), long.Parse(command[2]));
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
