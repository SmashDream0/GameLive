using System;
using System.Collections.Generic;
using System.Text;

namespace GameLife.Core
{
    public class LiveComparer : IEqualityComparer<HashSet<Point>>
    {
        public Boolean Equals(HashSet<Point> x, HashSet<Point> y)
        {
            Boolean result = x.Count == y.Count;

            if (result)
            {
                foreach (var point in x)
                {
                    result = y.Contains(point) && result;

                    if (!result)
                    { break; }
                }
            }

            return result;
        }

        public Int32 GetHashCode(HashSet<Point> obj)
        {
            var hash = 31;

            foreach (var point in obj)
            { hash ^= point.GetHashCode(); }

            return hash;
        }
    }
}