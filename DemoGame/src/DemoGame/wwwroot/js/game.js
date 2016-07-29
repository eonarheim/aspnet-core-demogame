var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Config = {
    background: ex.Color.fromHex("#49a269"),
    mapSize: 1600
};
var Resources = {
    TxMap: new ex.Texture("images/map.png"),
    TxPlayer: new ex.Texture("images/dude.png")
};
var hub = $.connection.game;
var err = function (exc) {
    ex.Logger.getInstance().error("Error calling server method", exc);
};
// promise resolves when connected
var connected = $.Deferred();
function connectToServer() {
    return $.connection.hub.start({ transport: ['webSockets'] }, function () {
        ex.Logger.getInstance().info("Connected to server", hub.connection.id);
        connected.resolve();
    });
}
function joinGame(name) {
    // when connected
    connected.then(function () {
        // request to join game
        hub.server.join(name).fail(err);
    });
}
function onSynch(sp) {
    var synced = [];
    // ensure players are synchronized
    for (var _i = 0, sp_1 = sp; _i < sp_1.length; _i++) {
        var p = sp_1[_i];
        synced.push(p.Id);
        if (!players[p.Id]) {
            // spawn player
            onOtherJoined(p.Id, p.Name, p.X, p.Y);
        }
        players[p.Id].easeTo(p.X, p.Y, 150, ex.EasingFunctions.EaseInOutCubic);
    }
    // remove disconnected players
    var disconnected = [];
    for (var pid in players) {
        if (players.hasOwnProperty(pid)) {
            if (synced.indexOf(pid) === -1) {
                disconnected.push(pid);
            }
        }
    }
    for (var _a = 0, disconnected_1 = disconnected; _a < disconnected_1.length; _a++) {
        var pid = disconnected_1[_a];
        onLeave(pid);
    }
}
function onJoined(name, x, y, others) {
    ex.Logger.getInstance().info("Joined", x, y);
    if (player) {
        player.connectionId = hub.connection.id;
        return;
    }
    ;
    player = new MainPlayer(hub.connection.id, name, x, y);
    players[player.connectionId] = player;
    game.add(player);
    game.currentScene.camera.setActorToFollow(player);
    onSynch(others);
}
function onOtherJoined(id, name, x, y) {
    ex.Logger.getInstance().info("Other player joined", id, name, x, y);
    if (players[id])
        return;
    var op = new Player(id, name, x, y);
    players[id] = op;
    game.add(op);
}
function onLeave(id) {
    ex.Logger.getInstance().info("Player disconnected", id);
    if (!players[id])
        return;
    var p = players[id];
    game.remove(p);
    delete players[id];
}
// methods server can invoke on client
hub.client.joined = onJoined;
hub.client.otherJoined = onOtherJoined;
hub.client.leave = onLeave;
hub.client.synch = onSynch;
/// <reference path="Config.ts" />
/// <reference path="Resources.ts" />
/// <reference path="Server.ts"/>
var player;
var players = {};
var game = new ex.Engine({
    canvasElementId: 'game',
    displayMode: ex.DisplayMode.FullScreen,
    pointerScope: ex.Input.PointerScope.Canvas
});
game.backgroundColor = Config.background;
var loader = new ex.Loader();
for (var r in Resources) {
    loader.addResource(Resources[r]);
}
// center camera on map
game.currentScene.camera = new ex.LockedCamera();
game.currentScene.camera.x = Config.mapSize;
game.currentScene.camera.y = Config.mapSize;
game.start(loader).then(function () {
    // join game
    connectToServer();
    // add map
    var map = new ex.Actor(0, 0, Config.mapSize, Config.mapSize);
    map.anchor.setTo(0, 0);
    map.addDrawing(Resources.TxMap);
    map.setCenterDrawing(false);
    game.add(map);
});
var Player = (function (_super) {
    __extends(Player, _super);
    function Player(connectionId, name, x, y) {
        _super.call(this, x, y, 32, 32);
        this.connectionId = connectionId;
        this.name = name;
    }
    Player.prototype.onInitialize = function (engine) {
        _super.prototype.onInitialize.call(this, engine);
        this.scale.setTo(1.5, 1.5);
        this.addDrawing(Resources.TxPlayer);
        var nameLabel = new ex.Label(this.name, 0, -20, "Arial");
        nameLabel.textAlign = ex.TextAlign.Center;
        this.add(nameLabel);
    };
    return Player;
}(ex.Actor));
var MainPlayer = (function (_super) {
    __extends(MainPlayer, _super);
    function MainPlayer() {
        _super.apply(this, arguments);
    }
    MainPlayer.prototype.update = function (engine, delta) {
        _super.prototype.update.call(this, engine, delta);
        if (engine.input.keyboard.wasReleased(ex.Input.Keys.Up)) {
            hub.server.moveUp();
        }
        if (engine.input.keyboard.wasReleased(ex.Input.Keys.Down)) {
            hub.server.moveDown();
        }
        if (engine.input.keyboard.wasReleased(ex.Input.Keys.Left)) {
            hub.server.moveLeft();
        }
        if (engine.input.keyboard.wasReleased(ex.Input.Keys.Right)) {
            hub.server.moveRight();
        }
    };
    return MainPlayer;
}(Player));
//# sourceMappingURL=game.js.map