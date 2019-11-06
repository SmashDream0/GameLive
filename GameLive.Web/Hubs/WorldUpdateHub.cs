using GameLife.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameLive.Web.Hubs
{
    [Authorize]
    public class WorldUpdateHub : Hub
    {
        public WorldUpdateHub(Logic.ConnectionMapping connectionMapping)
        {
            _connectionMapping = connectionMapping;
        }

        private readonly Logic.ConnectionMapping _connectionMapping;

        public override Task OnConnectedAsync()
        {
            _connectionMapping.Add(Context.User.Identity.Name, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectionMapping.Remove(Context.User.Identity.Name, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}