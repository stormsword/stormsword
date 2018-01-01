/* Initialize Game Engine (game engine) */
import PIXI from 'expose-loader?PIXI!phaser-ce/build/custom/pixi.js';
import p2 from 'expose-loader?p2!phaser-ce/build/custom/p2.js';
import Phaser from 'expose-loader?Phaser!phaser-ce/build/custom/phaser-split.js';

import {Play} from './play.js'

class Stormsword extends Phaser.Game {
  constructor() {
    super(800, 600, Phaser.AUTO, '', { create: () => {
      this.state.add('Play', Play, false)
      this.state.start('Play')
    }})
  }
}

let game = new Stormsword()
