/* server.js - The main stormsword server file */
const path = require('path')

const express = require('express')
let app = express()

let io

const GameData = require('./gamedata/gamedata.js')
const World = require('./world/world.js')
const Networking = require('./networking/networking.js')

class Server {
  constructor() {
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

    /* Setup socket.io for client connections*/
    io = require('socket.io')(server)

    /* Load Game Data into memory */
    let gamedata = new GameData()

    /* Load World */
    let world = new World()

    /* Initialize client/server connections */
    let networking = new Networking(world, io)
  }
}

module.exports = Server
