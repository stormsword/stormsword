
let sprites = ['grass_corners', 'grass_foliage']

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
export class LevelEditor extends Phaser.State {
  preload() {
    // Load tilesets
    sprites.forEach((sprite, index) => {
      this.load.image(sprite, `/world/sprites/${sprite}.png`); // Load sprite as an image so we can display the entire thing
    })

    this.load.tilemap('outside', null, outdoors_map, Phaser.Tilemap.TILED_JSON)
  }

  create() {
    // this.add.sprite(0, 0, 'grass_corners')
    // Configure World
    this.game.world.setBounds(0, 0, 2000, 2000);  // Create a large game surface so we can move the camera
    this.game.stage.backgroundColor = '#2d2d2d';

    // Setup tilemap
    this.map = this.game.add.tilemap('outside', 16, 16) // Create a blank tilemap with 16x16 tiles
    this.map.addTilesetImage('grass_corners')
    this.currentLayer = this.map.createLayer('outdoors')

    // Configure input
    this.game.input.addMoveCallback(this.updateCursor, this);
    this.cursors = this.game.input.keyboard.createCursorKeys();

    // Draw a marker over each square
    this.marker = this.game.add.graphics();
    this.marker.lineStyle(2, 0xfff000, 1);
    this.marker.drawRect(0, 0, 16, 16);
  }

  update() {
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

  pickTile(sprite, pointer) {
    this.currentTile = this.game.math.snapToFloor(pointer.x, 16) / 16;
  }

  updateCursor() {
    // console.log(this.game.input.activePointer.worldX)
    this.marker.x = this.currentLayer.getTileX(this.game.input.activePointer.worldX) * 16;
    this.marker.y = this.currentLayer.getTileY(this.game.input.activePointer.worldY) * 16;

    if (this.game.input.mousePointer.isDown)
    {
      console.log('mousedown')
        // this.map.putTile(this.currentTile, this.layer.getTileX(this.marker.x), this.layer.getTileY(this.marker.y), this.layer);
        // map.fill(currentTile, currentLayer.getTileX(marker.x), currentLayer.getTileY(marker.y), 4, 4, currentLayer);
    }
  }
}
