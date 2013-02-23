using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Graphics
{
    class TextureAtlas : IEnumerable<KeyValuePair<string,Frame>>
    {
        public readonly Texture2D Texture;
        private readonly Dictionary<string, Frame> dict;

        public TextureAtlas(Texture2D texture, Dictionary<string, Frame> dict)
        {
            this.Texture = texture;
            this.dict = dict;
        }

        public Frame this[string name]
        {
            get
            {
                return this.dict[name];
            }
        }

        public IEnumerator<KeyValuePair<string, Frame>> GetEnumerator()
        {
            return this.dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Frame[] ToArray()
        {
            return this.dict.Values.ToArray();
        }
    }
}
