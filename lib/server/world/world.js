/* World - Initialize and update the game world */
const EventEmitter = require('events')

const Gameloop = require('node-gameloop')

const Map = require('./map.js')

const Player = require('./player.js')
const Entity = require('./entity/entity.js')

// Load world data
const promisify = require('util').promisify
const jsonfile = require('jsonfile')
const readFile = promisify(jsonfile.readFile)  // Callback-based module needs to be wrapped in a promse to use `async```

module.exports = class World extends EventEmitter {
  constructor() {
    super()

    // State of game world
    this.state = {
      players: [],
      entities: []
    }

    // Setup world map
    this.map = new Map()

    this.loadData()

    // Start the game loop
    const loop = Gameloop.setGameLoop((delta) => {
      this.update()
    })
  }

  async loadData() {
    // Spawn objects into the world
    this.GAME_DIR = 'lib/server/world/'
    this.gameData = await readFile(this.GAME_DIR + './world.json')
    this.gameData.forEach(async (entity) => {
      this.spawn('Entity', entity)
    })
  }

  update(delta) {
    /* The core game loop that ticks every 30ms */

    // Process Game
    for(let i=0; i < this.state.players.length; i++) {
      this.state.players[i].update()
    }

    for(let i=0; i < this.state.entities.length; i++) {
      this.state.entities[i].update()
    }

    // Send results to clients
    this.emit('update', this.state)
  }

  spawn(type, metadata) {
    /* Spawns an object into the world */
    let entity

    switch(type) {
      case 'Player':
        if(metadata.id) {
          console.log(`player ${metadata.id} connected`)
          if(metadata.character) {
            entity = new Player(metadata)
            this.state.players.push(entity)
          } else {
            console.log('no character found')
          }
        } else {
          throw new Error('Player cannot spawn without an id')
        }
      break;
      case 'Entity':
        console.log(metadata)
        entity = new Entity(metadata)
        this.state.entities.push(entity)
        break;
    }

    if(entity) {
      // Tell clients to spawn the new player
      this.emit('spawn', entity)
    }
  }

  despawn(type, metadata) {
    let entity

    switch(type) {
      case 'Player':
        if(metadata.id) {
          // Iterate through players array and remove player
          for(let i=0; i < this.state.players.length; i++) {
            if(metadata.id == this.state.players[i].id) {
              console.log(`player ${metadata.id} disconnected`)

              entity = this.state.players[i]
              this.state.players.splice(i, 1)
              i = this.state.players.length // Terminate the for loop
            }
          }
        } else {
          throw new Error ('Player cannot despawn without an id')
        }
        break;
    }

    if(entity) {
      // Tell clients to despawn the new player
      this.emit('despawn', entity)
    }
  }

  input(id, velocity) {
    let index = this.findPlayer(id)

    if(index !== undefined) {
      this.state.players[index].body.velocity = velocity
    }
  }

  chatStart(id) {
    let index = this.findPlayer(id)

    if(index !== undefined) {
      this.state.players[index].chatting = true
    }
  }

  chatSend(id, message) {
    let index = this.findPlayer(id)

    if(index !== undefined) {
      if(message && message != "") {
        console.log('Message: ' + JSON.stringify(message))
        this.emit('message', this.state.players[index], message)
      }
      this.state.players[index].chatting = false
    }
  }

  chatCancel(id) {
    let index = this.findPlayer(id)

    if(index !== undefined) {
      this.state.players[index].chatting = false
    }
  }

  findPlayer(id) {
    for(let i = 0; i < this.state.players.length; i++) {
      if(id == this.state.players[i].id) {
        return(i)
      }
    }
  }

}
