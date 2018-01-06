var webpack = require('webpack');
var path = require('path');

// Setup our game
GAME_FILE = path.resolve(__dirname, 'lib/client/game/stormsword.js')

// Setup tools
TOOLS_FILE = path.resolve(__dirname, 'lib/client/tools/tools.js')

var config = {
  devtool: 'source-map',
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
        exclude: /node_modules/,
        loader: 'babel-loader'
      },
      {
        test: /\.json/,
        exclude: /node_modules/,
        loader: 'json-loader'
      }
    ]
  }
};

module.exports = config;
