using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    class InvalidTypeException : Exception
    {
        public InvalidTypeException(string p)
            : base(p)
        {
        }
    }
}
