/* World - Initialize and update the game world */
const EventEmitter = require('events')

const Gameloop = require('node-gameloop')

const Player = require('./player.js')

module.exports = class World extends EventEmitter{
  constructor() {
    super()
    this.clients = []   // Currently connected clients

    // State of game world
    this.state = {
      players: []
    }

    // Start the game loop
    const loop = Gameloop.setGameLoop((delta) => {
      this.update() })
  }

  addClient(id) {
    // Adds a newly connected client to the world
    if(id) {
      // Add client to list of active players
      this.clients.push(id)

      // Spawn new player
      let player = new Player(id)
      this.spawn(player)
    }
  }

  removeClient(id) {
    // Removes a disconnected client from the world
    // Find/remove client
    if(id && this.clients.length > 0) {
      const index = this.clients.indexOf(id)

      if (index !== -1) {
        this.clients.splice(index, 1)
      }
    }

    // Find/remove player
    if(id && this.state.players.length > 0) {
      this.despawn('Player', id)
    }
  }

  spawn(entity) {
    /* Spawns an object into the world */

    let type = entity.constructor.name
    switch(type) {
      case 'Player':
        console.log(`player ${entity.id} connected`)
        this.state.players.push(entity)
        this.emit('spawn', entity)
    }
  }

  despawn(type, id) {
    switch(type) {
      case 'Player':
      if(id) {
        // Iterate through players array and
        for(let i=0; i < this.state.players.length; i++) {
          if(id == this.state.players[i].id) {
            console.log(`player ${id} disconnected`)
            this.state.players.splice(i, 1)
            i = this.state.players.length
          }
        }
      }
    }
  }

  update(delta) {
    /* The core game loop that ticks every 30ms */
    // console.log(this.state)
    // HandleInput
    // Process Game
    // Send results to clients
  }
}
