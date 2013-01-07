using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Math
{
    struct Vector2f
    {
        private static Vector2f _zero = new Vector2f(0, 0);
        private static Vector2f _one = new Vector2f(1, 1);
        private static Vector2f _up = new Vector2f(0, 1);
        private static Vector2f _right = new Vector2f(1, 0);

        public static Vector2f Zero
        {
            get { return _zero; }
        }

        public static Vector2f One
        {
            get { return _one; }
        }

        public static Vector2f Up
        {
            get { return _one; }
        }

        public static Vector2f Right
        {
            get { return _right; }
        }


        public float X;
        public float Y;

        public Vector2f(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float this [int index] 
        {
            get 
            {
                if (index == 0)
                    return X;
                else if (index == 1)
                    return Y;

                throw new IndexOutOfRangeException();
            }
            set 
            {
                if (index == 0)
                    this.X = value;
                else if (index == 1)
                    this.Y = value;

                throw new IndexOutOfRangeException();
            }
        }
        
        public float Magnitude
        {
            get { return MathHelper.Sqrt(X * X + Y * Y); }
        }

        public float MagnitudeSquared
        {
            get { return X * X + Y * Y; }
        }

        public Vector2f Normalized
        {
            get
            {
                return (this / this.Magnitude);
            }
        }

        public void Normalize()
        {
            float mag = this.Magnitude;
            this.X /= mag;
            this.Y /= mag;
        }

        public static float Angle(Vector2f from, Vector2f to)
        {
            return (from - to).Magnitude;
        }

        public static float Distance(Vector2f first, Vector2f second)
        {
            return (first - second).Magnitude;
        }

        public static Vector2f operator /(Vector2f vec, float scale)
        {
            return new Vector2f(vec.X / scale, vec.Y / scale);
        }

        public static Vector2f operator *(Vector2f vec, float scale)
        {
            return new Vector2f(vec.X * scale, vec.Y * scale);
        }

        public static Vector2f operator *(float scale, Vector2f vec)
        {
            return new Vector2f(vec.X * scale, vec.Y * scale);
        }

        public static float operator *(Vector2f first, Vector2f second)
        {
            return first.X * second.X + first.Y * second.Y;
        }

        public static Vector2f operator +(Vector2f first, Vector2f second)
        {
            return new Vector2f(first.X + second.X, first.Y + second.Y);
        }

        public static Vector2f operator -(Vector2f first, Vector2f second)
        {
            return new Vector2f(first.X - second.X, first.Y - second.Y);
        }  

        public static bool operator ==(Vector2f first, Vector2f second) 
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public static bool operator !=(Vector2f first, Vector2f second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2f)
            {
                Vector2f other = (Vector2f)obj;
                return this.X == other.X && this.Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
    }
}
