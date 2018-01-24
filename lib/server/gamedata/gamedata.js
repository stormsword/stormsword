const promisify = require('util').promisify
const jsonfile = require('jsonfile')
const readFile = promisify(jsonfile.readFile)  // Callback-based module needs to be wrapped in a promse to use `async```


module.exports = class GameData {
  constructor(socket) {

    this.GAME_DIR = 'lib/client/game/'

    this.entities = [  // List of game entities
      {
        'name': 'ranger',
        'location': 'char/ranger_data.json'
      }
    ]

    this.data = {}

    this.loadData() // Load game data objects so we can refer to/manipulate them later
  }

  async loadData() {
    this.entities.forEach(async (entity) => {
      this.data[entity.name] = await readFile(this.GAME_DIR + entity.location)
    })
  }

  sendData(socket, name) {
    // Request from client to load data for a game entity
    // Input: 'name' (Name of the entity to load)
    // Output: Sends an event (data:load:${name}) back to the client with game data -or- sends an error

    // Check that name exists
    // Load data
    // Send data down to client
    if(name) {
      socket.emit('data:load:ranger', this.data[name])
    } else {
      socket.emit('data:error', `'${name}' could not be found.`)
    }
  }

  addClient(socket) {
    if(socket) {
      socket.on('data:request', (name) => {
        this.sendData(socket, name)
      })
    }

  }
}
