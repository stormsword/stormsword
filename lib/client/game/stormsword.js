/* Initialize Game Engine (game engine) */
import Phaser from 'expose-loader?Phaser!phaser/dist/phaser.js';

import {Select} from './select.js'
import {Play} from './play.js'

/* Connect to socket server */
// Detect hostname for socket.io to connect
const hostname = window && window.location && window.location.hostname;

import openSocket from 'socket.io-client';
window.socket = openSocket(hostname + ':3000'); // Connect to the server to get client updates

class Stormsword extends Phaser.Game {
  constructor() {
    super(900, 600, Phaser.AUTO, 'game', {
      create: () => {
        this.state.add('Select', Select, false)
        this.state.add('Play', Play, false)

        // Prevent right clicks from popping up a context menu
        this.canvas.oncontextmenu = function (e) { e.preventDefault(); }
        this.stage.disableVisibilityChange = true

        // this.state.start('Select')
        this.state.start('Play')
        window.socket.emit('selectCharacter', 'ranger')
    }})
  }
}

let game = new Stormsword()
