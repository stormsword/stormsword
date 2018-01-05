/* fileselector.js - Menu to select tile for placing */

// currentTile = currently selected tile

export class TileSelector {
  constructor(_game) {
    this.game = _game

    //  Our tile picker menu
    let tileSelector = this.game.add.group();

    let tileSelectorBackground = this.game.make.graphics();
    tileSelectorBackground.beginFill(0x000000, 0.5);
    tileSelectorBackground.drawRect(0, 0, 800, 18);
    tileSelectorBackground.endFill();

    tileSelector.add(tileSelectorBackground);

    var tileStrip = tileSelector.create(0, 0, 'grass');
    tileStrip.inputEnabled = true;

    // Tilestrip mouse events
    tileStrip.events.onInputDown.add(this.pickTile, this);
    tileStrip.events.onInputOver.add(() => {
      this.cursorTile.alpha = 0
    })
    tileStrip.events.onInputOut.add(() => {
      this.cursorTile.alpha = 0.6
    })

    tileSelector.fixedToCamera = true;

    this.pickTile(null, {x: 0}) // Select the first tile by default
  }

  pickTile(sprite, pointer) {
    this.currentTile = this.game.math.snapToFloor(pointer.x, 16) / 16;

    /* Setup tile to be stuck to cursor */
    if(this.cursorTile) {
      // There's already a cursor sprite, delete it!
      this.cursorTile.destroy()
    }

    this.cursorTile = this.game.add.sprite(pointer.worldX * 16, pointer.worldY * 16, `grass-sheet`, this.currentTile)
    this.tileCount = 0  // Placeholder for rotating tile
    this.cursorTile.alpha = 0.8 // Make sprite transparent so it doesn't totally cover other tiles

  }

  rotateTile(direction) {
    /* Because tiles move when they are rotated, we need to constantly reset the anchor */
    switch(this.tileCount) {
      case 0:
        // First: 90 right
        this.cursorTile.anchor.setTo(0, 1)
        this.cursorTile.angle += 90
        this.tileCount++
        break;
      case 1:
        // First: 90 right
        this.cursorTile.anchor.setTo(1, 1)
        this.cursorTile.angle += 90
        this.tileCount++
        break;
      case 2:
        // First: 90 right
        this.cursorTile.anchor.setTo(1, 0)
        this.cursorTile.angle += 90
        this.tileCount++
        break;
      case 3:
        // First: 90 right
        this.cursorTile.anchor.setTo(0, 0)
        this.cursorTile.angle += 90
        this.tileCount = 0
        break;
    }
  }

}
