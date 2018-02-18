/* Start the game */

import {Input} from './input.js'
import {Chat} from './chat/chat.js'

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
    this.load.spritesheet('chattyping', './character/sprites/chattyping.png', 30, 18)
    this.load.spritesheet('chatbubble', './character/sprites/chatbubble.png')

    // Load characters
    this.load.spritesheet('ranger', './character/sprites/ranger.png', 64, 64)
    this.load.spritesheet('wizard', './character/sprites/wizard.png', 64, 64)
    this.load.spritesheet('warrior', './character/sprites/warrior.png', 64, 64)

    this.groups = {
      world: new Phaser.Group(this.game, null, 'world'),
      ui: new Phaser.Group(this.game, null, 'ui')
    }
    this.groups.ui.fixedToCamera = true
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

    /* Configure Input */
    this.input = new Input(this.game)

    /* Configure Chat */
    this.chat = new Chat(this.game, this.input, this.socket, this.groups.ui)

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
