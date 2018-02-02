/* Start the game */

import {Debug} from './debug/debug.js'
import {Character} from './char/character.js'

export class Play extends Phaser.State {

  constructor() {
    super()
    this.players = []
    this.debug = new Debug()

  }

  preload() {
    this.load.spritesheet('ranger', './char/sprites/ranger.png', 64, 64)
    this.load.spritesheet('wizard', './char/sprites/wizard.png', 64, 64)
    this.load.spritesheet('warrior', './char/sprites/warrior.png', 64, 64)
  }

  create() {
    window.socket.emit('loaded')
    window.socket.on('init', (state) => {
      for(let i = 0; i < state.players.length; i++) {
        this.spawn(state.players[i])
      }
    })

    // Prevent right clicks from popping up a context menu
    this.game.canvas.oncontextmenu = function (e) { e.preventDefault(); }
    this.game.stage.disableVisibilityChange = true


    window.socket.on('spawn', (entity) => {
      // console.log(this)
      console.log('spawning: ' + entity.id)
      this.spawn(entity)
    })

    window.socket.on('update', (data) => {
      // Process data from server

      // Iterate through data and players and process any updates
      for(let i = 0; i < this.players.length; i++) {
        for(let j = 0; j < data.players.length; j++) {
          if(this.players[i].id == data.players[j].id) {
            this.players[i].x = data.players[j].x
            this.players[i].y = data.players[j].y
            this.players[i].body.velocity.x = data.players[j].body.velocity.x
            this.players[i].body.velocity.y = data.players[j].body.velocity.y
            this.players[i].body.facing = data.players[j].body.facing
          }
        }
      }

      // Output debug data
      this.debug.update(data.players, this.players)
    })


    window.socket.on('despawn', (entity) => {
      console.log(`despawning: ${entity.id}`)

      let index
      for(let i=0; i < this.players.length; i++) {
        if(this.players[i].id == entity.id) {
          index = i
        }
      }
      if (index !== -1) {
        this.players[index].kill()
        this.players.splice(index, 1)

      }
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
  }

  spawn(entity) {
    let player = new Character(entity.id, this.game, 350, 300, entity.character)
    // Add to game engine and enable physics
    console.log('adding: ' + player.name)

    this.game.add.existing(player)
    this.game.physics.arcade.enable(player)

    // Add to array of players
    this.players.push(player)
  }
}
