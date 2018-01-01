/* Start the game */

export class Play extends Phaser.State {
  preload() {
    this.load.spritesheet('warrior', './char/sprites/warrior.png', 62, 66, 28)
    this.warrior = this.game.add.sprite(200, 200, 'warrior')
  }

  create() {
    this.warrior.animations.add('walk')
    this.warrior.animations.play('walk', 50, true)
  }
}
