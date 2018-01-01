var webpack = require('webpack');
var path = require('path');

GAME_DIR = path.resolve(__dirname, 'lib/game')
GAME_FILE = path.resolve(__dirname, 'lib/game/stormsword.js')

var config = {
  devtool: 'source-map',
  entry: GAME_FILE,
  output: {
    path: path.resolve('./lib/server/static/js'),
    filename: 'bundle.js'
  }
};

module.exports = config;
