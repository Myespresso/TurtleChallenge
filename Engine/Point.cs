using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    [Serializable]
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            return $"X:{X} Y:{Y}";
        }
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && b.Y == a.Y;
        }
        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || b.Y != a.Y;
        }
        public override bool Equals(object obj)
        {
            var result = false;
            if (obj != null)
            {
                try
                {
                    var point = (Point)obj;
                    result = this.X == point.X && this.Y == point.Y;

                }
                catch { result = false; }
            }
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int hashBase = (int)2166136261;
                const int hashMul = 16777619;
                int hash = hashBase;
                hash = (hash * hashMul) ^ X;
                hash = (hash * hashMul) ^ Y;
                return hash;
            }

        }
    }
}
