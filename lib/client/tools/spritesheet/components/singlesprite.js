import {Character} from '../../../game/character/character.js'

export class SingleSprite {
  constructor(state) {
      this.state = state
  }

  preload() {
    // Initialize html elements
    this.animDropdown = document.getElementById('anim')
    this.start = document.getElementById('start')
    this.end = document.getElementById('end')
    this.frameArray = document.getElementById('frameArray')

    // Set default animation values
    this.startFrame = this.start.value
    this.endFrame = this.end.value
  }

  create() {
    this.UIListeners()
  }

  loadAnimations() {
    this.anims = this.character.anims

    // Clear select object
    this.removeOptions(this.animDropdown)

    this.populateAnimOptions(this.animDropdown, this.anims)
  }

  animateSprite() {
    this.sprite.animations.play(this.currentAnim, this.currentSpeed, true)
  }

  renderSprite(name) {
    /* SingleSprite animation */
    // Display individual sprite (for animation)
    this.sprite = this.state.game.add.sprite(600, 50, name + '-sheet', 0)
    this.sprite.scale.x = 2
    this.sprite.scale.y = 2

    // Load in sprite animation frames
    this.character = new Character(null, this.state.game, 0, 0, name)

    // We need to wait for the data to come back from the server
    this.character.events.onLoad.add(() => {
      this.loadAnimations()
      this.animateSprite()
    })
  }


  UIListeners() {
    this.animDropdown.onchange = (event) => {
      // Swap out the entire animation for another stored animation
      this.currentAnim = event.target.value

      let frames = this.anims[this.currentAnim].frames
      this.start.value = frames[0]
      this.end.value = frames[frames.length-1]
      this.frameArray.value = '[' + frames + ']'
      this.animateSprite()
    }

    // First Frame changes
    this.start.onchange = (event) => {
      // Override first frame of animation
      this.overrideAnim(event.target.value, this.end.value)
      this.animateSprite()
    }

    // Last Frame changes
    this.end.onchange = (event) => {
      this.overrideAnim(this.start.value, event.target.value)
      this.animateSprite()
    }
  }

  load(sprite) {
    this.state.load.spritesheet(sprite + '-sheet', `/char/sprites/${sprite}.png`, 64, 64) // Load spritesheet so we can animate it
  }

  populateAnimOptions(select, object) {
    // Hydrates an array with data pass in

    this.frames = object
    let index = 0
    for(let item in object) {
      // Push objects onto select object
      // Populate sprites into dropdown
      let option = document.createElement('option')
      option.value = item
      option.text = item


      if(index == 0) {
        // Select first item by default
        option.default = true
        this.currentAnim = item
        let frames = object[item].frames
        this.start.value = frames[0]
        this.end.value = frames[frames.length-1]
        this.frameArray.value = '[' + frames + ']'
        this.currentSpeed = object[item].speed
      }

      this.sprite.animations.add(item, object[item].frames, object[item].speed)
      select.add(option)

      index++
    }
  }

  overrideAnim(start, end) {
    // Overrides an existing animation
    this.start.value = start
    this.end.value = end
    let frames = this.animFrames(this.start.value, this.end.value)
    this.frameArray.value = '[' + frames + ']'
    this.currentAnim = 'new'
    this.sprite.animations.add(this.currentAnim, frames, this.currentSpeed)
  }

  animFrames(start, end) {
    // Creates an array of frames for use in animations
    // Input: starting frame of the animation (i.e. 112)
    // Output: ending frame fo the animation (i.e. 119)
    // Result: [112, 113, 114, 115, 116, 117, 118, 119]
    let array = []

    // Check for strings etc
    start = parseInt(start)
    end = parseInt(end)

    for(let i = start; i < end+1; i++) {
        array.push(i)
    }

    return(array)
  }

  removeOptions(select) {
    // Clears a <select> object on the page so we can repopulate it
    // Input: <select> object
    // Output: null

    for(let i = select.options.length - 1; i >= 0; i--) {
      select.remove(i)
    }
  }

}
