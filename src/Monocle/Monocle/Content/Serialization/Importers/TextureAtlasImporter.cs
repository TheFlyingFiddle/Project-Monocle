using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization.ImportedContent;
using System.IO;
using Monocle.Graphics;

namespace Monocle.Content.Serialization.Importers
{
    [Importer(true, ".atlas")]
    class TextureAtlasImporter : Importer<AtlasContent>
    {
        public override AtlasContent Import(System.IO.Stream data)
        {
            var atlasContent = new AtlasContent();

            using (TextReader treader = new StreamReader(data))
            {
                string line = treader.ReadLine();
                while ((line = treader.ReadLine()) != null)
                {

                    string[] first = line.Split('=');
                    string[] second = first[1].Split(' ');

                    string name = first[0].Trim();
                    float x = int.Parse(second[1]);
                    float y = int.Parse(second[2]);
                    float w = int.Parse(second[3]);
                    float h = int.Parse(second[4]);

                    var elem = new AtlasContent.AtlasElement(new Rect(x, y, w, h), name);
                    atlasContent.Elements.Add(elem);
                }
            }

            return atlasContent;
        }
    }
}
