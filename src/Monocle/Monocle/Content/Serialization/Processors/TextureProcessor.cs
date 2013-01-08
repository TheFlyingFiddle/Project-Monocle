using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Monocle.Content.Serialization
{
    [Processor(typeof(Texture2DContent))]
    class TextureProcessor : Processor<Bitmap, Texture2DContent>
    {
        public PixelFormat PixelFormat
        {
            get;
            set;
        }

        public bool PreMultiply
        {
            get;
            set;
        }

        public override Texture2DContent Process(Bitmap input)
        {
            //Do some stuff.
            return new Texture2DContent();
        }
    }

    public enum PixelFormat
    {
        RGBA,
        RGB_5_6_5,
        RGBA_5_5_5_1,
        DTX1,
        DTX5
    }
}
