/* Start the game */

let walk = []
export class Play extends Phaser.State {

  preload() {
    this.load.spritesheet('rogue', './char/sprites/rogue.png', 64, 64)
  }

  create() {
    this.rogue = this.game.add.sprite(0, 0, 'rogue', 19)

    // this.rogue.animations.add('walk:left')
    // this.rogue.animations.play('walk', 50, true)
  }
}
