using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameLive.Web.Models;
using GameLife.Core;
using GameLive.Web.Logic;
using Newtonsoft.Json;
using GameLive.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using GameLive.Web.Config;

namespace GameLive.Web.Controllers
{
    public class GameController : Controller
    {
        public GameController(IHubContext<WorldUpdateHub> worldHub, Logic.GameWorldRepository gameWorlds, Logic.ConnectionMapping connectionMapping)
        {
            _worldHub = worldHub;
            _gameWorlds = gameWorlds;
            _connectionMapping = connectionMapping;
        }

        private readonly Logic.ConnectionMapping _connectionMapping;
        private readonly IHubContext<WorldUpdateHub> _worldHub;
        private readonly Logic.GameWorldRepository _gameWorlds;

        public IActionResult Index()
        {
            return View("GameView");
        }

        /// <summary>
        /// Создать игру
        /// </summary>
        /// <param name="liveJson"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateGame(String liveJson)
        {
            var live = JsonConvert.DeserializeObject<GameLife.Core.Point[]>(liveJson);

            AutoIteratedGameWorld worldIterator;
            if (_gameWorlds.Get(User.Identity.Name, out worldIterator))
            {
                worldIterator.Stop();
                _gameWorlds.Remove(User.Identity.Name);
            }

            worldIterator = _gameWorlds.Add(User.Identity.Name, live);
            worldIterator.OnUpdate += UpdateClientWorld;
            worldIterator.Start();

            return Json(new Models.GameStateShortViewModel() { IsStoped = false });
        }

        private void UpdateClientWorld(GameLifeProvider workProvider, String id)
        {
            var result = new GameState
            {
                Live = workProvider.GetWorld().Live.Select(x => new Models.Point() { X = x.X, Y = x.Y }),
                IsStoped = !workProvider.CanIterate,
                IterationCount = workProvider.IterationCount,
                GameStateError = EGameStateError.None,
            };

            var connections = _connectionMapping.GetConnections(id);

            foreach (var connection in connections)
            { _worldHub.Clients.Client(connection).SendAsync("WorldUpdate", result); }
        }

        /// <summary>
        /// Поставить игру на паузу
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult StopGame()
        {
            GameStateShortViewModel result;

            AutoIteratedGameWorld worldIterator;
            if (_gameWorlds.Get(User.Identity.Name, out worldIterator))
            {
                worldIterator.Stop();
                result = new GameStateShortViewModel()
                {
                    GameStateError = EGameStateError.None,
                    IsStoped = true,
                };
            }
            else
            {
                result = new GameStateShortViewModel()
                {
                    GameStateError = EGameStateError.GameNotExist,
                    IsStoped = false,
                };
            }

            return Json(result);
        }

        /// <summary>
        /// Продолжить игру с прежнего состояния
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ContinueGame(String connectionID)
        {
            GameStateShortViewModel result;

            AutoIteratedGameWorld worldIterator;
            if (_gameWorlds.Get(User.Identity.Name, out worldIterator))
            {
                worldIterator.Start();
                result = new GameStateShortViewModel()
                {
                    GameStateError = EGameStateError.None,
                    IsStoped = false,
                };
            }
            else
            {
                result = new GameStateShortViewModel()
                {
                    GameStateError = EGameStateError.GameNotExist,
                    IsStoped = false,
                };
            }

            return Json(result);
        }
    }
}