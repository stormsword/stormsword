var webpack = require('webpack');
var path = require('path');

// Setup our game
GAME_FILE = path.resolve(__dirname, 'lib/client/game/stormsword.js')

// Setup tools
TOOLS_FILE = path.resolve(__dirname, 'lib/client/tools/tools.js')

var config = {
  entry: {
    'lib/server/static/js/bundle-game.js': GAME_FILE,
    'lib/server/static/js/bundle-tools.js': TOOLS_FILE
  },
  output: {
    path: path.resolve('./'),
    filename: '[name]'
  },
  module: {
    loaders: [
      {
        test: /\.js?/,
        include: path.resolve(__dirname, '/lib/client/'),
        loader: 'babel-loader'
      }
    ]
  }
};

module.exports = config;
