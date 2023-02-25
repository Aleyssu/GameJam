# Companion
QGDC Game Jam 2023

Companion Mechanics

Asset credits:
https://opengameart.org/content/12-player-movement-sfx


Display:
Side-view
Pixel graphics

Mechanics:
	Combat:
Just one enemy type for now - skeletons
You are weak, small and agile
maybe some sort of range attack only? (for the player)
Your companion is strong, big and slow
hit point system for the player
companion attack when in attack range of an enemy. IEnumerator function to delay attack by 1.5f. StartCoroutine(DelayAttack(1.5f))

Movement:
Direct your companion
Move and Jump
Leap off your companion
Bounce off of walls
Crawl through small gaps (or rather walk through small gaps)

	VFX:
Light dust cloud effect when jumping
Screen shake when your companion lands from a large height
	
	SFX:
-Light player landing and jumping sounds
-Heavy landing sounds for companion - big crashing rumble for big landing
-
	
	Theme
dungeon (door, wall, button)
forest
underground
	

	Level Design:
Buttons and doors

Commands:
Hunt (Detect and go to any detected enemies)
Stay (stay at your spot at time of command)
Follow me (follows you) (default command)

moving enemy ai states:
patrol -> player/companion on same platform then go to chasing
attacked/influenced -> goes to chasing mode
chasing -> once within attack range, attack

