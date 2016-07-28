interface SignalR {
    game: SignalR.Hub.Proxy;
}

interface IServerPlayer {
    Id: string;
    Name: string;
    X: number;
    Y: number;
}

var hub = $.connection.game;

// promise resolves when connected
var connected = $.Deferred();

function connectToServer() {
    return $.connection.hub.start({ transport: ['webSockets'] }, () => {
        ex.Logger.getInstance().info("Connected to server", hub.connection.id);    

        connected.resolve();
    });
}

function joinGame(name: string) {

    // when connected
    connected.then(() => {
        // request to join game
        hub.server.join(name);
    });
}

function onSynch(sp: IServerPlayer[]) {
    var synced: string[] = [];

    // ensure players are synchronized

    for (var p of sp) {
        synced.push(p.Id);

        if (!players[p.Id]) {
            // spawn player
            onOtherJoined(p.Id, p.Name, p.X, p.Y);
        }

        players[p.Id].easeTo(p.X, p.Y, 150, ex.EasingFunctions.EaseInOutCubic);
    }

    // remove disconnected players

    var disconnected: string[] = [];

    for (var pid in players) {
        if (players.hasOwnProperty(pid)) {
            if (synced.indexOf(pid) === -1) {
                disconnected.push(pid);
            }
        }
    }

    for (var pid of disconnected) {
        onLeave(pid);
    }
}

function onJoined(name: string, x: number, y: number, others: IServerPlayer[]) {
    ex.Logger.getInstance().info("Joined", x, y);

    if (player) {
        player.connectionId = hub.connection.id;
        return;
    };

    player = new MainPlayer(hub.connection.id, name, x, y);
    players[player.connectionId] = player;

    game.add(player);
    game.currentScene.camera.setActorToFollow(player);

    onSynch(others);
}

function onOtherJoined(id: string, name: string, x: number, y: number) {

    ex.Logger.getInstance().info("Other player joined", id, name, x, y);

    if (players[id]) return;    

    var op = new Player(id, name, x, y);

    players[id] = op;

    game.add(op);
}

function onLeave(id: string) {
    ex.Logger.getInstance().info("Player disconnected", id);

    if (!players[id]) return;

    var p = players[id];

    game.remove(p);

    delete players[id];
}

// methods server can invoke on client
hub.client.joined = onJoined;
hub.client.otherJoined = onOtherJoined;
hub.client.leave = onLeave;
hub.client.synch = onSynch;