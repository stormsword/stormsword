/* chat.js - Allows users to talk to each other */

import {ChatUI} from './chatui.js'

export class Chat {
  constructor(game, input, socket, group) {
    this.game = game
    this.socket = socket
    this.chatting = false
    this.group = group

    // Handle Input
    this.input = input
    this.input.enter.add(() => {
      if(!this.chatting) {
        // Show typing indicator
        this.socket.emit('chat:start')
        this.message = ""

        this.ui = new ChatUI(this.game, this.group)

        this.input.chatMode((char) => {
          // Called when a key is pressed
          this.message += char  // Append to chat
          this.ui.updateMessage(this.message)
        })
      } else {
        if(this.message != "") {
          // Send message to server
          this.socket.emit('chat:send', this.message)
        } else {
          // Cancel chatting
          this.socket.emit('chat:cancel')
        }
        this.message = ""
        this.input.moveMode()
        this.ui.destroy()
      }
    }, this)

    // handle escape keypress (cancel chat)
    this.input.escape.add(() => {
      if(this.chatting) {
        this.socket.emit('chat:cancel')
        this.input.moveMode()
        this.ui.destroy()
      }
    }, this)

    // handle backspace keypress when typing
    this.input.backspace.add(() => {
      if(this.chatting) {
        if(this.message.length > 0) {
          this.message = this.message.slice(0, -1)  // Remove the last character from the message
          this.ui.updateMessage(this.message)
        }
      }
    })

    // handle spacebar keypress when typing
    this.input.spacebar.add(() => {
      if(this.chatting) {
        this.message += " "
        this.ui.updateMessage(this.message)
      }
    })
  }


}
