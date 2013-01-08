using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils.Logging
{
    class ConsoleLogger : ILogger
    {
        public void LogInfo(string info)
        {
            Console.WriteLine(info);
        }

        public void LogInfo(string formatedInfo, params object[] param)
        {
            Console.WriteLine(formatedInfo, param);
        }

        public void LogException(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
