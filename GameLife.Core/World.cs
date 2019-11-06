using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLife.Core
{
    public struct World
    {
        public World(Int32 height, Int32 width) : this(new HashSet<Point>(), height, width)
        { }
        public World(HashSet<Point> live, Int32 height, Int32 width)
        {
            Live = live;
            this.Height = height;
            this.Width = width;
        }

        /// <summary>
        /// Жизнь
        /// </summary>
        public HashSet<Point> Live
        { get; private set; }

        /// <summary>
        /// Высота мира
        /// </summary>
        public Int32 Height
        { get; private set; }

        /// <summary>
        /// Ширина мира
        /// </summary>
        public Int32 Width
        { get; private set; }

        /// <summary>
        /// Убрать жизнь
        /// </summary>
        public void Clear()
        { Live.Clear(); }

        /// <summary>
        /// Есть-ли жизнь в ячейке
        /// </summary>
        /// <param name="point">Ячейка</param>
        /// <returns></returns>
        public Boolean IsAlive(Point point)
        {
            point = GetPointWithCorrection(point);

            return Live.Contains(point);
        }

        /// <summary>
        /// Занести жизнь в ячейку
        /// </summary>
        /// <param name="point">Ячейка</param>
        /// <returns></returns>
        public Boolean SetAlive(Point point)
        {
            Boolean result = false;

            point = GetPointWithCorrection(point);

            if (!Live.Contains(point))
            {
                Live.Add(point);
                result = true;
            }

            return result;
        }

        private Point GetPointWithCorrection(Point point)
        {
            Int32 newX = point.X, newY = point.Y;

            newX %= Width;
            newY %= Height;

            if (newX < 0)
            { newX = newX + Width; }
            if (newY < 0)
            { newY = newY + Height; }

            point = new Point(newX, newY);

            return point;
        }

        public Boolean Equals(World other)
        {
            var liveComparer = new LiveComparer();

            return liveComparer.Equals(this.Live, other.Live);
        }
    }
}