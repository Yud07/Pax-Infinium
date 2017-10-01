using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace pax_infinium
{
    public class Polygon
    {
        public Polygon() {
            Lines = new List<PolyLine>();
        }

        public List<PolyLine> Lines { get; set; }
        public bool Contains(Vector2 point)
        {

            bool result = false;

            foreach (var side in Lines)
            {
                if (point.Y > Math.Min(side.Start.Y, side.End.Y))
                    if (point.Y <= Math.Max(side.Start.Y, side.End.Y))
                        if (point.X <= Math.Max(side.Start.X, side.End.X))
                        {
                            if (side.Start.Y != side.End.Y)
                            {
                                float xIntersection = (point.Y - side.Start.Y) * (side.End.X - side.Start.X) / (side.End.Y - side.Start.Y) + side.Start.X;
                                if (side.Start.X == side.End.X || point.X <= xIntersection)
                                    result = !result;
                            }
                        }
            }
            return result;
        }
    }

    public class PolyLine
    {
        public Vector2 Start;
        public Vector2 End;

        public PolyLine(Vector2 Start, Vector2 End)
        {
            this.Start = Start;
            this.End = End;
        }
    }
}
