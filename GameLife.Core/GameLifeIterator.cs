using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLife.Core
{
    public class GameLifeIterator
    {
        public GameLifeIterator(World world)
        {
            _world = world;
            NewWorld = new World(world.Width, world.Height);
        }

        private readonly World _world;

        /// <summary>
        /// Дивный новый мир
        /// </summary>
        public World NewWorld
        { get; private set; }

        /// <summary>
        /// Произвести ирерацию мира
        /// </summary>
        public void Iterate()
        {
            NewWorld.Clear();

            if (_world.Live.Any())
            {
                for (Int32 x = 0; x < NewWorld.Width; x++)
                {
                    for (Int32 y = 0; y < NewWorld.Height; y++)
                    {
                        var point = new Point(x, y);

                        if (_world.IsAlive(point))
                        {
                            if (!IsDead(point))
                            { NewWorld.SetAlive(point); }
                        }
                        else if (CanMakeLive(point))
                        { NewWorld.SetAlive(point); }
                    }
                }
            }
        }

        private Boolean IsDead(Point point)
        {
            Int32 neighbourCount = GetNeighbourCount(point);

            return neighbourCount != 2 && neighbourCount != 3;
        }

        private Boolean CanMakeLive(Point point)
        {
            Int32 neighbourCount = GetNeighbourCount(point);

            return neighbourCount == 3;
        }

        private Int32 GetNeighbourCount(Point point)
        {
            Int32 result = 0;

            foreach (var pointCorrection in _checkCorrections)
            {
                var newPoint = new Point(point.X + pointCorrection.X, point.Y + pointCorrection.Y);

                if (_world.IsAlive(newPoint))
                { result++; }
            }

            return result;
        }

        private static readonly Point[] _checkCorrections = new[]
        {
            new Point(0,1),
            new Point(1,0),
            new Point(1,1),

            new Point(0,-1),
            new Point(-1,0),
            new Point(-1,-1),

            new Point(-1,1),
            new Point(1,-1),
        };
    }
}