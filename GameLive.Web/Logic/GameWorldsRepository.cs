using GameLife.Core;
using GameLive.Web.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLive.Web.Logic
{
    public class GameWorldRepository
    {
        public GameWorldRepository(IOptions<ConfigSettings> configSettings)
        {
            _width = configSettings.Value.WorldSize.Width;
            _height = configSettings.Value.WorldSize.Height;
            _worldUpdateIntervalMs = configSettings.Value.WorldUpdateIntervalMs;
            _cacheTimeoutSec = configSettings.Value.WorldRepositoryTimeoutSec;
            _worlds = new ConcurrentDictionary<String, KeyValuePair<AutoIteratedGameWorld, DateTime>>();
        }

        private readonly int _width;
        private readonly int _height;
        private readonly int _worldUpdateIntervalMs;
        private readonly int _cacheTimeoutSec;

        private readonly ConcurrentDictionary<String, KeyValuePair<AutoIteratedGameWorld, DateTime>> _worlds;

        /// <summary>
        /// Получить мир
        /// </summary>
        /// <param name="id">Ключ мира</param>
        /// <param name="worldConnection">Мир</param>
        /// <returns></returns>
        public Boolean Get(String id, out AutoIteratedGameWorld worldConnection)
        {
            KeyValuePair<AutoIteratedGameWorld, DateTime> worldConnectionResult;

            if (_worlds.TryGetValue(id, out worldConnectionResult))
            { worldConnection = worldConnectionResult.Key; }
            else
            { worldConnection = null; }

            return worldConnection != null;
        }

        /// <summary>
        /// Удалить мир
        /// </summary>
        /// <param name="id">Ключ мира</param>
        /// <returns></returns>
        public Boolean Remove(String id)
        {
            Boolean result;

            if (_worlds.ContainsKey(id))
            {
                KeyValuePair<AutoIteratedGameWorld, DateTime> world;

                _worlds.Remove(id, out world);

                world.Key.OnUpdate -= UpdateAction;

                result = true;
            }
            else
            { result = false; }

            return result;
        }

        /// <summary>
        /// Добавить новый мир
        /// </summary>
        /// <param name="id">Ключ мира</param>
        /// <param name="live">Список живых клеток</param>
        /// <returns></returns>
        public AutoIteratedGameWorld Add(String id, IEnumerable<GameLife.Core.Point> live)
        {
            Actualize();

            var world = new World(new HashSet<GameLife.Core.Point>(live), _width, _height);

            var gameWorld = new GameLifeProvider(world);
            var worldConnection = new AutoIteratedGameWorld(gameWorld, _worldUpdateIntervalMs, id);

            if (!Add(worldConnection))
            { throw new InvalidOperationException(); }

            return worldConnection;
        }

        /// <summary>
        /// Добавить новый мир
        /// </summary>
        /// <param name="world">Мир</param>
        /// <returns></returns>
        public Boolean Add(AutoIteratedGameWorld world)
        {
            Boolean result;

            if (_worlds.ContainsKey(world.ID))
            { result = false; }
            else
            {
                _worlds.AddOrUpdate(world.ID, new KeyValuePair<AutoIteratedGameWorld, DateTime>(world, DateTime.Now), (o1, o2) => o2);
                world.OnUpdate += UpdateAction;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Актуализировать кеш
        /// </summary>
        public void Actualize()
        {
            lock (_worlds)
            {
                var removeIds = new List<String>();

                foreach (var keyValuePaur in _worlds)
                {
                    if ((DateTime.Now - keyValuePaur.Value.Value).TotalSeconds > _cacheTimeoutSec)
                    { removeIds.Add(keyValuePaur.Key); }
                }

                foreach (var removeId in removeIds)
                { Remove(removeId); }
            }
        }

        /// <summary>
        /// Обновить кеш по ключу мира
        /// </summary>
        /// <param name="id">Ключ мира</param>
        public void Update(String id)
        {
            lock (_worlds)
            {
                AutoIteratedGameWorld world;

                Get(id, out world);
                Remove(id);
                Add(world);
            }
        }

        private void UpdateAction(GameLifeProvider worldIterator, String id)
        { Update(id); }
    }
}