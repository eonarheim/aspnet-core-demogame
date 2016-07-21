class Player extends ex.Actor {
    constructor(public connectionId: string, public name: string, x: number, y: number) {
        super(x, y, 32, 32);
    }

    public onInitialize(engine: ex.Engine) {
        super.onInitialize(engine);

        this.addDrawing(Resources.TxPlayer);

        var nameLabel = new ex.Label(this.name, 0, -20, "Arial");
        nameLabel.textAlign = ex.TextAlign.Center;
        this.add(nameLabel);
    }
}

class MainPlayer extends Player {

    public update(engine: ex.Engine, delta: number) {
        super.update(engine, delta);

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
    }

}