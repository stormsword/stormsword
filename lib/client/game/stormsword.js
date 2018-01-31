/* Initialize Game Engine (game engine) */
import PIXI from 'expose-loader?PIXI!phaser-ce/build/custom/pixi.js';
import p2 from 'expose-loader?p2!phaser-ce/build/custom/p2.js';
import Phaser from 'expose-loader?Phaser!phaser-ce/build/custom/phaser-split.js';

import {Play} from './play.js'

/* Connect to socket server */
// Detect hostname for socket.io to connect
const hostname = window && window.location && window.location.hostname;

import openSocket from 'socket.io-client';
window.socket = openSocket(hostname + ':3000'); // Connect to the server to get client updates

class Stormsword extends Phaser.Game {
  constructor() {
    super(800, 600, Phaser.AUTO, 'game', { create: () => {
      this.state.add('Play', Play, false)
      this.state.start('Play')
    }})
  }
}

let game = new Stormsword()
