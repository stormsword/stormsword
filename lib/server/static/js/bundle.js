/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/*!********************************!*\
  !*** ./lib/game/stormsword.js ***!
  \********************************/
/*! dynamic exports provided */
/*! all exports used */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\n\nwindow.onload = function () {\n  var game = new Phaser.Game(1024, 576, Phaser.AUTO, '', { preload: preload, create: create, update: update });\n  console.log(game);\n\n  function preload() {\n    // game.load.image('logo', 'phaser.png');\n  }\n\n  function create() {\n    // var logo = game.add.sprite(game.world.centerX, game.world.centerY, 'logo');\n    // logo.anchor.setTo(0.5, 0.5);\n  }\n\n  function update() {}\n};//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiMC5qcyIsInNvdXJjZXMiOlsid2VicGFjazovLy9saWIvZ2FtZS9zdG9ybXN3b3JkLmpzPzU3NzQiXSwic291cmNlc0NvbnRlbnQiOlsid2luZG93Lm9ubG9hZCA9IGZ1bmN0aW9uKCkge1xuICB2YXIgZ2FtZSA9IG5ldyBQaGFzZXIuR2FtZSgxMDI0LCA1NzYsIFBoYXNlci5BVVRPLCAnJywgeyBwcmVsb2FkOiBwcmVsb2FkLCBjcmVhdGU6IGNyZWF0ZSwgdXBkYXRlOiB1cGRhdGUgfSk7XG4gIGNvbnNvbGUubG9nKGdhbWUpXG5cbiAgZnVuY3Rpb24gcHJlbG9hZCgpIHtcbiAgICAvLyBnYW1lLmxvYWQuaW1hZ2UoJ2xvZ28nLCAncGhhc2VyLnBuZycpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlKCkge1xuICAgIC8vIHZhciBsb2dvID0gZ2FtZS5hZGQuc3ByaXRlKGdhbWUud29ybGQuY2VudGVyWCwgZ2FtZS53b3JsZC5jZW50ZXJZLCAnbG9nbycpO1xuICAgIC8vIGxvZ28uYW5jaG9yLnNldFRvKDAuNSwgMC41KTtcbiAgfVxuXG4gIGZ1bmN0aW9uIHVwZGF0ZSgpIHtcblxuICB9XG5cbn07XG5cblxuXG4vLyBXRUJQQUNLIEZPT1RFUiAvL1xuLy8gbGliL2dhbWUvc3Rvcm1zd29yZC5qcyJdLCJtYXBwaW5ncyI6Ijs7QUFBQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBSUEiLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///0\n");

/***/ })
/******/ ]);