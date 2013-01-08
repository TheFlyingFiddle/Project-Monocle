using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Monocle.Content.Serialization
{
    [Importer(true, ".jpg", ".png", ".bmp", ".tiff", ".psd")]
    public class TextureImporter : Importer<Bitmap>
    {
        public override Bitmap Import(Stream data)
        {
            var bitmap = new Bitmap(data);
            

            //Do stuff.
            return bitmap;
        }
    }
}
