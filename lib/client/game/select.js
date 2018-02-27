/* Start the game */

import {Debug} from './debug/debug.js'

import {Character} from './character/character.js'

export class Select extends Phaser.Scene {

  constructor() {
    super({key: 'Select'})
    this.debug = new Debug()
  }

  preload() {
    this.load.image('selector', './character/sprites/selector.png')
    this.load.spritesheet('ranger', './character/sprites/ranger.png', {frameWidth: 64, frameHeight: 64 })
    this.load.spritesheet('warrior', './character/sprites/warrior.png', {frameWidth: 64, frameHeight: 64 })
    this.load.spritesheet('wizard', './character/sprites/wizard.png', {frameWidth: 64, frameHeight: 64 })
  }

  create() {
    window.socket.on('play', () => {
      // Player selected a character, start the actual game!
      // Kill off existing sprites
      this.selector.destroy()
      this.warrior.destroy()
      this.wizard.destroy()
      this.ranger.destroy()
      console.log('Loading Scene: Play')
      this.scene.start('Play')
    })

    // Define boundaries of world
    this.centerX = this.sys.game.config.width / 2
    this.centerY = this.sys.game.config.height / 2

    // Title Text
    let titleStyle = { font: "36px Georgia", fill: "#ffffff", align: "center" };
    this.add.text(this.centerX-175, 50, 'Choose your character', titleStyle)

    // Selector (triangle)

    this.selector = new Phaser.GameObjects.Sprite(this, this.centerX, 165, 'selector')
    this.selector.scaleX = 0.025
    this.selector.scaleY = -0.025
    this.selector.alpha = 0
    this.add.existing(this.selector)

    // Add UI (characters)
    let charTextStyle = { font: "14px Georgia", fill: "#ffffff", align: "center" };

    // Warrior
    this.warrior = new Character('warriorSelect', this, this.centerX-150, 200, 'warrior')
    this.add.text(this.warrior.x-20, this.warrior.y+40, 'warrior', charTextStyle)
    this.configureCharacter(this.warrior)

    // Wizard
    this.wizard = new Character('wizardSelect', this, this.centerX, 200, 'wizard')
    this.add.text(this.wizard.x-20, this.wizard.y+40, 'wizard', charTextStyle)
    this.configureCharacter(this.wizard)

    // Ranger
    this.ranger = new Character('rangerSelect', this, this.centerX+150, 200, 'ranger')
    this.add.text(this.ranger.x-20, this.ranger.y+40, 'ranger', charTextStyle)
    this.configureCharacter(this.ranger)

    // Configure Input
    this.cursors = this.input.keyboard.createCursorKeys();
  }

  configureCharacter(character) {
    // this.add.existing(character)
    let test = this.physics.add.sprite()

    character.body.facing = Phaser.DOWN

    // Configure mouse events
    character.setInteractive()

    character.on('pointerdown', (pointer) => {
      this.onClick(character.name)
    })

    character.on('pointerover', (pointer) => {
      this.onMouseOver(character.name)
    })

    character.on('pointerout', (pointer) => {
      this.onMouseOut(character.name)
    })
    // character.events.onInputDown.add(this.onClick, this)
    // character.events.onInputOver.add(this.onMouseOver, this)
    // character.events.onInputOut.add(this.onMouseOut, this)
  }

  onClick(sprite) {
    switch(sprite) {
      case 'ranger':
        // Player selected a ranger
        window.socket.emit('selectCharacter', 'ranger')
        break;
      case 'wizard':
        // Player selected a wizard
        window.socket.emit('selectCharacter', 'wizard')
        break;
      case 'warrior':
        // Player selected a warrior
        window.socket.emit('selectCharacter', 'warrior')
        break;
    }
  }

  onMouseOver(sprite) {
    // Animate selector arrow
    this.tweens.add({
      targets: [
        this.selector
      ],
      alpha: 1,
      duration: 150,
      ease: 'Linear'
    })

    switch(sprite) {
      case 'ranger':
        this.selector.x = this.ranger.x
        this.ranger.body.velocity.y = 0.00001 // HACK - Not sure how to separate state maching from movement
        break;
      case 'wizard':
        this.selector.x = this.wizard.x
        this.wizard.body.velocity.y = 0.00001
        break;
      case 'warrior':
        this.selector.x = this.warrior.x
        this.warrior.body.velocity.y = 0.00001
        break;
    }

    this.sys.canvas.style.cursor = "pointer";
  }

  onMouseOut(sprite) {
    // Stop animating selector arrow
    this.tweens.add({
      targets: [
        this.selector
      ],
      alpha: 0,
      duration: 150,
      ease: 'Linear'
    })

    // Stop animating player
    switch(sprite) {
      case 'ranger':
        this.ranger.body.velocity.y = 0
        break;
      case 'wizard':
        this.wizard.body.velocity.y = 0
        break;
      case 'warrior':
        this.warrior.body.velocity.y = 0
        break;
    }

    // Reset mouse cursor
    this.sys.canvas.style.cursor = "default";
  }
}
