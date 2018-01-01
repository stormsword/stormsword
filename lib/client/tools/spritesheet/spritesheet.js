/* Spritesheet Viewer for PhaserJS
    View your spritesheets in phaser
   */

let sprites = ['rogue', 'warrior', 'wizard']

export class Spritesheet extends Phaser.State {
  preload() {
    sprites.forEach((sprite) => {
      console.log(sprite)
        this.load.spritesheet(sprite, `/char/sprites/${sprite}.png`, 64, 64)
    })
  }

  create() {
    this.rogue = this.game.add.sprite(0, 0, 'rogue', 0)
    this.wizard = this.game.add.sprite(64, 0, 'wizard', 0)
    this.warrior = this.game.add.sprite(128, 0, 'warrior', 0)

    // this.rogue.animations.add('walk:left')
    // this.rogue.animations.play('walk', 50, true)
  }
}
