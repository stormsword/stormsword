/* Initialize Game Engine (game engine) */
import PIXI from 'expose-loader?PIXI!phaser-ce/build/custom/pixi.js';
import p2 from 'expose-loader?p2!phaser-ce/build/custom/p2.js';
import Phaser from 'expose-loader?Phaser!phaser-ce/build/custom/phaser-split.js';

import {Spritesheet} from './spritesheet/spritesheet.js'
import {LevelEditor} from './level-editor/level-editor.js'

/* Connect to socket server */
// Detect hostname for socket.io to connect
const hostname = window && window.location && window.location.hostname;

import openSocket from 'socket.io-client';
window.socket = openSocket(hostname + ':3000'); // Connect to the server to get client updates

class Tools extends Phaser.Game {
  constructor() {
    super(800, 600, Phaser.AUTO, 'game', { create: () => {
      this.state.add('Spritesheet', Spritesheet, false)
      this.state.add('LevelEditor', LevelEditor, false)
      this.state.start(window.state)  // Select a tool by passing in a url
    }})
  }
}

let tools = new Tools()
