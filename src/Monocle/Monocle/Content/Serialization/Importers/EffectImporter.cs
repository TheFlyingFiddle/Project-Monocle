using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization.ImportedContent;
using System.IO;

namespace Monocle.Content.Serialization.Importers
{
    [Importer(true, ".effect")]
    public class EffectImporter : Importer<EffectContent> 
    {
        public override EffectContent Import(System.IO.Stream data)
        {
            TextReader reader = new StreamReader(data);

            EffectContent content = new EffectContent();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] words = line.Split('=');
                if (words[0].ToLower() == "fs")
                {
                    content.FragmentShaderPath = words[1];
                }
                else if (words[0].ToLower() == "vs")
                {
                    content.VertexShaderPath = words[1];
                }
            }

            if (string.IsNullOrEmpty(content.FragmentShaderPath) ||
               string.IsNullOrEmpty(content.VertexShaderPath))
                throw new ResourceException("The effect was not valid!");

            return content;
        }
    }
}
