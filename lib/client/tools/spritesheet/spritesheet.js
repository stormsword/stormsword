/* Spritesheet Viewer for PhaserJS
    View your spritesheets in phaser
   */

import {SpriteGrid} from './components/spritegrid.js'
import {SingleSprite} from './components/singlesprite.js'

let sprites = ['ranger', 'warrior', 'wizard']

export class Spritesheet extends Phaser.State {
  constructor() {
    super()

    this.spriteGrid = new SpriteGrid(this)  // Load a grid view of all frames from a given sprite
    this.singleSprite = new SingleSprite(this)  // Load a single sprite sheet
  }

  preload() {
    // Grab HTML elements from page
    this.spriteDropdown = document.getElementById('sprite')

    // Load each sprite and add them to the dropdown menu
    sprites.forEach((sprite, index) => {
      this.spriteGrid.load(sprite)
      this.singleSprite.load(sprite)

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

    this.singleSprite.preload()
  }

  create() {
    this.game.world.setBounds(0, 0, 2000, 2000);  // Create a large game surface so we can move the camera

    this.renderSprite(sprites[this.current])

    this.UIListeners()

    // Configure camera input
    this.cursors = this.game.input.keyboard.createCursorKeys();

    this.singleSprite.create()
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

  renderSprite(name) {
    this.game.world.removeAll() // Clear the world first

    /* Spritesheet */
    // Display entire Spritesheet
    this.spriteImage = this.game.add.image(0, 0, name + '-image')

    // Overlay numbers on the sheet
    // this.drawNumbers(this.spriteImage.width, this.spriteImage.height)  // Dynamic based on sprite image
    this.drawNumbers(832, this.spriteImage.height)  // Fixed sprite width

    this.singleSprite.renderSprite(name)
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

  UIListeners() {
    // Configure UI listeners so we know what changed

    // Dropdown changes
    this.spriteDropdown.onchange = (event) => {
      this.current = event.target.value
      this.renderSprite(sprites[this.current])
    }

    // Call children UI listeners
    this.singleSprite.UIListeners()

  }
}
