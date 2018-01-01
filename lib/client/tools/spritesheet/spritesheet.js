/* Spritesheet Viewer for PhaserJS
    View your spritesheets in phaser
   */

let sprites = ['rogue', 'warrior', 'wizard']

export class Spritesheet extends Phaser.State {
  preload() {
    this.dropdown = document.getElementById('sprite')
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

        this.dropdown.add(option)
    })
  }

  create() {
    this.game.world.setBounds(0, 0, 2000, 2000);  // Create a large game surface so we can move the camera

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
    // Display Spritesheet as a single image
    this.spriteImage = this.game.add.image(0, 0, name + '-image')

    // Overlay numbers on the sheet
    // this.drawNumbers(this.spriteImage.width, this.spriteImage.height)  // Dynamic based on sprite image
    this.drawNumbers(832, this.spriteImage.height)  // Fixed sprite width

    /* Sprite animation */
    // Display individual sprite (for animation)
    this.sprite = this.game.add.sprite(600, 50, name + '-sheet', 0)

    this.animateSprite()
  }

  animateSprite(start, end) {
    let speed = 15

    let animationFrames = this.animFrames(this.startFrame, this.endFrame)
    frameArray.value = '[' + animationFrames  + ']' // Output animation frames for easy copy/pasta

    this.sprite.animations.add('walk:left', animationFrames)
    this.sprite.animations.play('walk:left', speed, true)
  }

  UIListeners() {
    // Configure UI listeners so we know what changed

    // Dropdown changes
    this.dropdown.onchange = (event) => {
      this.current = event.target.value
      this.loadSprite(sprites[this.current])
    }

    // First Frame changes
    this.start.onchange = (event) => {
      this.startFrame = event.target.value
      this.animateSprite()
    }

    // Last Frame changes
    this.end.onchange = (event) => {
      this.endFrame = event.target.value
      this.animateSprite()
    }
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
    // console.log(array)
    return(array)
  }

}
