using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace GameLife.Core
{
    public class GameLifeProvider
    {
        public GameLifeProvider(World startWorld)
        {
            this.Width = startWorld.Width;
            this.Height = startWorld.Height;

            _liveIterations = new List<HashSet<Point>>();

            _liveIterations.Add(startWorld.Live);
        }

        private List<HashSet<Point>> _liveIterations;

        /// <summary>
        /// Кол-во ячеек в ширину
        /// </summary>
        public Int32 Width
        { get; private set; }

        /// <summary>
        /// Кол-во ячеек в высоту
        /// </summary>
        public Int32 Height
        { get;private set; }

        /// <summary>
        /// Текущее количество итераций
        /// </summary>
        public Int32 IterationCount
        { get => _liveIterations.Count; }

        /// <summary>
        /// Можно ли продолжать итерации
        /// </summary>
        public Boolean CanIterate
        { get; private set; }

        /// <summary>
        /// Получить текущую итерацию
        /// </summary>
        /// <returns></returns>
        public World GetWorld()
        {
            if (!_liveIterations.Any())
            { throw new Exception("There no iteration"); }

            return new World(_liveIterations.Last(), Height, Width);
        }

        /// <summary>
        /// Очистить
        /// </summary>
        public void Clear()
        { _liveIterations.Clear(); }

        /// <summary>
        /// Итерировать мир
        /// </summary>
        public void IterateWorld()
        {
            var iterator = new GameLifeIterator(GetWorld());
            iterator.Iterate();
            var newIteration = iterator.NewWorld.Live;

            var liveComparer = new LiveComparer();

            CanIterate = !liveComparer.Equals(new HashSet<Point>(), newIteration);

            if (CanIterate)
            {
                foreach (var liveIteration in _liveIterations)
                {
                    if (liveComparer.Equals(liveIteration, newIteration))
                    {
                        CanIterate = false;
                        break;
                    }
                }

                if (CanIterate)
                { _liveIterations.Add(newIteration); }
            }
        }
    }
}
