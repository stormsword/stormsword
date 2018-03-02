module.exports = class Entity {
  constructor(metadata) {
    this.type = metadata.type

    this.x = metadata.x
    this.y = metadata.y

    this.blocking = true  // Can a player walk through it?

  }

  update() {

  }
}
