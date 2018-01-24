/* World - Initialize and update the game world */

module.exports = class World {
  constructor() {
    this.clients = []   // Currently connected clients

    this.state = {}     // State of game world
  }

  addClient(id) {
    if(id) {
      this.clients.push(id)
    }
    console.log(this.clients)
  }

  removeClient(id) {
    if(id && this.clients.length > 0) {
      const index = this.clients.indexOf(id)

      if (index !== -1) {
        this.clients.splice(index, 1)
      }
    }
    console.log(this.clients)
  }
}
