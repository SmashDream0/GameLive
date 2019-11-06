using GameLife.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace GameLive.Web.Logic
{
    public class AutoIteratedGameWorld
    {
        public AutoIteratedGameWorld(GameLifeProvider gameLifeProvider, int intervalMs, String id)
        {
            _timer = new Timer();

            GameLifeProvider = gameLifeProvider;
            ID = id;

            _timer.Interval = intervalMs;
            _timer.Elapsed += _timer_Elapsed;
        }

        /// <summary>
        /// Событие: после создания новой итерации
        /// </summary>
        public event Action<GameLifeProvider, String> OnUpdate;

        private readonly Timer _timer;

        /// <summary>
        /// Итератор мира
        /// </summary>
        public GameLifeProvider GameLifeProvider
        { get; private set; }

        /// <summary>
        /// Ключ мира
        /// </summary>
        public String ID
        { get; private set; }

        /// <summary>
        /// Запустить работу
        /// </summary>
        public void Start()
        { _timer.Start(); }

        /// <summary>
        /// Остановить работу
        /// </summary>
        public void Stop()
        { _timer.Stop(); }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (GameLifeProvider)
            {
                GameLifeProvider.IterateWorld();

                OnUpdate?.Invoke(GameLifeProvider, ID);

                if (!GameLifeProvider.CanIterate)
                { _timer.Enabled = false; }
            }
        }
    }
}
