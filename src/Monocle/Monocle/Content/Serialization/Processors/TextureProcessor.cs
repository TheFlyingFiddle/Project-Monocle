using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Monocle.Graphics;


namespace Monocle.Content.Serialization
{
    [Processor(typeof(Texture2DContent))]
    class TextureProcessor : Processor<Bitmap, Texture2D>
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

        public override Texture2D Process(Bitmap input)
        {


            return null;
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
