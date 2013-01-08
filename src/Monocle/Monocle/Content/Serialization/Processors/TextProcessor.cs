using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    [Processor(typeof(string), true)]
    class TextProcessor : Processor<string, string>
    {
        public override string Process(string input)
        {
            return input;
        }
    }
}
