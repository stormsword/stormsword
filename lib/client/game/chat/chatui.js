/* chatui.js - Display chat UI on screen */

export class ChatUI {
  constructor(game, group) {
    this.game = game
    this.group = group

    this.message = ""

    // Display chat thang
    // Display box
    this.box = this.game.add.graphics(0, 0)
    this.box.beginFill('black', 0.5)
    this.box.drawRect(15, 500, 500, 20)

    // Display chat text
    let style = { font: "11px Georgia", wordWrap: true, wordWrapWidth: this.box.width, align: "left", fill: "white" };
    this.text = this.game.add.text(18, 503, 'Chat: ', style)

    // Display cursor
    this.cursor = this.game.add.text(18+this.text.width, 503, '|', style)
    this.blinkTimer = setInterval(() => {
      // Cursor should blink like a text editor
      this.cursor.visible = !this.cursor.visible
    }, 500)
  }

  updateMessage(message) {
    // whenever player types something, we should update the ui
    this.message = message
    this.text.setText('Chat: ' + this.message)
    this.cursor.x = 18+this.text.width
  }

  destroy() {
    this.box.destroy()
    this.text.destroy()
    this.cursor.destroy()
    clearInterval(this.blinkTimer)
  }
}
