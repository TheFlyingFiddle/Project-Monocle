using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization.Importers;
using Monocle.Graphics;
using OpenTK;
using System.IO;

namespace Monocle.Content.Serialization.Processors
{
    [Processor(typeof(FontFile), true)]
    class FontProcessor : Processor<FontFile, Font>
    {
        public override Font Process(FontFile input, IResourceContext context)
        {
            if (input.Pages.Count > 1)
                throw new ArgumentException("The font system can only handle fonts that are in a single page!" +
                          string.Format(" The file {0} contains {1} pages!", this.ResourcePath, input.Pages.Count));

            string face = input.Info.Face + "-" + input.Info.Size + "-" + (input.Info.Bold == 1 ? "b" : "") + (input.Info.Italic == 1 ? "i" : "");
            var texture = LoadTexture(input.Pages[0].File, context);

            int largest = input.Chars.Max((x) => x.ID);

            var charMap = new CharInfo[largest + 1];
            for (int i = 0; i < input.Chars.Count; i++)
            {
                FontChar c = input.Chars[i];

                
                var info =
                    new CharInfo(texture, new Rect(c.X - 1f, c.Y - 1f, c.Width + 1f, c.Height + 1f),
                                 new Vector2(c.XOffset, c.YOffset),
                                 c.XAdvance);

                charMap[c.ID] = info;
                

            }


            return new Font(face, input.Info.Size, input.Common.LineHeight, texture, charMap);
        }

        private Texture2D LoadTexture(string fileName, IResourceContext context)
        {
            string path = Path.Combine(Path.GetDirectoryName(this.ResourcePath), fileName);
            return context.LoadAsset<Texture2D>(path, processor: new TextureProcessor(System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                                                  OpenTK.Graphics.OpenGL.PixelInternalFormat.CompressedRedRgtc1));
        }
    }
}