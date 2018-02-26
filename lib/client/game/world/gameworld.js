/* gameWorld.js - Client side world creation and state management */

import {Character} from '../character/character.js'
import {Map} from './map.js'

export class GameWorld {
  constructor(game) {
    this.players = []

    this.game = game

    // Initialize World
    this.game.world.setBounds(0, 0, 2000, 2000);  // Create a large game surface so we can move the camera
    this.game.physics.startSystem(Phaser.Physics.ARCADE);
    this.game.stage.backgroundColor = '#2d2d2d';

    this.game.camera.follow(this.player, Phaser.Camera.FOLLOW_LOCKON, 0.1, 0.1);  // Attach camera to player

    this.map = new Map(this)

    this.groups = {
      players: this.game.add.group(),
      map: this.game.add.group()
    }

    this.game.world.bringToTop(this.groups.players)
  }

  loadMap(tiles, csv) {
    this.map.load(tiles, csv, this.groups.map)
  }

  updatePlayer(id, data) {
    this.players[id].x = data.x
    this.players[id].y = data.y
    this.players[id].body.velocity.x = data.body.velocity.x
    this.players[id].body.velocity.y = data.body.velocity.y
    this.players[id].body.facing = data.body.facing
    this.players[id].chatting = data.chatting
  }

  spawn(entity) {
    let player = new Character(entity.id, this.game, 350, 300, entity.character)

    // Add to game engine and enable physics
    console.log('adding: ' + player.name)

    this.game.add.existing(player)
    this.game.physics.arcade.enable(player)

    // Add to array of players
    this.players.push(player)
    this.groups.players.add(player)
  }

  despawn(id) {
    let index = this.findPlayer(id)

    if (index !== -1) {
      this.players[index].kill()
      this.players.splice(index, 1)
    }
  }

  showMessage(id, message) {
    let index = this.findPlayer(id)

    if (index !== -1) {
      this.players[index].showChatBubble(message)
    }

  }

  findPlayer(id) {
    let index

    for(let i=0; i < this.players.length; i++) {
      if(this.players[i].id == id) {
        index = i
      }
    }

    return(index)
  }

}
