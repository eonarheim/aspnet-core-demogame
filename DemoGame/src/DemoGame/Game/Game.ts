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

game.start(loader).then(() => {

    connectToServer();

    game.currentScene.camera = new ex.LockedCamera();

    var map = new ex.Actor(0, 0, game.getWidth(), game.getHeight());
    map.anchor.setTo(0, 0);
    map.addDrawing(Resources.TxMap);
    map.setCenterDrawing(false);

    game.add(map);
});