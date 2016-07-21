using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGame.Models;

namespace DemoGame.Hubs
{
    [HubName("game")]
    public class GameHub : Hub
    {
        private const int MapSize = 1600;

        // don't do this
        private static readonly ConcurrentDictionary<string, Player> Players = new ConcurrentDictionary<string, Player>();
        
        public void Join(string name)
        {
            var id = Context.ConnectionId;

            if (Players.ContainsKey(id)) return;

            var p = new Player()
            {
                Id = id,
                Name = name
            };

            // todo determine spawn location
            var spawn = GetSpawnPoint();

            Clients.Caller.joined(spawn.X, spawn.Y);
            Clients.All.otherJoined(id, name, spawn.X, spawn.Y);
        }
        
        private Point GetSpawnPoint()
        {
            // todo randomize
            return new Point(MapSize / 2, MapSize / 2);
        }
    }
}
