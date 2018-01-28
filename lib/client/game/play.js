/* Start the game */

import {Ranger} from './char/ranger.js'

export class Play extends Phaser.State {

  constructor() {
    super()
    this.players = []
    this.serverDebug = document.getElementById('serverDebug')
    this.clientDebug = document.getElementById('clientDebug')
  }

  preload() {
    this.load.spritesheet('ranger', './char/sprites/ranger.png', 64, 64)
    this.load.image('grass', './world/sprites/grass_corners.png', 16, 16)
    this.load.image('grass:foliage', './world/sprites/grass_foliage.png', 16, 16)
  }

  create() {
    // Connect to Server
    window.socket.emit('loaded')

    window.socket.on('spawn', (entity) => {
      console.log('spawning: ' + entity.id)
      // Initialize Player
      let player = new Ranger(entity.id, this.game, 350, 300)

      // Add to array of players
      this.players.push(player)

      // Add to game engine and enable physics
      this.game.add.existing(player)
      this.game.physics.arcade.enable(player)
    })

    window.socket.on('update', (data) => {
      // Process data from server
      // this.serverDebug.textContent = data.players.toString()
      // this.clientDebug.textContent = this.players[0].toString()

      // Iterate through data and players and process any updates
      for(let i = 0; i < this.players.length; i++) {
        for(let j = 0; j < data.players.length; j++) {
          if(this.players[i].id == data.players[j].id) {
            this.serverDebug.textContent = `x: ${data.players[j].x} y: ${data.players[j].y}`
            this.clientDebug.textContent = `x: ${this.players[i].x} y: ${this.players[i].y}`
            this.players[i].x = data.players[j].x
            this.players[i].y = data.players[j].y
            // console.log(data.players[j].x)
          }
        }
      }
    })


    window.socket.on('despawn', (entity) => {
      console.log('despawning: ')
      console.log(entity)
    })

    // Initialize World
    this.game.world.setBounds(0, 0, 2000, 2000);  // Create a large game surface so we can move the camera
    this.game.physics.startSystem(Phaser.Physics.ARCADE);
    this.game.stage.backgroundColor = '#2d2d2d';

    this.game.camera.follow(this.player, Phaser.Camera.FOLLOW_LOCKON, 0.1, 0.1);  // Attach camera to player

    // Configure Input
    this.cursors = this.game.input.keyboard.createCursorKeys();
  }

  update() {
    // Handle Input
    let velocity = {
      x: 0,
      y: 0
    }
    if(this.cursors.up.isDown) {
      velocity.y = -1 // HACK - Reversed y values due to funky origin setting
    } else if(this.cursors.down.isDown) {
      velocity.y = 1
    }

    if(this.cursors.right.isDown) {
      velocity.x = 1
    } else if(this.cursors.left.isDown) {
      velocity.x = -1
    }

    // Make sure the player character has been spawned
    if(this.players.length > 0) {
      window.socket.emit('input', velocity)
    }

    // Render
    // console.log(`x: ${this.players[0].x} y: ${this.players[0].y}`)
  }
}
