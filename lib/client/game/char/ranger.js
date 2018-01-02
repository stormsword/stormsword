/* Ranger Character Class */

export class Ranger extends Phaser.Sprite {
  constructor(game, x, y) {
    super(game, x, y, 'ranger')

    this.name = 'ranger'

    this.loadAnimations()
  }

  update() {
    // this.body.velocity = 0
  }

  move(x, y) {
    // Called by game state
    if(x == 1 && y == 0) {
      // Walk Right
      this.animations.play('walk:right')
      this.body.velocity.x = 250
    } else if (x == 1 && y == 1) {
      // Walk Up/Right
      this.animations.play('walk:up')
    } else if (x == 0 && y == 1) {
      // Walk Up
      this.animations.play('walk:up')
    } else if (x == -1 && y == 1) {
      // Walk Up/Left
      this.animations.play('walk:up')
    } else if (x == -1 && y == 0) {
      // Walk Left
      this.animations.play('walk:left')
    } else if (x == -1 && y == -1) {
      // Walk Down/left
      this.animations.play('walk:down')
    } else if (x == 0 && y == -1) {
      // Walk Down
      this.animations.play('walk:down')
    } else if (x == 1 && y == -1) {
      // Walk Down/Right
      this.animations.play('walk:down')
    }
  }

  loadAnimations() {
    // Configure animations - see /tools/spritesheet
    let speed = 15  // Speed to play animations (frames per second)
    // Idle
    this.animations.add('idle:right', [143], speed)
    this.animations.add('idle:left', [117], speed)
    this.animations.add('idle:up', [104], speed)
    this.animations.add('idle:down', [130], speed)

    // Walk
    this.animations.add('walk:right', [143,144,145,146,147,148,149], speed)
    this.animations.add('walk:left', [117,118,119,120,121,122,123], speed)
    this.animations.add('walk:up', [104,105,106,107,108,109,110,111,112], 20)
    this.animations.add('walk:down', [130,131,132,133,134,135,136,137,138], 20)

    // Shoot
    this.animations.add('shoot:right', [247,248,249,250,251,252,253,254,255,256,257,258,259], speed)
    this.animations.add('shoot:left', [221,222,223,224,225,226,227,228,229,230,231], speed)
    this.animations.add('shoot:up', [208,209,210,211,212,213,214,215,216,217,218,219,220], speed)
    this.animations.add('shoot:down', [234,235,236,237,238,239,240,241,242,243,244,245,246], speed)

    // Death
    this.animations.add('death', [260, 261, 261, 262, 262, 262, 262, 262, 263, 263, 264, 264, 264, 264, 264, 265], speed)
  }
}
