/* Start the game */

import {Debug} from './debug/debug.js'

import {Character} from './char/character.js'

export class Select extends Phaser.State {

  constructor() {
    super()
    this.debug = new Debug()
  }

  preload() {
    this.load.spritesheet('ranger', './char/sprites/ranger.png', 64, 64)
    this.load.spritesheet('warrior', './char/sprites/warrior.png', 64, 64)
    this.load.spritesheet('wizard', './char/sprites/wizard.png', 64, 64)
  }

  create() {

    // Add UI (characters)
    let warrior = new Character('test1', this.game, this.game.world.centerX, 200, 'warrior')
    let wizard = new Character('test2', this.game, this.game.world.centerX, 200, 'wizard')
    this.ranger = new Character('test3', this.game, this.game.world.centerX, 200, 'ranger')
    this.game.add.existing(this.ranger)
    this.game.physics.arcade.enable(this.ranger)

    this.ranger.body.facing = Phaser.DOWN

    this.ranger.inputEnabled = true
    this.ranger.events.onInputDown.add(this.onClick, this)

    // Configure Input
    this.cursors = this.game.input.keyboard.createCursorKeys();
  }

  onClick(sprite) {
    switch(sprite.key) {
      case 'ranger':
        console.log('click!')
        // Player selected a ranger
    }
  }
}
