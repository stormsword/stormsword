/* server.js - The main stormsword server file */
const path = require('path')

const express = require('express')
let app = express()

let io

const GameData = require('./gamedata/gamedata.js')
const World = require('./world/world.js')

class Server {
  constructor() {
    this.clients = []   // Currently connected clients

    /* Setup Express */
    app.use(express.static(path.join(__dirname, 'static')));
    app.use(express.static(path.join(__dirname, '../client/game/')))
    app.set('views', path.join(__dirname, 'views'));
    app.set('view engine', 'pug');

    // configure primary route
    app.get('/', (req, res, next) => {
      res.render('index')
    })

    app.get('/tools/:tool', (req, res, next) => {
      res.render(req.params.tool)
    })

    /* Setup server */
    const server = require('http').Server(app);
    server.listen(3000);

    /* Setup socket.io */
    io = require('socket.io')(server)

    /* Load Game Data into memory */
    let gamedata = new GameData()

    /* Load World */
    this.world = new World()

    this.world.on('spawn', (entity) => {
      io.emit('spawn', entity)
    })

    this.world.on('despawn', (entity) => {
      io.emit('despawn', entity)
    })

    /* Add client event listeners */
    io.on('connection', (socket) => {
      // gamedata.addClient(socket)
      this.addClient(socket.id)

      /* Add event listeners */
      socket.on('disconnect', () => {
        this.removeClient(socket.id)
      })
    })
  }

addClient(id) {
  // Adds a newly connected client to the world
  if(id) {
    // Add client to list of active players
    this.clients.push(id)

    // Spawn new player
    this.world.spawn('Player', {id: id})
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

module.exports = Server
