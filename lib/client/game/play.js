/* Start the game */

import {Ranger} from './char/ranger.js'

export class Play extends Phaser.State {

  preload() {
    this.load.spritesheet('ranger', './char/sprites/ranger.png', 64, 64)
  }

  create() {
    this.ranger = new Ranger(this.game, 350, 300)
    this.game.add.existing(this.ranger)

    this.cursors = this.game.input.keyboard.createCursorKeys();
  }

  update() {
    let x = 0
    let y = 0

    if(this.cursors.up.isDown) {
      y = 1
    } else if(this.cursors.down.isDown) {
      y = -1
    }

    if(this.cursors.right.isDown) {
      x = 1
    } else if(this.cursors.left.isDown) {
      x = -1
    }

    this.ranger.move(x, y)
  }
}
