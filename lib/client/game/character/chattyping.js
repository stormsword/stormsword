export class ChatTyping extends Phaser.Sprite {
  constructor(game, x, y, name, character) {
    super(game, x, y, name)

    this.character = character  // Reference to the character i'm attached to

    this.animations.add('default', [0,1,2,3,4,5,6,7,8,9,10,11], 15)

    this.game.add.existing(this)
  }

  update() {
    this.animations.play('default')

    // Grab location from parent character
    this.x = this.character.x+16
    this.y = this.character.y
  }
}
