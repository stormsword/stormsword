export class ChatBubble extends Phaser.Sprite {
  constructor(game, x, y, name, character, message) {
    super(game, x, y, name)

    this.character = character  // Reference to the character i'm attached to

    this.message = message
    console.log(this.message)

    // Setup timed event to fade out and destroy myself
    this.events.onDone = new Phaser.Signal()
    // Fade out chat bubble after 3s
    let fadeOut = this.game.add.tween(this).to({ alpha: 0 }, 300, Phaser.Easing.Linear.None, true, 3500, 0);
    fadeOut.onComplete.add(() => {this.events.onDone.dispatch()}) // Let parent know animation is complete so it cna be garbage collected

    this.game.add.existing(this)

    // // text for chat bubble
    // var style = { font: "7px Arial", wordWrap: true, wordWrapWidth: this.chatSprite.width, align: "center", fill: "#000000" };
    // this.text = this.game.add.text(this.x, this.y, "- text on a sprite -\ndrag me", style);
    // this.text.lineSpacing = -8
  }

  update() {
    // Grab location from parent character
    this.x = this.character.x - 8
    this.y = this.character.y - 34
  }
}
