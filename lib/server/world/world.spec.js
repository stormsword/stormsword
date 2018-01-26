/* Tests for world.js */
const mocha = require('mocha')
const assert = require('chai').assert

const World = require('./world.js')

describe('world', () => {
  describe('constructor', () => {
    it('Begins with an empty state', () => {
      const _state = {
        players: []
      }

      let world = new World()
      assert.deepEqual(_state, world.state)
    })
  })
  describe('spawn', () => {
    describe('player', () => {
      it('With ID: Spawns a new player', () => {
        let type = 'Player'
        let metadata = {id: 5}

        const _state = {
          players: [{id: 5}]
        }

        let world = new World()

        assert.doesNotThrow(() => {
          world.spawn(type, metadata)
          assert.deepEqual(_state, world.state)
        })
      })
      it('Bad ID call: Throws error', () => {
        let type = 'Player'
        let metadata = 5

        const _state = {
          players: []
        }

        let world = new World()

        assert.throws(() => {
          world.spawn(type, metadata)
          assert.deepEqual(_state, world.state)
        })
      })
      it('Without ID: Throws error', () => {
        let type = 'Player'
        let metadata = null

        const _state = {
          players: []
        }

        let world = new World()

        assert.throws(() => {
          world.spawn(type, metadata)
          assert.deepEqual(_state, world.state)
        })
      })
    })
  })
  describe('despawn', () => {
    describe('player', () => {
      it('With ID: Despawns a player', () => {
        let type = 'Player'
        let metadata = {id: 5}

        const _state = {
          players: []
        }

        let world = new World()

        assert.doesNotThrow(() => {
          world.spawn(type, metadata)
          world.despawn(type, metadata)
          assert.deepEqual(_state, world.state)
        })
      })
      it('Bad ID call: Throws error', () => {
        let type = 'Player'
        let metadata = {id: 5}
        let badmetadata = 5


        const _state = {
          players: [{id: 5}]
        }

        let world = new World()

        assert.throws(() => {
          world.spawn(type, metadata)
          world.despawn(type, badmetadata)
          assert.deepEqual(_state, world.state)
        })
      })
      it('Without ID: Throws error', () => {
        let type = 'Player'
        let metadata = {id: 5}
        let nometadata = null

        const _state = {
          players: []
        }

        let world = new World()

        assert.throws(() => {
          world.spawn(type, metadata)
          world.despawn(type, nometadata)
          assert.deepEqual(_state, world.state)
        })
      })
    })
  })
})
