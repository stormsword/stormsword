/* Start the game */

import {Ranger} from './char/ranger.js'

export class Play extends Phaser.State {

  preload() {
    this.load.spritesheet('ranger', './char/sprites/ranger.png', 64, 64)
    this.load.image('grass', './world/sprites/grass_corners.png', 16, 16)
    this.load.image('grass:foliage', './world/sprites/grass_foliage.png', 16, 16)
  }

  create() {
    // Connect to Server
    window.socket.on('world:update', () => {
        console.log('received world update')
    })

    window.socket.on('spawn', (entity) => {
      console.log('spawning: ')
      console.log(entity)
    })

    // Initialize World
    this.game.world.setBounds(0, 0, 2000, 2000);  // Create a large game surface so we can move the camera
    this.game.physics.startSystem(Phaser.Physics.ARCADE);
    this.game.stage.backgroundColor = '#2d2d2d';

    // Initialize Player
    this.player = new Ranger(this.game, 350, 300)
    this.game.add.existing(this.player)
    this.game.physics.arcade.enable(this.player)
    this.game.camera.follow(this.player, Phaser.Camera.FOLLOW_LOCKON, 0.1, 0.1);  // Attach camera to player

    // Configure Input
    this.cursors = this.game.input.keyboard.createCursorKeys();
  }

  update() {
    let x = 0
    let y = 0

    if(this.cursors.up.isDown) {
      y = 1
    } else if(this.cursors.down.isDown) {
      y = -1
    }

    if(this.cursors.right.isDown) {
      x = 1
    } else if(this.cursors.left.isDown) {
      x = -1
    }

    this.player.move(x, y)
  }
}
