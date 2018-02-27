/* Initialize Game Engine (game engine) */
import Phaser from 'expose-loader?Phaser!phaser/dist/phaser.js';

import {Select} from './select.js'
import {Play} from './play.js'

/* Connect to socket server */
// Detect hostname for socket.io to connect
const hostname = window && window.location && window.location.hostname;

import openSocket from 'socket.io-client';
window.socket = openSocket(hostname + ':3000'); // Connect to the server to get client updates


const config = {
  type: Phaser.CANVAS,
  parent: 'phaser-example',
  width: 800,
  height: 600,
  physics: {
    default: 'arcade'
  },
  scene: [Select, Play]
}

class Stormsword extends Phaser.Game {
  constructor(config) {
    super(config)
    // {
    //   create: () => {
    //     this.scenes.add('Select', Select, false)
    //     this.states.add('Play', Play, false)
    //
    //     // Prevent right clicks from popping up a context menu
    //     this.canvas.oncontextmenu = function (e) { e.preventDefault(); }
    //     this.stage.disableVisibilityChange = true
    //
    //     // this.state.start('Select')
    //     this.scenes.start('Play')
    //     window.socket.emit('selectCharacter', 'ranger')
    //     console.log('test')
    // }})
  }
}



let game = new Stormsword(config)
