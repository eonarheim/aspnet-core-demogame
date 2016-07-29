/// <reference path="Config.ts" />
/// <reference path="Resources.ts" />
/// <reference path="Server.ts"/>

var player: MainPlayer;
var players: { [key: string]: Player } = {};

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

game.start(loader).then(() => {

    // join game
    connectToServer();

    // add map
    var map = new ex.Actor(0, 0, Config.mapSize, Config.mapSize);
    map.anchor.setTo(0, 0);
    map.addDrawing(Resources.TxMap);
    map.setCenterDrawing(false);

    game.add(map);
});