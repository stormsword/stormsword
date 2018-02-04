/* Map - The world map a player can navigate */

module.exports = class Map {
  constructor() {
    this.TYPES = {
      grass: 0,
      wall: 1
    }

    this.tiles = [
      [1, 1, 1, 1, 1, 1, 1, 1, 1, 1],
      [1, 0, 0, 0, 0, 0, 0, 0, 0, 1],
      [1, 0, 0, 0, 0, 0, 0, 0, 0, 1],
      [1, 0, 0, 0, 0, 0, 0, 0, 0, 1],
      [1, 0, 0, 0, 0, 0, 0, 0, 0, 1],
      [1, 0, 0, 0, 0, 0, 0, 0, 0, 1],
      [1, 1, 1, 1, 1, 1, 1, 1, 1, 1]
    ]
  }

  csv() {
    let csv = ''

    for(let y = 0; y < this.tiles.length; y++) {
      for(let x = 0; x < this.tiles[y].length; x++) {
        csv += this.tiles[y][x]
        if(x < this.tiles[y].length-1) {
          csv += ','
        }
      }

      if(y < this.tiles.length) {
        csv += '\n'
      }
    }

    return(csv)
  }
}
