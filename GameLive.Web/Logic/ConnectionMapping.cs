using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLive.Web.Logic
{
    public class ConnectionMapping
    {
        private readonly Dictionary<String, HashSet<String>> _connections =
            new Dictionary<String, HashSet<String>>();

        /// <summary>
        /// Количество сопоставлений
        /// </summary>
        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        /// <summary>
        /// Добавить сопоставление
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="connectionId"></param>
        public void Add(String key, String connectionId)
        {
            lock (_connections)
            {
                HashSet<String> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<String>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        /// <summary>
        /// Поулчить список подключений
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<String> GetConnections(String key)
        {
            HashSet<String> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<String>();
        }

        /// <summary>
        /// Удалить подключение
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Remove(String key, String connectionId)
        {
            lock (_connections)
            {
                HashSet<String> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}