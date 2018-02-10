/* chat.js - Allows users to talk to each other */

export class Chat {
  constructor(input, socket) {
    this.input = input
    this.socket = socket
    this.chatting = false

    this.input.enter.add(() => {
      if(!this.chatting) {
        this.socket.emit('chat:start')
        this.message = ""

        this.input.chatMode((char) => {
          // Called when a key is pressed
          this.message += char  // Append to chat
        })
      } else {
        this.socket.emit('chat:send', this.message)
        this.message = ""
        this.input.moveMode()
      }
    }, this)

    this.input.escape.add(() => {
      if(this.chatting) {
        this.socket.emit('chat:cancel')
        this.input.moveMode()
      }
    }, this)

    this.input.backspace.add(() => {
      if(this.chatting) {
        if(this.message.length > 0) {
          this.message = this.message.slice(0, -1)  // Remove the last character from the message
        }
      }
    })

    this.input.spacebar.add(() => {
      if(this.chatting) {
        this.message += " "
      }
    })

    /* Chat Networking */
    this.socket.on('message', (player, message) => {
      console.log(player)
      console.log(message)
    })
  }
}
