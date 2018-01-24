/* server.js - The main stormsword server file */
const path = require('path')

const express = require('express')
let app = express()

let io

const GameData = require('./gamedata/gamedata.js')

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

    /* Load Game Data into memory */
    let gamedata = new GameData()
    
    /* Setup server */
    const server = require('http').Server(app);
    server.listen(3000);

    /* Setup socket.io */
    io = require('socket.io')(server)

    io.on('connection', function(socket) {
      gamedata.addClient(socket)
    })


  }
}

module.exports = Server
