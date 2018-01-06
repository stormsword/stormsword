/* Spritesheet Viewer for PhaserJS
    View your spritesheets in phaser
   */

import {Ranger} from '../../game/char/ranger.js'
let sprites = ['ranger', 'warrior', 'wizard']

export class Spritesheet extends Phaser.State {
  preload() {
    // Grab HTML elements from page
    this.spriteDropdown = document.getElementById('sprite')
    this.animDropdown = document.getElementById('anim')
    this.start = document.getElementById('start')
    this.end = document.getElementById('end')
    this.frameArray = document.getElementById('frameArray')


    // Default animation values
    this.startFrame = this.start.value
    this.endFrame = this.end.value

    // Load each sprite and add them to the dropdown menu
    sprites.forEach((sprite, index) => {
        this.load.image(sprite + '-image', `/char/sprites/${sprite}.png`); // Load sprite as an image so we can display the entire thing
        this.load.spritesheet(sprite + '-sheet', `/char/sprites/${sprite}.png`, 64, 64) // Load spritesheet so we can animate it

        // Populate sprites into dropdown
        let option = document.createElement('option')
        option.value = index
        option.text = sprite

        if(index == 0) {
          // Select first item by default
          option.default = true
          this.current = index
        }

        this.spriteDropdown.add(option)
    })
  }

  create() {
    this.game.world.setBounds(0, 0, 2000, 2000);  // Create a large game surface so we can move the camera

    window.socket.emit()
    this.loadSprite(sprites[this.current])

    this.UIListeners()

    // Configure input
    this.cursors = this.game.input.keyboard.createCursorKeys();
  }

  update() {
    let speed = 3
    if (this.cursors.up.isDown)
    {
        this.game.camera.y -= 4 * speed;
    }
    else if (this.cursors.down.isDown)
    {
        this.game.camera.y += 4 * speed;
    }

    if (this.cursors.left.isDown)
    {
        this.game.camera.x -= 4 * speed;
    }
    else if (this.cursors.right.isDown)
    {
        this.game.camera.x += 4 * speed;
    }
  }

  loadSprite(name) {
    this.game.world.removeAll() // Clear the world first

    /* Sprite single image */
    // Display entire Spritesheet
    this.spriteImage = this.game.add.image(0, 0, name + '-image')

    // Overlay numbers on the sheet
    // this.drawNumbers(this.spriteImage.width, this.spriteImage.height)  // Dynamic based on sprite image
    this.drawNumbers(832, this.spriteImage.height)  // Fixed sprite width

    /* Sprite animation */
    // Display individual sprite (for animation)
    this.sprite = this.game.add.sprite(600, 50, name + '-sheet', 0)
    this.sprite.scale.x = 2
    this.sprite.scale.y = 2

    // Load in sprite animation frames
    this.character = new Ranger(this.game)

    // We need to wait for the data to come back from the server
    this.character.events.onLoad.add(() => {
      this.loadAnimations()
      this.animateSprite()
    })
  }

  drawNumbers(width, height) {
    // Draws numbers over a spritesheet
    let index = 0 // index of each sprite

    for(let i = 0; i < height; i += 64) {
      for(let j = 0; j < width; j += 64) {
        this.game.add.text(j+4, i, index, { font: 'bold 14px', fill: 'red'})
        index++
      }
    }
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

  UIListeners() {
    // Configure UI listeners so we know what changed

    // Dropdown changes
    this.spriteDropdown.onchange = (event) => {
      this.current = event.target.value
      this.loadSprite(sprites[this.current])
    }

    this.animDropdown.onchange = (event) => {
      // Swap out the entire animation for another stored animation
      this.currentAnim = event.target.value

      let frames = this.anims[this.currentAnim].frames
      this.start.value = frames[0]
      this.end.value = frames[frames.length-1]
      this.frameArray = frames
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

  overrideAnim(start, end) {
    // Overrides an existing animation
    this.start.value = start
    this.end.value = end
    this.frameArray = this.animFrames(this.start.value, this.end.value)
    this.currentAnim = 'new'
    this.sprite.animations.add(this.currentAnim, this.frameArray, this.currentSpeed)
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
        this.currentSpeed = object[item].speed
      }

      this.sprite.animations.add(item, object[item].frames, object[item].speed)
      select.add(option)

      index++
    }
  }
}
