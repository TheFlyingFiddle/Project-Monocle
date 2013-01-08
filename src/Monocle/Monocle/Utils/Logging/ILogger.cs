using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Utils.Logging
{
    interface ILogger
    {
        void LogInfo(string info);
        void LogInfo(string info, params object[] param);
        void LogException(Exception e);
    }
}
