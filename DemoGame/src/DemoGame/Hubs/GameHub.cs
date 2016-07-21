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
        private const int WallSize = 32;
        private const int Speed = 32; // px per move

        // don't do this
        private static readonly ConcurrentDictionary<string, Player> Players = new ConcurrentDictionary<string, Player>();
        
        public void Join(string name)
        {
            var id = Context.ConnectionId;

            if (Players.ContainsKey(id)) return;

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Player" + Players.Count + 1;
            }

            var p = new Player()
            {
                Id = id,
                Name = name
            };

            if (Players.TryAdd(id, p))
            {
                // todo determine spawn location
                var spawn = GetSpawnPoint();

                p.X = spawn.X;
                p.Y = spawn.Y;

                Clients.Caller.joined(name, spawn.X, spawn.Y, Players.Values);
                Clients.Others.synch(Players.Values);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;

            if (Players.ContainsKey(id))
            {
                Player player;
                if (Players.TryRemove(id, out player))
                {
                    Clients.All.leave(id);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        #region Commands

        public void MoveUp()
        {
            Player p;
            if (Players.TryGetValue(Context.ConnectionId, out p))
            {
                p.Y = Math.Max(WallSize, p.Y - Speed);
                
                Synchronize();
            }
        }

        public void MoveDown()
        {
            Player p;
            if (Players.TryGetValue(Context.ConnectionId, out p))
            {
                p.Y = Math.Min(MapSize - WallSize, p.Y + Speed);
                
                Synchronize();
            }
        }

        public void MoveLeft()
        {
            Player p;
            if (Players.TryGetValue(Context.ConnectionId, out p))
            {
                p.X = Math.Max(WallSize, p.X - Speed);

                Synchronize();
            }
        }

        public void MoveRight()
        {
            Player p;
            if (Players.TryGetValue(Context.ConnectionId, out p))
            {
                p.X = Math.Min(MapSize - WallSize, p.X + Speed);

                Synchronize();
            }
        }
        
        #endregion

        private void Synchronize()
        {
            Clients.All.synch(Players.Values);
        }

        private Point GetSpawnPoint()
        {
            // todo randomize
            return new Point(MapSize / 2, MapSize / 2);
        }
    }
}
