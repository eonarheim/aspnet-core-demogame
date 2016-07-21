var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Config = {
    background: ex.Color.Azure,
    mapSize: 1600
};
var Resources = {
    TxMap: new ex.Texture("images/map.png"),
    TxPlayer: new ex.Texture("images/dude.png")
};
var hub = $.connection.game;
function connectToServer() {
    return $.connection.hub.start(function () {
        ex.Logger.getInstance().info("Connected to server", hub.connection.id);
        // request to join game
        hub.server.join(name);
    });
}
function onJoined(x, y) {
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
}
function onOtherJoined(id, name, x, y) {
    ex.Logger.getInstance().info("Other player joined", id, name, x, y);
    if (players[id])
        return;
    var op = new Player(id, name, x, y);
    players[id] = op;
    game.add(op);
}
// methods server can invoke on client
hub.client.joined = onJoined;
hub.client.otherJoined = onOtherJoined;
/// <reference path="Config.ts" />
/// <reference path="Resources.ts" />
/// <reference path="Server.ts"/>
var player;
var name = "Kamranicus";
var players = {};
var game = new ex.Engine({
    canvasElementId: 'game',
    displayMode: ex.DisplayMode.FullScreen
});
game.backgroundColor = Config.background;
var loader = new ex.Loader();
for (var r in Resources) {
    loader.addResource(Resources[r]);
}
game.start(loader).then(function () {
    connectToServer();
    game.currentScene.camera = new ex.LockedCamera();
    var map = new ex.Actor(0, 0, game.getWidth(), game.getHeight());
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
    return MainPlayer;
}(Player));
//# sourceMappingURL=game.js.map