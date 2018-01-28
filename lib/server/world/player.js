/* Class representing a player in the game) */
module.exports = class Player {
  constructor(metadata) {
    this.id = metadata.id
    this.x = Math.floor(Math.random() * 10) +1
    this.y = Math.floor(Math.random() * 10) +1

    this.speed = 1

    this.velocity = {
      x: 0,
      y: 0
    }
  }

  update() {
      if(this.velocity.x != 0 || this.velocity.y != 0) {
        this.move()
      }
  }

  move() {
    this.x += this.velocity.x * this.speed
    this.y += this.velocity.y * this.speed  // HACK y is negative for now due to origin position
  }
}
