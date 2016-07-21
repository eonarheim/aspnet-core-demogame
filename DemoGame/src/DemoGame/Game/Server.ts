interface SignalR {
    game: SignalR.Hub.Proxy;
}

var hub = $.connection.game;

function connectToServer() {
    return $.connection.hub.start(() => {
        ex.Logger.getInstance().info("Connected to server", hub.connection.id);

        // request to join game
        hub.server.join(name);
    });
}

function onJoined(x: number, y: number) {
    ex.Logger.getInstance().info("Joined", x, y);

    if (player) {
        player.connectionId = hub.connection.id;
        return;
    };

    player = new MainPlayer(hub.connection.id, name, x, y);
    players[player.connectionId] = player;

    game.add(player);
    game.currentScene.camera.setActorToFollow(player);
}

function onOtherJoined(id: string, name: string, x: number, y: number) {

    ex.Logger.getInstance().info("Other player joined", id, name, x, y);

    if (players[id]) return;    

    var op = new Player(id, name, x, y);

    players[id] = op;

    game.add(op);
}

// methods server can invoke on client
hub.client.joined = onJoined;
hub.client.otherJoined = onOtherJoined;