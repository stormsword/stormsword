// import {Character} from '../../../game/character/character.js'

export class SpriteGrid {
  constructor(state) {
      this.state = state
  }

  load(sprite) {
    this.state.load.image(sprite + '-image', `/character/sprites/${sprite}.png`); // Load sprite as an image so we can display the entire thing
  }
}
