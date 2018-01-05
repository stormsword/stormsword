/* level-editor.js - Level/Tilemap Editor*/

let sprites = ['grass']

import { TileSelector } from './components/tileselector.js'

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
    this.tileSelector = new TileSelector(this.game)

    // Draw a marker over each square
    this.createMarker()

    // Configure input
    this.configureInput()
  }

  update() {
    this.moveMap()
  }

  setupTiles(width, height) {
    // Setup the main tilemap and layers
    this.map = this.add.tilemap()
    this.map.setTileSize(16, 16)
    this.map.addTilesetImage('grass')

    this.currentLayer = this.map.create('level1', width / 16, height / 16, 16, 16);  // Layer should be as big as the world
  }

  createMarker() {
    this.marker = this.game.add.graphics();
    this.marker.lineStyle(2, 0xfff000, 1);
    this.marker.drawRect(0, 0, 16, 16);
  }

  updateCursor() {
    let x = this.currentLayer.getTileX(this.game.input.activePointer.worldX) * (16 / this.game.camera.scale.x)  // Adjust for zoom
    let y = this.currentLayer.getTileY(this.game.input.activePointer.worldY) * (16 / this.game.camera.scale.y)

    this.tileSelector.cursorTile.x = x
    this.tileSelector.cursorTile.y = y

    this.marker.x = x
    this.marker.y = y

    if (this.game.input.mousePointer.isDown)
    {
        this.map.putTile(this.tileSelector.currentTile, this.currentLayer.getTileX(this.marker.x), this.currentLayer.getTileY(this.marker.y), this.layer);
        // map.fill(currentTile, currentLayer.getTileX(marker.x), currentLayer.getTileY(marker.y), 4, 4, currentLayer);
    }
  }

  configureInput() {
    this.cursors = this.game.input.keyboard.createCursorKeys(); // Configure arrow keys (used in update)

    this.game.input.addMoveCallback(this.updateCursor, this);   // Fire 'updateCursor' whenever the mouse moves

    this.keys = {}  // Object to hold our keys
    // r - Rotate the sprite left
    this.keys.Q = this.game.input.keyboard.addKey(Phaser.KeyCode.Q)
    this.keys.Q.onDown.add(() => {
       this.tileSelector.rotateTile('left')
     }, this)

    // t - Rotate the sprite right
    this.keys.W = this.game.input.keyboard.addKey(Phaser.KeyCode.W)
    this.keys.W.onDown.add(() => {
       this.tileSelector.rotateTile('right')
     }, this)

    // z - Zoom the camera

    this.zoom = this.game.input.keyboard.addKey(Phaser.KeyCode.Z)
    this.zoom.onDown.add(this.toggleZoom, this)
  }

  moveMap() {
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

  render() {
    // this.game.debug.cameraInfo(this.game.camera, 500, 32);
    this.game.debug.spriteInfo(this.tileSelector.cursorTile)
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
