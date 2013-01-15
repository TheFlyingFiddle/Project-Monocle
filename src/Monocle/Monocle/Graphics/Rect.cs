using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Monocle.Content.Serialization;

namespace Monocle.Graphics
{
    public struct Rect
    {
        public float X, Y, Width, Height;

        public Rect(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(this.X + this.Width / 2.0f, this.Y + this.Height / 2);
            }
            set
            {
                this.X = value.X - this.Width / 2.0f;
                this.Y = value.Y - this.Height / 2.0f;
            }
        }

        public float Left { get { return this.X; } }
        public float Right { get { return this.X + this.Width; } }
        public float Bottom { get { return this.Y; } }
        public float Top { get { return this.Y + this.Width; } }


        /// <summary>
        /// Converts the rectanlge to a Vector4. 
        /// The xy components of the Vector4 responds to the bottom left corner and
        /// the zw components corresponds to the top left corner.
        /// </summary>
        /// <returns>A Vector4 representation of the rectangle.</returns>
        public Vector4 ToVector4()
        {
            return new Vector4(X, Y, X + Width, Y + Height);
        }


        public static Rect MaxRect(Rect first, Rect second)
        {
            return new Rect(Math.Min(first.X, second.X),
                            Math.Min(first.Y, second.Y),
                            Math.Max(first.X + first.Width, second.X + second.Width),
                            Math.Max(first.Y + first.Height, second.Y + second.Height));
        }

        public override string ToString()
        {
            return string.Format("(left:{0:F2}, top:{1:F2}, width:{2:F2}, height:{3:F2})", new object[]
			{
				this.X,
				this.Y,
				this.Width,
				this.Height
			});
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !(left == right);
        }

        public static bool operator ==(Rect left, Rect right)
        {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Width == right.Width &&
                   left.Height == right.Height;
        }


        [TypeWriter]
        public class RectWriter : TypeWriter<Rect>
        {
            public override void WriteType(Rect toWrite, IWriter writer)
            {
                writer.Write(toWrite.X);
                writer.Write(toWrite.Y);
                writer.Write(toWrite.Width);
                writer.Write(toWrite.Height);
            }
        }

        [TypeReader]
        public class RectReader : TypeReader<Rect>
        {
            public override Rect Read(IReader reader)
            {
                return new Rect(reader.ReadFloat(),
                                reader.ReadFloat(),
                                reader.ReadFloat(),
                                reader.ReadFloat());
                                
            }
        }

        private static readonly Rect _zero = new Rect(0, 0, 0, 0);

        public static Rect Zero 
        {
            get { return _zero; }
        }

        internal bool ContainsPoint(Vector2 vector2)
        {
            return this.Left < vector2.X && this.Right > vector2.X &&
                   this.Bottom < vector2.Y && this.Top > vector2.Y;
        }
    }
}