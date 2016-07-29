using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGame.Models;
using Microsoft.Extensions.Options;
using DemoGame.Services;

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
        private readonly AppOptions _options;

        public GameHub(IOptions<AppOptions> options)
        {
            _options = options.Value;
        }

        public void Join(string name)
        {
            var id = Context.ConnectionId;

            if (Players.ContainsKey(id)) return;

            // autogen names if custom names turned off or player didn't supply one
            if (string.IsNullOrWhiteSpace(name) || !_options.EnableCustomNames)
            {
                name = "Player" + (Players.Count + 1);
            } else
            {
                // max length 25 chars
                name = name.Substring(0, Math.Min(25, name.Length));
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
            var r = new Random(DateTime.Now.Millisecond);
            var rx = r.Next(0, MapSize);
            var ry = r.Next(0, MapSize);

            return new Point(rx, ry);
        }
    }
}
