/* Spritesheet Viewer for PhaserJS
    View your spritesheets in phaser
   */

let sprites = ['rogue', 'warrior', 'wizard']
let current // Select first item by default
let dropdown

export class Spritesheet extends Phaser.State {
  preload() {
    dropdown = dropdown = document.getElementById('sprite')

    // Load each sprite and add them to the dropdown menu
    sprites.forEach((sprite, index) => {
        // this.load.spritesheet(sprite, `/char/sprites/${sprite}.png`, 64, 64) // Needed for animations
        this.load.image(sprite, `/char/sprites/${sprite}.png`); // Load sprite as an image so we can display the entire thing
        // Populate sprites into dropdown
        let option = document.createElement('option')
        option.value = index
        option.text = sprite
        if(index == 0) {
          option.default = true
          current = index
        }
        dropdown.add(option)
    })
  }

  create() {
    this.loadSprite(sprites[current])


    /* Animations */
    // this.rogue = this.game.add.sprite(0, 0, 'rogue', 0)
    // this.wizard = this.game.add.sprite(64, 0, 'wizard', 0)
    // this.warrior = this.game.add.sprite(128, 0, 'warrior', 0)
    // this.rogue.animations.add('walk:left')
    // this.rogue.animations.play('walk', 50, true)

    // Listen for dropdown changes
    dropdown.onchange = (event) => {
      current = event.target.value
      this.loadSprite(sprites[current])
    }
  }

  loadSprite(name) {
    this.game.world.removeAll() // Clear the world first

    let spriteImage = this.game.add.image(0, 0, name)

    /* Overlay numbers on the sheet */
    this.drawNumbers(spriteImage.width, spriteImage.height)

  }

  drawNumbers(width, height) {
    let index = 0 // index of each sprite

    for(let i = 0; i < height; i += 64) {
      for(let j = 0; j < width; j += 64) {
        this.game.add.text(j+4, i, index, { font: 'bold 14px', fill: 'red'})
        index++
      }
    }
  }
}
