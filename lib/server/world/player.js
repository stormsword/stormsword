const DOWN = 4
const UP = 3
const LEFT = 1
const RIGHT = 2

/* Class representing a player in the game) */
module.exports = class Player {
  constructor(metadata) {
    /* Variables */
    this.speed = 2

    this.id = metadata.id
    this.character = metadata.character

    this.x = Math.floor(Math.random() * 10) +1
    this.y = Math.floor(Math.random() * 10) +1

    this.body = {}  // Body maps to phaser Body object to keep data model consistent
    this.body.facing = DOWN  // Face down by default

    this.body.velocity = {
      x: 0,
      y: 0
    }
  }

  update() {
      if(this.body.velocity.x != 0 || this.body.velocity.y != 0) {
        this.move()
      }
  }

  move() {
    this.x += this.body.velocity.x * this.speed
    this.y += this.body.velocity.y * this.speed  // HACK y is negative for now due to origin position

    this.setDirection(this.body.velocity.x, this.body.velocity.y)
  }

  setDirection(x, y) {
    // Input: x and y values from keyboard
    // Output: Stores the direction the character is facing
    // Called by game state
    if(x == 1 && y == 0) {
      // Walk Right
      this.body.facing = RIGHT
    } else if (x == 1 && y == -1) {
      // Walk Up/Right
      this.body.facing = UP
    } else if (x == 0 && y == -1) {
      // Walk Up
      this.body.facing = UP
    } else if (x == -1 && y == -1) {
      // Walk Up/Left
      this.body.facing = UP
    } else if (x == -1 && y == 0) {
      // Walk Left
      this.body.facing = LEFT
    } else if (x == -1 && y == 1) {
      // Walk Down/left
      this.body.facing = DOWN
    } else if (x == 0 && y == 1) {
      // Walk Down
      this.body.facing = DOWN
    } else if (x == 1 && y == 1) {
      // Walk Down/Right
      this.body.facing = DOWN
    }
  }
}
