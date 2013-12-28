//
//  IntroScene.m
//  stormSword
//
//  Created by Bradley Dickason on 12/28/13.
//  Copyright Bradley Dickason 2013. All rights reserved.
//
// -----------------------------------------------------------------------

// Import the interfaces
#import "IntroScene.h"
#import "HelloWorldScene.h"

// -----------------------------------------------------------------------
#pragma mark - IntroScene
// -----------------------------------------------------------------------

NSString *preview =
@"Preview Text Line 1\n\n Preview Text Line 2";

// -----------------------------------------------------------------------

@implementation IntroScene

// -----------------------------------------------------------------------

+ (IntroScene *)scene
{
	return [[self alloc] init];
}

// -----------------------------------------------------------------------

- (id)init
{
    // Apple recommend assigning self with supers return value
    self = [super init];
    // Crash if basic initialization for some reason failed
    NSAssert(self, @"Unable to create class IntroScene");
    
    // Here is where custom code for the scene starts
    
    // create a colored background
    CCNodeColor *background = [CCNodeColor nodeWithColor:[CCColor colorWithCcColor4b:(ccColor4B){200, 200, 200, 255}]];
    [self addChild:background];
    
    // create a string with preview text
    NSMutableAttributedString *string = [[NSMutableAttributedString alloc] initWithString:preview];
    
    // set color and font of the string
    [string addAttribute:NSForegroundColorAttributeName
                   value:[UIColor blackColor]
                   range:NSMakeRange(0, string.length)];
    [string addAttribute:NSFontAttributeName
                   value:[UIFont fontWithName:@"Arial" size:22.0f]
                   range:NSMakeRange(0, string.length)];
    
    // create an attributes label, and place it in upper left corner
    CCLabelTTF *label = [CCLabelTTF labelWithAttributedString:string];
    label.positionType = CCPositionTypeNormalized;
    label.position = ccp(0.4f, 0.8f);
    [self addChild:label];
    
    // add a next button
    CCButton *nextButton = [CCButton buttonWithTitle:@"[ Next ]"];
    nextButton.positionType = CCPositionTypeNormalized;
    nextButton.position = ccp(0.9f, 0.9f);
    [nextButton setTarget:self selector:@selector(onNextClicked:)];
    [self addChild:nextButton];
	
    // done
	return self;
}

// -----------------------------------------------------------------------
#pragma mark - Button Callbacks
// -----------------------------------------------------------------------

- (void)onNextClicked:(id)sender
{
    // start main scene with transition
    [[CCDirector sharedDirector] replaceScene:[HelloWorldScene scene]
                               withTransition:[CCTransition transitionPushWithDirection:CCTransitionDirectionLeft duration:1.0f]];
}

// -----------------------------------------------------------------------
@end