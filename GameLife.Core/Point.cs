using System;
using System.Collections.Generic;
using System.Text;

namespace GameLife.Core
{
    public struct Point
    {
        public Point(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
        }

        public Int32 X
        { get; set; }

        public Int32 Y
        { get; set; }

        public override String ToString()
        {
            return $"X={X};Y={Y}";
        }
    }
}
