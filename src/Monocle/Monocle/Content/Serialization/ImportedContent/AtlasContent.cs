using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Graphics;

namespace Monocle.Content.Serialization.ImportedContent
{
    class AtlasContent
    {
        public class AtlasElement
        {
            public readonly Rect srcRect;
            public readonly string name;

            public AtlasElement(Rect srcRect, string name)
            {
                this.srcRect = srcRect;
                this.name = name;
            }
        }

        public List<AtlasElement> Elements = new List<AtlasElement>();
    }
}
