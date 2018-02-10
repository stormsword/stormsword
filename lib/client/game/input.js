export class Input {
  constructor(game) {
    this.game = game

    this.MODES = {
      move: 0,
      keyboard: 1
    }

    this.mode = this.MODES.move

    this.cursors = this.game.input.keyboard.createCursorKeys();

    this.keys = {}  // Object to hold our keys

    /* Enter - Used to start chat or send a message */
    this.enter = new Phaser.Signal()
    this.keys.ENTER = this.game.input.keyboard.addKey(Phaser.KeyCode.ENTER)
    this.keys.ENTER.onDown.add(() => {
      this.enter.dispatch()
    })

    /* Escape - Used to Cancel chat */
    this.escape = new Phaser.Signal()
    this.keys.ESCAPE = this.game.input.keyboard.addKey(Phaser.KeyCode.ESC)
    this.keys.ESCAPE.onDown.add(() => {
      this.escape.dispatch()
    })

    /* Backspace - Only used in chat mode */
    this.backspace = new Phaser.Signal()
    this.keys.BACKSPACE = this.game.input.keyboard.addKey(Phaser.KeyCode.BACKSPACE)
    this.keys.BACKSPACE.onDown.add(() => {
      this.backspace.dispatch()
    })

    /* Spacebar - Used in chat mode */
    this.spacebar = new Phaser.Signal()
    this.keys.SPACEBAR = this.game.input.keyboard.addKey(Phaser.KeyCode.SPACEBAR)
    this.keys.SPACEBAR.onDown.add(() => {
      this.spacebar.dispatch()
    })
  }

  /* Basic movement functions */
  moveUp() {
    return(this.cursors.up.isDown)
  }

  moveDown() {
    return(this.cursors.down.isDown)
  }

  moveRight() {
    return(this.cursors.right.isDown)
  }

  moveLeft() {
    return(this.cursors.left.isDown)
  }

  chatMode(callback) {
    this.mode = this.MODES.keyboard

    this.game.input.keyboard.addCallbacks(this, null, null, (char) => {
      this.keyPress(char, callback)
    });
  }

  moveMode() {
    this.mode = this.MODES.move

    let keyboard = this.game.input.keyboard
    keyboard.onDownCallback = null
    keyboard.onUpCallback = null
    keyboard.onPressCallback = null
  }

  keyPress(char, callback) {
    callback(char)
  }
}
