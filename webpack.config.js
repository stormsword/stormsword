var webpack = require('webpack');
var path = require('path');

// Setup our game
GAME_DIR = path.resolve(__dirname, 'lib/game')
GAME_FILE = path.resolve(__dirname, 'lib/game/stormsword.js')

console.log(require.resolve('phaser-ce'))
var config = {
  devtool: 'source-map',
  entry: GAME_FILE,
  output: {
    path: path.resolve('./lib/server/static/js'),
    filename: 'bundle.js'
  },
  module: {
    loaders: [
      {
        test: /\.js?/,
        exclude: /node_modules/,
        loader: 'babel-loader'
      }
    ]
  }
};

module.exports = config;
