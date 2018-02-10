/* Start the game */

import {Input} from './input.js'
import {Chat} from './chat.js'

import {Character} from './char/character.js'
import {Map} from './world/map.js'

import {Debug} from './debug/debug.js'

export class Play extends Phaser.State {

  constructor() {
    super()
    this.players = []

    this.socket = window.socket

    this.map = new Map(this)

    this.debug = new Debug()
  }

  preload() {
    this.groups = {
      players: this.game.add.group(),
      map: this.game.add.group()
    }

    this.game.world.bringToTop(this.groups.players)

    // Load map
    this.load.image('grass', './world/sprites/grass.png')

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
          this.spawn(state.players[i])
        }
      }
    })

    this.socket.on('map', (tiles, csv) => {
      this.map.load(tiles, csv, this.groups.map)
    })

    this.socket.on('spawn', (entity) => {
      // console.log(this)
      console.log('spawning: ' + entity.id)
      this.spawn(entity)
    })

    this.socket.on('update', (data) => {
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
            this.players[i].chatting = data.players[i].chatting
            if(this.players[i].id == this.id) {
              // Handle states that affect this client's UI
              if(this.players[i].chatting) {
                this.chatting = true
              } else {
                this.chatting = false
              }
            }
          }
        }
      }

      // Output debug data
      this.debug.update(data.players, this.players)
    })


    this.socket.on('despawn', (entity) => {
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

    /* Configure Input */
    this.input = new Input(this.game)

    // Chat Input
    this.input.enter.add(() => {
      if(!this.chatting) {
        this.socket.emit('chat:start')
        this.message = ""

        this.input.chatMode((char) => {
          // Called when a key is pressed
          this.message += char  // Append to chat
        })
      } else {
        this.socket.emit('chat:send', this.message)
        this.message = ""
        this.input.moveMode()
      }
    }, this)

    this.input.escape.add(() => {
      if(this.chatting) {
        this.socket.emit('chat:cancel')
        this.input.moveMode()
      }
    }, this)

    this.input.backspace.add(() => {
      if(this.chatting) {
        if(this.message.length > 0) {
          this.message = this.message.slice(0, -1)  // Remove the last character from the message
        }
      }
    })

    this.input.spacebar.add(() => {
      if(this.chatting) {
        this.message += " "
      }
    })

    /* Chat Networking */
    window.socket.on('message', (player, message) => {
      console.log(player)
      console.log(message)
    })
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

    // if(this.chatting) {
      // console.log(this.message)
    // }
  }

  spawn(entity) {
    let player = new Character(entity.id, this.game, 350, 300, entity.character)

    if(entity.id == this.socket.id) {
      // It's-a me!
      this.id = entity.id
    }

    // Add to game engine and enable physics
    console.log('adding: ' + player.name)

    this.game.add.existing(player)
    this.game.physics.arcade.enable(player)

    // Add to array of players
    this.players.push(player)
    this.groups.players.add(player)
  }
}
