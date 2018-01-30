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
      // Listen to data updates for this character
      this.loadData(data)
    })

    window.socket.emit('data:request', this.name) // Request new data from the server

  }

  update() {
    // this.body.velocity = 0
    let anim

    if(this.body.velocity.x != 0 || this.body.velocity.y != 0) {
      anim = 'walk'
    } else {
      anim = 'stand'
    }

    // Player is walking
    switch(this.body.facing) {
      case Phaser.UP:
        this.animations.play(`${anim}:up`)
        break
      case Phaser.DOWN:
        this.animations.play(`${anim}:down`)
        break
      case Phaser.LEFT:
        this.animations.play(`${anim}:left`)
        break
      case Phaser.RIGHT:
        this.animations.play(`${anim}:right`)
        break
    }
  }

  loadData(data) {
    // Grab data from server for this character from its definition file
    this.data = data
    this.anims = data.anims

    console.log(data)
    // Configure animations - see /tools/spritesheet
    for(let anim in this.anims) {
      console.log(anim)
      this.animations.add(anim, this.anims[anim].frames, this.anims[anim].speed)
    }

    this.events.onLoad.dispatch()
  }
}
