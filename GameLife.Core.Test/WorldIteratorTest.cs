using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLife.Core.Test
{
    public class GameLiveIteratorTest
    {
        private static string GetStringWorld(World world)
        {
            var sb = new StringBuilder();

            for (int i=0; i < world.Width;i++)
            {
                for (int j = 0; j < world.Height; j++)
                {
                    var point = new Point(i, j);
                    var isAlive = world.IsAlive(point);

                    sb.Append((isAlive ? '1' : '0')); sb.Append(' ');
                }

                sb.AppendLine();
            }

            var result = sb.ToString();

            return result;
        }

        [Test]
        public void WorldIterateTest()
        {
            var world = new World(5, 5);

            //1 0 0 0 0
            //0 1 0 0 1
            //0 0 1 0 0
            //1 0 0 1 1 
            //0 0 0 1 0 

            var points = new[]
            {
                new Point(0, 0), new Point(1, 1), new Point(1, 4),
                new Point(2, 2), new Point(2, 3),
                new Point(3, 0), new Point(3, 3), new Point(3, 4),
                new Point(4, 3)
            };

            foreach (var point in points)
            { world.Live.Add(point); }

            //var result1 = GetStringWorld(world);

            var worldIterator = new GameLifeIterator(world);
            worldIterator.Iterate();

            //1 0 0 0 1
            //1 1 1 1 1
            //0 1 1 0 0
            //0 0 0 0 0
            //1 0 0 1 0

            var newWorld = worldIterator.NewWorld;

            //var result2 = GetStringWorld(newWorld);

            var resultPoints = new[]
            {
                new Point(0, 0), new Point(0, 4),
                new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4),
                new Point(2, 1), new Point(2, 2),
                new Point(4, 0), new Point(4, 3)
            };

            foreach (var point in resultPoints)
            { Assert.IsTrue(newWorld.IsAlive(point)); }

            
        }
    }
}