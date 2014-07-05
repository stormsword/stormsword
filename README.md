Stormsword
===========

Simple top-down 2d RPG inspired by Zelda and Secret of Mana.

# Latest Release

[Grab the latest release files here](https://github.com/bdickason/stormsword/releases)!

# Building the game (creating binaries)

I've created a simple buildscript that will build osx/windows/linux automagically:
`make version=0.1`

This will build each version into `~/Desktop/StormSword/v0.1`


# Movement

The player will move with WASD keys and can move freely in all four directions.

It is possible (using abilities, for example) for the player to 'leap' or be tossed in the air.



# Camera

The camera looks top-down at the player and art will be rendered with a slight lean to give the illusion of depth (think Zelda).

The Camera will pan in all four directions, following the player but creating bounds when the player hits a wall he/she cannot pass. 

Super Mario World Camera Logic Review: https://www.youtube.com/watch?v=TCIMPYM0AQg


# Animations

We use Unity's "Animator" Component to apply an animation to a character or an object.

## Character Animations

Characters are animated via a sequence of sprites that are mapped to animation keyframes (Similar to Flash). To add an animation, select your object in the Heirarchy and click *Window->Animation*. You should see a set of keyframes here. Clicking on a frame will pull it up in the inspector and allow you to drag a sprite into the Inspector.

To add a new animation frame, click the *Record* button and click on a spot in the timeline where you'd like to add the new sprite. Now drag your new sprite onto the game object's Inspector and you should see a new keyframe pop up.


## Animation Transitions

In order for a character's animation to be triggered, you need to first create a trigger event that transitions from one animation state to another. The default animation state is *Idle*.

Right click on 'Idle' and select 'Create New Transition.' You can select from a series of dropdowns on the Inspector to pick your variable. If your variable is not available, create it under the 'Parameters' menu in the Animator.

Currently Movement code lives within the MoveScript.cs script.

To access a variable from within your code/script, you use the `animator` object:
````
  /* Play movement animation */
  animator.SetBool ("isMoving", isMoving);
  animator.SetFloat("movement_x", movement.x);
  animator.SetFloat("movement_y", movement.y);
````


### Associating a sprite with an action

When dealing with conditional animation such as animating a character's WALK_LEFT vs. WALK_RIGHT, you can use a Blend Tree with a 2D coordinate system. This allows you to trigger a specific animation when, for example, movement_x is 1 and movement_y is 0 (WALK_RIGHT!)

To edit a Blend Tree, first open the Animation for your given character (Double click on it in the Project tab). You will be presented with an Animation State editor screen. Click on the state you'd like to adjust (for example: Walking) and double click the 'Walking Tree' under *Motion* in the Inspector.


# License

Stormsword is distributed under a Creative Commons BY-NC-SA 3.0 License. Feel free to modify, tweak, or rebuild the game. To sell it requires permission.

For more info see LICENSE.md
