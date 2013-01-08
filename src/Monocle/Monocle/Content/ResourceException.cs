using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content
{
    class ResourceException : Exception
    {
        private string p;

        public ResourceException(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
    }
}
