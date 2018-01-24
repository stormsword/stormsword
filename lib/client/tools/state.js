/* Extension to add state to States inspired by React */

export class State extends Phaser.State {
  constructor() {
    super()
    this.data = {}
  }

  setData(_data) {
    this.data = Object.assign({}, this.data, _data)
  }
}
