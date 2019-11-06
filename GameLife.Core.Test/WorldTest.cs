using GameLife.Core;
using NUnit.Framework;

namespace GameLife.Core.Test
{
    public class WorldTest
    {
        [Test]
        public void GetLifeTest()
        {
            var world = new World(5, 5);

            // 1 0 0 0 0
            // 0 1 0 0 1
            // 0 0 1 0 0
            // 1 0 0 1 1 
            // 0 0 0 1 0 

            var points = new[]
            {
                new Point(0, 0), new Point(1, 1), new Point(1, 4),
                new Point(2, 2), new Point(2, 3),
                new Point(3, 0), new Point(3, 3), new Point(3, 4),
                new Point(4, 3)
            };

            foreach (var point in points)
            { world.Live.Add(point); }

            foreach (var point in points)
            { Assert.IsTrue(world.IsAlive(point)); }            
        }

        [Test]
        public void SetLifeTest()
        {
            var world = new World(5, 5);

            // 1 0 0 0 0
            // 0 1 0 0 1
            // 0 0 1 0 0
            // 1 0 0 1 1 
            // 0 0 0 1 0 

            var points = new[]
            {
                new Point(0, 0), new Point(1, 1), new Point(1, 4),
                new Point(2, 2), new Point(2, 3),
                new Point(3, 0), new Point(3, 3), new Point(3, 4),
                new Point(4, 3)
            };

            foreach (var point in points)
            { Assert.IsTrue(world.SetAlive(point)); }            
        }

        [Test]
        public void SetLifeWithOverflowTest()
        {
            var world = new World(5, 5);

            // 1 0 0 0 0
            // 0 1 0 0 1
            // 0 0 1 0 0
            // 1 0 0 1 1 
            // 0 0 0 1 0 

            var points = new[]
            {
                new Point(0, 0), new Point(1, -5), new Point(1, -1),
                new Point(2, 2), new Point(2, 3),
                new Point(3, 0), new Point(-5, -2), new Point(3, 4),
                new Point(4, -2)
            };

            foreach (var point in points)
            { Assert.IsTrue(world.SetAlive(point)); }
        }

        [Test]
        public void GetLifeWithOverflowTest()
        {
            var world = new World(5, 5);

            // 1 0 0 0 0
            // 0 1 0 0 1
            // 0 0 1 0 0
            // 1 0 0 1 1 
            // 0 0 0 1 0 

            var points = new[]
            {
                new Point(0, 0), new Point(1, 1), new Point(1, 4),
                new Point(2, 2), new Point(2, 3),
                new Point(3, 0), new Point(3, 3), new Point(3, 4),
                new Point(4, 3)
            };

            foreach (var point in points)
            { world.Live.Add(point); }

            foreach (var point in points)
            {
                var newPoint = new Point(point.X + world.Width * 2, point.Y + world.Height);

                Assert.IsTrue(world.IsAlive(point));
            }            
        }
    }
}