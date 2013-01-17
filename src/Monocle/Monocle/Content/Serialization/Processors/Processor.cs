using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Content.Serialization
{
    public interface IProcessor
    {
        string ResourcePath { get; set; }
        object Process(object input, IResourceContext context);
    }

    public abstract class Processor<I, O> : IProcessor
    {
        public string ResourcePath { get; set;  }

        public abstract O Process(I input, IResourceContext context);

        public object Process(object input, IResourceContext context)
        {
            return Process((I)input, context);
        }
    }
}