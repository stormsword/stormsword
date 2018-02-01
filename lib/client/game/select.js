/* Start the game */

import {Debug} from './debug/debug.js'

import {Character} from './char/character.js'

export class Select extends Phaser.State {

  constructor() {
    super()
    this.debug = new Debug()
  }

  preload() {
    this.load.image('selector', './char/sprites/selector.png')
    this.load.spritesheet('ranger', './char/sprites/ranger.png', 64, 64)
    this.load.spritesheet('warrior', './char/sprites/warrior.png', 64, 64)
    this.load.spritesheet('wizard', './char/sprites/wizard.png', 64, 64)
  }

  create() {
    // Title Text
    let titleStyle = { font: "36px Georgia", fill: "#ffffff", align: "center" };
    this.game.add.text(this.world.centerX-150, 50, 'Choose your character', titleStyle)

    // Selector (triangle)
    this.selector = new Phaser.Sprite(this.game, this.world.centerX, 197, 'selector')
    this.selector.scale.setTo(0.025, -0.025)
    this.selector.alpha = 0
    this.game.add.existing(this.selector)

    // Add UI (characters)
    let charTextStyle = { font: "14px Georgia", fill: "#ffffff", align: "center" };

    // Warrior
    this.warrior = new Character('warriorSelect', this.game, this.game.world.centerX-150, 200, 'warrior')
    this.game.add.text(this.warrior.x+10, this.warrior.centerY+40, 'warrior', charTextStyle)
    this.configureCharacter(this.warrior)

    // Wizard
    this.wizard = new Character('wizardSelect', this.game, this.game.world.centerX, 200, 'wizard')
    this.game.add.text(this.wizard.x+10 , this.wizard.centerY+40, 'wizard', charTextStyle)
    this.configureCharacter(this.wizard)

    // Ranger
    this.ranger = new Character('rangerSelect', this.game, this.game.world.centerX+150, 200, 'ranger')
    this.game.add.text(this.ranger.x+11, this.ranger.centerY+40, 'ranger', charTextStyle)
    this.configureCharacter(this.ranger)

    // Configure Input
    this.cursors = this.game.input.keyboard.createCursorKeys();
  }

  configureCharacter(character) {
    this.game.add.existing(character)
    this.game.physics.arcade.enable(character)

    character.body.facing = Phaser.DOWN

    character.inputEnabled = true

    character.events.onInputDown.add(this.onClick, this)
    character.events.onInputOver.add(this.onMouseOver, this)
    character.events.onInputOut.add(this.onMouseOut, this)
  }

  onClick(sprite) {
    switch(sprite.key) {
      case 'ranger':
        console.log('click: ranger')
        // Player selected a ranger
        break;
      case 'wizard':
        console.log('click: wizard')
        break;
      case 'warrior':
        console.log('click: warrior')
        break;
    }
  }

  onMouseOver(sprite) {
    // Animate selector arrow
    this.game.add.tween(this.selector).to( { alpha: 1 }, 150, Phaser.Easing.Linear.None, true);

    switch(sprite.key) {
      case 'ranger':
        this.selector.x = this.ranger.centerX-14
        this.ranger.body.velocity.y = 0.00001 // HACK - Not sure how to separate state maching from movement
        break;
      case 'wizard':
        this.selector.x = this.wizard.centerX-14
        this.wizard.body.velocity.y = 0.00001
        break;
      case 'warrior':
        this.selector.x = this.warrior.centerX-14
        this.warrior.body.velocity.y = 0.00001
        break;
    }

    this.game.canvas.style.cursor = "pointer";
  }

  onMouseOut(sprite) {
    // Stop animating selector arrow
    this.game.add.tween(this.selector).to( { alpha: 0 }, 150, Phaser.Easing.Linear.None, true);

    // Stop animating player
    switch(sprite.key) {
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
    this.game.canvas.style.cursor = "default";
  }
}
