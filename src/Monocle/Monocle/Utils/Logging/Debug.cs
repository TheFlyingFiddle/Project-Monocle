using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils.Logging
{
    public static class Debug
    {
        private static ILogger logger;

        static Debug()
        {
            logger = new ConsoleLogger();
        }

        public static void LogInfo(string info)
        {
            logger.LogInfo(info);
        }

        public static void LogInfo(string formatedInfo, params object[] param)
        {
            logger.LogInfo(formatedInfo, param);
        }

        public static void LogException(Exception e)
        {
            logger.LogException(e);
        }
    }
}
