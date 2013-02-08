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
        /// <summary>
        /// The x coordinate of the rectangle. 
        /// </summary>
        public float X;

        /// <summary>
        /// The y coordinate of the rectangle.
        /// </summary>
        public float Y;

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public float W;

        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public float H;
        
        public Rect(float x, float y, float w, float h)
        {
            this.X = x;
            this.Y = y;
            this.W = w;
            this.H = h;
        }

        /// <summary>
        /// Gets or sets the center of the rectangle.
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return new Vector2(this.X + this.W / 2.0f, this.Y + this.H / 2);
            }
            set
            {
                this.X = value.X - this.W / 2.0f;
                this.Y = value.Y - this.H / 2.0f;
            }
        }

        public Vector2 TopLeft
        {
            get { return new Vector2(this.X, this.Y); }
        }
        
        public Vector2 TopRight
        {
            get { return new Vector2(this.X + this.W, this.Y); }
        }

        public Vector2 BottomLeft
        {
            get { return new Vector2(this.X, this.Y + this.H); }
        }

        public Vector2 BottomRight
        {
            get { return new Vector2(this.X + this.W, this.Y + this.H); }
        }

        public void Displace(Vector2 displacement)
        {
            this.X += displacement.X;
            this.Y += displacement.Y;
        }


        /// <summary>
        /// Gets the left side of the rectangle.
        /// </summary>
        public float Left { get { return this.X; } }

        /// <summary>
        /// Gets the right side of the rectangle.
        /// </summary>
        public float Right { get { return this.X + this.W; } }

        /// <summary>
        /// Gets the bottom side of the rectangle.
        /// </summary>
        public float Bottom { get { return this.Y + this.H; } }


        /// <summary>
        /// Gets the top side of the rectangle.
        /// </summary>
        public float Top { get { return this.Y; } }




        /// <summary>
        /// Converts the rectanlge to a Vector4. 
        /// The xy components of the Vector4 responds to the top left corner and
        /// the zw components corresponds to the bottom right corner.
        /// </summary>
        /// <returns>A Vector4 representation of the rectangle.</returns>
        public Vector4 ToVector4()
        {
            return new Vector4(X, Y, X + W, Y + H);
        }

        /// <summary>
        /// Creates a rectangle that contains the other two rectangles.
        /// </summary>
        /// <param name="first">The first rectangle.</param>
        /// <param name="second">The second rectangle.</param>
        /// <returns>A rectangle that contains the input rectangles.</returns>
        public static Rect MaxRect(Rect first, Rect second)
        {
            return new Rect(Math.Min(first.X, second.X),
                            Math.Min(first.Y, second.Y),
                            Math.Max(first.X + first.W, second.X + second.W),
                            Math.Max(first.Y + first.H, second.Y + second.H));
        }

        /// <summary>
        /// Creates a rectangle that contains the other two rectangles.
        /// </summary>
        /// <param name="first">The first rectangle.</param>
        /// <param name="second">The second rectangle.</param>
        /// <returns>A rectangle that contains the input rectangles.</returns>
        public static void MaxRect(ref Rect first, ref Rect second, out Rect result)
        {
            result.X = Math.Min(first.X, second.X);
            result.Y = Math.Min(first.Y, second.Y);
            result.W = Math.Max(first.X + first.W, second.X + second.W);
            result.H = Math.Max(first.Y + first.H, second.Y + second.H);
        }


        /// <summary>
        /// Writes the rectangle nicely formated.
        /// </summary>
        /// <returns>A formated rectangle representation.</returns>
        public override string ToString()
        {
            return string.Format("(left:{0:F2}, top:{1:F2}, width:{2:F2}, height:{3:F2})", new object[]
			{
				this.X,
				this.Y,
				this.W,
				this.H
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
                   left.W == right.W &&
                   left.H == right.H;
        }


        [TypeWriter]
        public class RectWriter : TypeWriter<Rect>
        {
            public override void WriteType(Rect toWrite, IWriter writer)
            {
                writer.Write(toWrite.X);
                writer.Write(toWrite.Y);
                writer.Write(toWrite.W);
                writer.Write(toWrite.H);
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

        public bool ContainsPoint(Vector2 vector2)
        {
            return this.Left < vector2.X && this.Right > vector2.X &&
                   this.Bottom > vector2.Y && this.Top < vector2.Y;
        }

        internal static Rect MinRect(Rect rect, Rect scissorRect)
        {
            throw new NotImplementedException();
        }
    }
}