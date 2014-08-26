0.0.5 / 2014-08-26
==================
  * update: New sprite for player!!!
  * add: Created 'Stomp' ability
    * activated by pressing W
    * Pushes player forward a short distance at a high speed
    * Knocks enemies in the path out of the way
  * add: Camera now follows player smoothly
  * update: Expanded basic zone with walls, exit
  * update: Make inspector more usable for designers
  * add: Pause Menu
    * Open by pressing `Esc`
    * User can quit the game :)
  * add: Main Menu
    * Uses temporary sprites/animation
    * Allows user to start a game and loads Forest Zone by default
  * add: Mixpanel tracking so we can see how many people are playing
    * Players can disable tracking from the Pause menu
  * update: Player's weapon now animates more smoothly and adjusts for the direction he is facing
  * add: Da Boss Man (enemy)
    * Currently stuck in the middle of the map, will soon be moved to his own room
  * bugfix: DoT and Snare no longer crash the game when the player died

0.0.4 / 2014-07-31
==================
 
 * update: Added two AI types
   * 'Stalker' - returns to spawnpoint when you move ~3 squares away
   * 'Wanderer' - roams in place when you move ~3 squares away
 * add: Abilities have a small UI icon that fades out when on cooldown
 * update: Goblin now animates properly when walking/attacking
 * add: Pigmy (enemy) 
 * add: Created Effects that can be applied by weapons or abilities
   * DoT effect - applies damage over time (currently used by Pigmy enemy)
   * Snare effect - reduces move speed by a % (currently used on Stomp)
 * add: Created 'Stomp' ability
   * activated by pressing Q
   * damages all enemies in an area
   * displays a short animation
 * bugfix: Player can no longer walk through walls

0.0.3 / 2014-07-26
==================

  * add: hp bar now displays current HP as character takes damage
  * add: Brute (enemy)
  * add: Hobgoblin (enemy)
  * add: Player now attacks in all directions
  * add: Maps are now designed in Tiled
  * add: Player character is loaded in via Tiled
  * add: All enemies are now loaded in via Tiled

0.0.2 / 2014-07-14
==================

  * add: hp bubble flies up when an enemy takes damage
  * add: enemy is knocked back slightly when they take damage
  * update: Ranged attacks now fire in direction character is facing
  * add: Fireballs animate as they fly across the screen
  * add: Tilemaps allow placement of basic terrain


0.0.1 / 2014-07-05
==================

  * Initial prototype - A single player can run around a fixed-width map and swing a hammer.
  * add: Melee attacks
  * add: Healthbar placeholder
  * add: Player
