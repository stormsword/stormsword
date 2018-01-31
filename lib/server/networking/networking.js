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
      // gamedata.addClient(socket)
      this.addClient(socket.id)
      this.world.spawn('Player', {id: socket.id})

      /* Add event listeners */
      socket.on('loaded', () => {
        // Spawn player once client has loaded assets
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
      this.clients.push(id)
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

      // Find/remove player from world
      this.world.despawn('Player', {id: id})
    }
  }
}

module.exports = Networking
