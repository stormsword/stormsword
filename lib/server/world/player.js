/* Class representing a player in the game) */
module.exports = class Player {
  constructor(metadata) {
    this.id = metadata.id
    this.x = Math.floor(Math.random() * 10) +1
    this.y = Math.floor(Math.random() * 10) +1
  }
}
