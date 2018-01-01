/* Spritesheet Viewer for PhaserJS
    View your spritesheets in phaser
   */

let sprites = ['rogue', 'warrior', 'wizard']

export class Spritesheet extends Phaser.State {
  preload() {
    sprites.forEach((sprite) => {
      console.log(sprite)
        // this.load.spritesheet(sprite, `/char/sprites/${sprite}.png`, 64, 64) // Needed for animations
        this.load.image(sprite, `/char/sprites/${sprite}.png`); // Load sprite as an image so we can display the entire thing
    })


  }

  create() {
    const spriteImage = this.game.add.image(0, 0, 'rogue')

    /* Overlay numbers on the sheet */
    this.drawNumbers(spriteImage.width, spriteImage.height)

    // marker.lineStyle(2, 0xff0000, 1);
    // marker.drawRect(0, 0, 64, 64);

    /* Animations */
    // this.rogue = this.game.add.sprite(0, 0, 'rogue', 0)
    // this.wizard = this.game.add.sprite(64, 0, 'wizard', 0)
    // this.warrior = this.game.add.sprite(128, 0, 'warrior', 0)
    // this.rogue.animations.add('walk:left')
    // this.rogue.animations.play('walk', 50, true)
  }

  drawNumbers(width, height) {
    let index = 0 // index of each sprite

    for(let i = 0; i < height; i += 64) {
      for(let j = 0; j < width; j += 64) {
        this.game.add.text(j+4, i, index, { font: 'bold 14px', fill: 'red'})
        index++
      }
    }
  }
}
