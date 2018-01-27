/* Networking.js - Handles all i/o from client and server */

class Networking {
  constructor(world, io) {
    this.clients = []   // Currently connected clients

    this.world = world
    this.io = io

    this.clientListeners()
    this.worldListeners()
  }

  clientListeners() {
    /* Add client event listeners */
    this.io.on('connection', (socket) => {
      // gamedata.addClient(socket)
      this.addClient(socket.id)

      /* Add event listeners */
      socket.on('loaded', () => {
          // Spawn player once client has loaded assets
          this.world.spawn('Player', {id: socket.id})
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
      this.world.on('spawn', (entity) => {
        this.io.emit('spawn', entity)
      })

      this.world.on('despawn', (entity) => {
        this.io.emit('despawn', entity)
      })

      this.world.on('update', (state) => {
        this.io.emit('update', state)
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
