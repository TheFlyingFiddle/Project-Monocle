using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Monocle.Content.Serialization.Importers
{
    [Importer(true, ".fnt")]
    class FontImporter : Importer<FontFile>
    {
        public override FontFile Import(System.IO.Stream data)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
            FontFile file = null;
            using (TextReader textReader = new StreamReader(data))
            {
                file = (FontFile)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            return file;
        }
    }
}
