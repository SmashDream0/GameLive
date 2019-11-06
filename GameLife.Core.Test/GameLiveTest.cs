using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLife.Core.Test
{
    public class GameLiveTest
    {
        [Test]
        public void GameLiveIterateValidTest()
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
            { world.SetAlive(point); }

            var game = new GameLifeProvider(world);

            game.IterateWorld();

            var iteratedWorld = game.GetWorld();

            //var result2 = GetStringWorld(iteratedWorld);

            var resultPoints = new[]
            {
                new Point(0, 0), new Point(0, 4),
                new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4),
                new Point(2, 1), new Point(2, 2),
                new Point(4, 0), new Point(4, 3)
            };

            foreach (var point in resultPoints)
            { Assert.IsTrue(iteratedWorld.IsAlive(point)); }
        }

        [Test]
        public void GameLiveCanIterateValidTest()
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

            var game = new GameLifeProvider(world);

            game.IterateWorld();

            Assert.IsTrue(game.CanIterate);
        }

        [Test]
        public void GameLiveIterateEmptyTest()
        {
            var world = new World(5, 5);

            var game = new GameLifeProvider(world);

            game.IterateWorld();

            Assert.IsFalse(game.CanIterate);
        }
    }
}