using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization.ImportedContent;
using Monocle.Graphics;
using System.IO;

namespace Monocle.Content.Serialization.Processors
{
    [Processor(typeof(AtlasContent), true)]
    class TextureAtlasProcessor : Processor<AtlasContent, TextureAtlas>
    {
        private const string Texture_File_Ending = ".png";

        public override TextureAtlas Process(AtlasContent input, IResourceContext context)
        {
            var texture = LoadTexture(context);
            var dict = new Dictionary<string, Frame>();
            foreach (var element in input.Elements)
            {
                dict.Add(element.name, new Frame(element.srcRect, texture));
            }

            return new TextureAtlas(texture, dict);
        }

        private Texture2D LoadTexture(IResourceContext context)
        {
            string relativePath = Path.ChangeExtension(this.ResourcePath, Texture_File_Ending);
            return context.LoadAsset<Texture2D>(relativePath);
        }
    }
}
