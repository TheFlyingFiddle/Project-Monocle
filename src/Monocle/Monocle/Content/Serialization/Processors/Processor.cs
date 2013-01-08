using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    public interface IProcessor
    {
        object Process(object input);
    }

    public abstract class Processor<I, O> : IProcessor
    {
        public abstract O Process(I input);

        public object Process(object input)
        {
            return Process((I)input);
        }
    }
}