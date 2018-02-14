/* Start the game */

import {Input} from './input.js'
import {Chat} from './chat.js'

import {GameWorld} from './world/gameworld.js'

import {Debug} from './debug/debug.js'

export class Play extends Phaser.State {

  constructor() {
    super()

    this.socket = window.socket

    this.debug = new Debug()
  }

  preload() {
    this.gameworld = new GameWorld(this.game)


    // Load map
    this.load.image('grass', './world/sprites/grass.png')

    // Load chat UI
    this.load.spritesheet('chattyping', './char/sprites/chattyping.png', 30, 18)
    this.load.spritesheet('chatbubble', './char/sprites/chatbubble.png')

    // Load characters
    this.load.spritesheet('ranger', './char/sprites/ranger.png', 64, 64)
    this.load.spritesheet('wizard', './char/sprites/wizard.png', 64, 64)
    this.load.spritesheet('warrior', './char/sprites/warrior.png', 64, 64)
  }

  create() {
    this.socket.emit('loaded')
    this.socket.on('init', (state) => {
      for(let i = 0; i < state.players.length; i++) {
        if(state.players[i].id != this.socket.id) {
          this.gameworld.spawn(state.players[i])
        }
      }
    })

    this.socket.on('map', (tiles, csv) => {
      this.gameworld.loadMap(tiles, csv)
    })

    this.socket.on('spawn', (entity) => {
      // console.log(this)
      console.log('spawning: ' + entity.id)

      if(entity.id == this.socket.id) {
        // It's-a me!
        this.id = entity.id
      }

      this.gameworld.spawn(entity)
    })

    this.socket.on('update', (data) => {
      // Process data from server

      // Iterate through data and players and process any updates
      for(let i = 0; i < this.gameworld.players.length; i++) {
        for(let j = 0; j < data.players.length; j++) {
          if(this.gameworld.players[i].id == data.players[j].id) {
            this.gameworld.updatePlayer(i, data.players[j])

            if(this.gameworld.players[i].id == this.id) {
              // Handle states that affect this client's UI
              if(this.gameworld.players[i].chatting) {
                this.chat.chatting = true
              } else {
                this.chat.chatting = false
              }
            }
          }
        }
      }

      // Output debug data
      this.debug.update(data.players, this.gameworld.players)
    })


    this.socket.on('despawn', (entity) => {
      console.log(`despawning: ${entity.id}`)

      this.gameworld.despawn(entity.id)
    })

    /* Chat Networking */
    this.socket.on('message', (entity, message) => {
      this.gameworld.showMessage(entity.id, message)
    })


    // Initialize World
    this.game.world.setBounds(0, 0, 2000, 2000);  // Create a large game surface so we can move the camera
    this.game.physics.startSystem(Phaser.Physics.ARCADE);
    this.game.stage.backgroundColor = '#2d2d2d';

    this.game.camera.follow(this.player, Phaser.Camera.FOLLOW_LOCKON, 0.1, 0.1);  // Attach camera to player

    /* Configure Input */
    this.input = new Input(this.game)

    // Chat Input
    this.chat = new Chat(this.input, this.socket)
  }

  update() {
    // Handle Movement
    let velocity = {
      x: 0,
      y: 0
    }

    if(this.input.moveUp()) {
      velocity.y = -1 // HACK - Reversed y values due to funky origin setting
    } else if(this.input.moveDown()) {
      velocity.y = 1
    }

    if(this.input.moveRight()) {
      velocity.x = 1
    } else if(this.input.moveLeft()) {
      velocity.x = -1
    }

    // Send movement data to server
    if(velocity != {x: 0, y: 0}) {  // Only send movement data if keys were pressed
      this.socket.emit('input', velocity)
    }
  }


}
