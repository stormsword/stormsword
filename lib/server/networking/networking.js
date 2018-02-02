/* Networking.js - Handles all i/o from client and server */

const GameData = require('./gamedata/gamedata.js')

class Networking {
  constructor(world, io) {
    this.clients = []   // Currently connected clients

    this.world = world
    this.io = io

    /* Load Game Data into memory */
    this.gameData = new GameData()

    this.clientListeners()
    this.worldListeners()
  }

  clientListeners() {
    /* Add client event listeners */
    this.io.on('connection', (socket) => {
      this.addClient(socket.id)

      socket.on('selectCharacter', (character) => {
        // Player has selected a character, store their choice
        let index = this.findClient(socket.id)

        if(index != undefined) {
          if(!this.clients[index].character) {
            if(character == 'ranger' || character == 'wizard' || character == 'warrior') {
              console.log(`Select Character id: ${socket.id} char: ${character}`)
              this.clients[index].character = character
              socket.emit('play') // player can now enter the game
            }
          }
        }
      })

      /* Add event listeners */
      socket.on('loaded', () => {
        let index = this.findClient(socket.id)
        // Spawn player once client has loaded assets
        this.world.spawn('Player', {id: socket.id, character: this.clients[index].character})
        socket.emit('init', this.world.state)
      })

      socket.on('data:request', (entity) => {
        // Client is requesting data for an entity
        if(entity) {
          let {err, data} = this.gameData.get(entity)

          if(!err && data) {
            socket.emit(`data:load:${entity}`, data)
          } else {
            socket.emit(`data:error`, err)
          }
        }
      })

      socket.on('input', (velocity) => {
        this.world.input(socket.id, velocity)
      })

      socket.on('disconnect', () => {
        this.removeClient(socket.id)
      })
    })
  }

  worldListeners() {
    this.world.on('update', (state) => {
      this.io.emit('update', state)
    })

    this.world.on('spawn', (entity) => {
      console.log('spawning: ' + entity)
      this.io.emit('spawn', entity)
    })

    this.world.on('despawn', (entity) => {
      this.io.emit('despawn', entity)
    })
  }

  addClient(id) {
    // Adds a newly connected client to the world
    if(id) {
      // Add client to list of active players
      this.clients.push({id: id})
    }
  }

  removeClient(id) {
    // Removes a disconnected client from the world
    // Find/remove client
    if(id && this.clients.length > 0) {
      const index = this.findClient(id)

      if (index !== -1) {
        this.clients.splice(index, 1)
      }

      // Find/remove player from world
      this.world.despawn('Player', {id: id})
    }
  }

  findClient(id) {
    for(let i = 0; i < this.clients.length; i++) {
      if(id == this.clients[i].id) {
        return(i)
      }
    }
  }
}

module.exports = Networking
