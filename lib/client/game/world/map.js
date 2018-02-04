export class Map {
  constructor(state) {
    this.state = state
    this.tiles = []
    this.tileCSV = ''
  }

  load(tiles, csv, group) {
    this.tiles = tiles
    this.tileCSV = csv

    let width = 2000
    let height = 2000

    // this.currentLayer = this.tilemap.create('level1', width / 16, height / 16, 16, 16);  // Layer should be as big as the world

    this.state.game.cache.addTilemap('tilemap', null, csv, Phaser.Tilemap.CSV)

    this.tilemap = this.state.game.add.tilemap('tilemap', 16, 16)
    this.tilemap.addTilesetImage('grass')

    let tileLayer = this.tilemap.createLayer(0)
    group.add(tileLayer)

    tileLayer.resizeWorld()
  }
}
