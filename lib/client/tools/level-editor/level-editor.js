
let sprites = ['grass']


export class LevelEditor extends Phaser.State {
  preload() {
    // Load tilesets
    sprites.forEach((sprite, index) => {
      this.load.image(sprite, `/world/sprites/${sprite}.png`); // Load sprite as an image so we can display the entire thing
      this.load.spritesheet(`${sprite}-sheet`, `/world/sprites/${sprite}.png`, 16, 16)  // Load sprite as a spritesheet so we can pick individual tiles
    })
  }

  create() {
    // Configure World
    let width = 2000
    let height = 2000
    this.game.world.setBounds(0, 0, width, height);  // Create a large game surface so we can move the camera
    this.game.stage.backgroundColor = '#2d2d2d';

    // Setup tilemap
    this.setupTiles(width, height)

    // Create our tile selector
    this.createTileSelector();

    // Draw a marker over each square
    this.createMarker()

    // Configure input
    this.configureInput()
  }

  update() {
    // Controls to Move the map around
    if (this.cursors.left.isDown)
    {
        this.game.camera.x -= 4;
    }
    else if (this.cursors.right.isDown)
    {
        this.game.camera.x += 4;
    }

    if (this.cursors.up.isDown)
    {
        this.game.camera.y -= 4;
    }
    else if (this.cursors.down.isDown)
    {
        this.game.camera.y += 4;
    }
  }

  toggleZoom() {
    if(this.game.camera.scale.x == 2) {
      this.game.camera.scale.x = 1
      this.game.camera.scale.y = 1
    } else {
      this.game.camera.scale.x = 2
      this.game.camera.scale.y = 2
    }
  }

  setupTiles(width, height) {
    // Setup the main tilemap and layers
    this.map = this.add.tilemap()
    this.map.setTileSize(16, 16)
    this.map.addTilesetImage('grass')
    this.currentTile = 0; // Default to the first tile in the list
    this.currentLayer = this.map.create('level1', width / 16, height / 16, 16, 16);  // Layer should be as big as the world
  }

  pickTile(sprite, pointer) {
    this.currentTile = this.game.math.snapToFloor(pointer.x, 16) / 16;

    if(this.cursorTile) {
      // There's already a cursor sprite, delete it!
      this.cursorTile.destroy()
    }
    this.cursorTile = this.game.add.sprite(pointer.worldX * 16, pointer.worldY * 16, `grass-sheet`, this.currentTile)
    this.cursorTile.alpha = 0.6 // Make sprite transparent so it doesn't totally cover other tiles
  }

  createMarker() {
    this.marker = this.game.add.graphics();
    this.marker.lineStyle(2, 0xfff000, 1);
    this.marker.drawRect(0, 0, 16, 16);
  }

  updateCursor() {
    let x = this.currentLayer.getTileX(this.game.input.activePointer.worldX) * 16;
    let y = this.currentLayer.getTileY(this.game.input.activePointer.worldY) * 16;

    this.cursorTile.x = x
    this.cursorTile.y = y

    this.marker.x = x
    this.marker.y = y

    if (this.game.input.mousePointer.isDown)
    {
        this.map.putTile(this.currentTile, this.currentLayer.getTileX(this.marker.x), this.currentLayer.getTileY(this.marker.y), this.layer);
        // map.fill(currentTile, currentLayer.getTileX(marker.x), currentLayer.getTileY(marker.y), 4, 4, currentLayer);
    }
  }

  createTileSelector() {
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

  configureInput() {
    this.game.input.addMoveCallback(this.updateCursor, this);
    this.cursors = this.game.input.keyboard.createCursorKeys();
    this.zoom = this.game.input.keyboard.addKey(Phaser.KeyCode.Z)
    this.zoom.onDown.add(this.toggleZoom, this)
  }

  render() {
    // this.game.debug.cameraInfo(this.game.camera, 500, 32);
  }
}




/* Example tilemap:

this.load.tilemap('outside', null, outdoors_map, Phaser.Tilemap.TILED_JSON)

let outdoors_map = {
 "height": 5,
 "width": 5,
 "layers":[
        {
         "data":[ 0, 0, 0, 0, 0,
                  1, 1, 1, 1, 1,
                  2, 2, 2, 2, 2,
                  3, 3, 3, 3, 3,
                  4, 4, 4, 4, 4],
         "height":5,
         "name":"outdoors",
         "opacity":1,
         "type":"tilelayer",
         "visible":true,
         "width":5,
         "x":0,
         "y":0
        }],
 "orientation":"orthogonal",
 "properties":
    {

    },
 "tileheight": 16,
 "tilewidth": 16,
 "tilesets":[
        {
         "firstgid":0,
         "image":"../../client/game/world/sprites/grass_corners.png",
         "imageheight":96,
         "imagewidth":256,
         "margin":0,
         "name":"grass_corners",
         "properties":
            {

            },
         "spacing":0,
         "tileheight":16,
         "tilewidth":16
        }],
 "version":1
}

// this.map = this.game.add.tilemap('outside', 16, 16) // Load existing tilemap
// this.map.addTilesetImage('grass_corners')
// this.currentLayer = this.map.createLayer('outdoors')

*/
