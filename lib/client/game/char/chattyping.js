export class ChatTyping extends Phaser.Sprite {
  constructor(game, x, y, name) {
    super(game, x, y, name)

    this.animations.add('default', [0,1,2,3,4,5,6,7,8,9,10,11], 15)
  }

  update() {
    this.animations.play('default')
  }
}
