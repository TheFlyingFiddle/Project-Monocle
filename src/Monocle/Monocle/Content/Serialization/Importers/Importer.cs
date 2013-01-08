using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Monocle.Content.Serialization
{
    public interface IImporter
    {
        object Import(Stream data);
    }

    public abstract class Importer<T> : IImporter
    {
        public abstract T Import(Stream data);

        object IImporter.Import(Stream data)
        {
            return this.Import(data);
        }
    }
}
