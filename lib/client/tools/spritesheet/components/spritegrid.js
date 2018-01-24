import {Ranger} from '../../../game/char/ranger.js'

export class SpriteGrid {
  constructor(state) {
      this.state = state
  }

  load(sprite) {
    this.state.load.image(sprite + '-image', `/char/sprites/${sprite}.png`); // Load sprite as an image so we can display the entire thing
  }
}
