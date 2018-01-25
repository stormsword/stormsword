/* World - Initialize and update the game world */
const EventEmitter = require('events')

const Gameloop = require('node-gameloop')

const Player = require('./player.js')

module.exports = class World extends EventEmitter{
  constructor() {
    super()

    // State of game world
    this.state = {
      players: []
    }

    // Start the game loop
    const loop = Gameloop.setGameLoop((delta) => {
      this.update() })
  }

  spawn(type, metadata) {
    /* Spawns an object into the world */
    let entity

    switch(type) {
      case 'Player':
        if(metadata.id) {
          console.log(`player ${metadata.id} connected`)
          entity = new Player(metadata.id)
          this.state.players.push(entity)
        } else {
          throw new error('Player cannot spawn without an id')
        }
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
        // Iterate through players array and
        for(let i=0; i < this.state.players.length; i++) {
          if(metadata.id == this.state.players[i].id) {
            console.log(`player ${metadata.id} disconnected`)
            
            entity = this.state.players[i]
            this.state.players.splice(i, 1)
            i = this.state.players.length // Terminate the for loop
          }
        }
      }
    }

    if(entity) {
      // Tell clients to despawn the new player
      this.emit('despawn', entity)
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
