/* Ranger Character Class */

export class Ranger extends Phaser.Sprite {
  constructor(id, game, x, y) {
    super(game, x, y, 'ranger')
    this.id = id
    this.name = 'ranger'

    this.animationSpeed = 15

    // Add event so other modules can listen for load to be complete
    this.events.onLoad = new Phaser.Signal()


    // Load data from the server
    window.socket.on(`data:load:${this.name}`, (data) => {
      this.loadData(data)})  // Listen to data updates for this character

    window.socket.emit('data:request', this.name) // Request new data from the server
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

  loadData(data) {
    // Grab data from server for this character from its definition file
    this.data = data
    this.anims = data.anims

    // Configure animations - see /tools/spritesheet
    for(let anim in this.anims) {
      this.animations.add(anim, this.anims[anim].frames, this.anims[anim].speed)
    }
    this.events.onLoad.dispatch()
  }
}
