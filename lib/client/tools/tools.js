/* Initialize Game Engine (game engine) */
import PIXI from 'expose-loader?PIXI!phaser-ce/build/custom/pixi.js';
import p2 from 'expose-loader?p2!phaser-ce/build/custom/p2.js';
import Phaser from 'expose-loader?Phaser!phaser-ce/build/custom/phaser-split.js';

import {Spritesheet} from './spritesheet/spritesheet.js'

class Tools extends Phaser.Game {
  constructor() {
    super(800, 600, Phaser.AUTO, 'game', { create: () => {
      this.state.add('Spritesheet', Spritesheet, false)
      this.state.start('Spritesheet')
      console.log('tools!')
    }})
  }
}

let tools = new Tools()
