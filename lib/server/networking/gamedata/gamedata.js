const promisify = require('util').promisify
const jsonfile = require('jsonfile')
const readFile = promisify(jsonfile.readFile)  // Callback-based module needs to be wrapped in a promse to use `async```


module.exports = class GameData {
  constructor() {

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

  get(name) {
    // Request from client to load data for a game entity
    // Input: 'name' (Name of the entity to load)
    // Output: Returns data for a given entity

    if(name) {
      return({err: null, data: this.data[name]})
    } else {
      return({err: `'${name}' could not be found.`, data: null})
    }
  }
}
