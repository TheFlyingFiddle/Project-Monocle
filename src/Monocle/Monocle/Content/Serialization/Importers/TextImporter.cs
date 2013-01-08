using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Monocle.Content.Serialization
{
    [Importer(true,".txt")]
    class TextImporter : Importer<string>
    {

        public override string Import(Stream data)
        {
            var reader = new StreamReader(data);
            return reader.ReadToEnd();
        }
    }
}
