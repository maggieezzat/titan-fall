# Game-Engine and Level-Design
## Pilot and Titan Controls
- [ ] The pilot/titan have idle animation.
- [ ] The pilot/titan controls the 1st perspective camera **using the mouse cursor**.
- [ ] The pilot/titan moves **using arrow keys and WASD**.
- [ ] The pilot/titan can sprint by **holding down the left shift**, *if he's crouching, the crouch is undone*.
</br>

- [ ] The pilot can jump **using SPACE**.
- [ ] The pilot can double jump **using double SPACE** (SPACE in mid air).
- [ ] The pilot can wall run **by being mid-air and sprinting forward** (SPACE -> left Shift + FWD arrow).
- [ ] The pilot can crouch **using C**, *however he can't sprint*. Note: **Collider has to shrink**.
- [ ] The pilot can fire **using the left mouse button** *if he has sufficient ammo*. (check ray-cast target with colliders).
- [ ] The pilot can long-fire **by holding the left mouse button** *if he has sufficient ammo and if currentWeapon.firingMode == 0 (ie.automatic)*.
- [ ] The pilot/titan can reload **using R**, *if  ammo is not full and if his current weapon is of type primary weapon*.
- [ ] The pilot can switch weapon (primary <-> heavy) **using Z**.
- [ ] The pilot kills enemies which increases his titanfall meter by **+10** for killing pilots and **+50** for killing titans.
- [ ] The pilot gets hit by enemies and loses HP according to the damage amount of weapon used.
- [ ] The health meter increases automatically by 5HP/sec., *if the pilot doesn't get hit for 3 seconds straight and if his health isn't already full (reached 100)*.
- [ ] The pilot dies when the Health meter reaches zero and "Game Over" is shown.
- [ ] The "call titan" button becomes *iteractable* when:
      * the pilot is not already in the titan.
      * the titan fall meter is full (reached 100).
- [ ] When the "call titan" button is pressed, the button's *interactable* property is set to false, the titan appears next to the pilot and the titan fall meter is reset to 0.
- [ ] The pilot embarks the titan with animation by **pressing E**, *if he is close to the titan*, and then he becomes the titan.
</br>

- [ ] The titan can fire **using the left mouse button**. (check ray-cast target with colliders).
- [ ] The titan can long-fire **by holding the left mouse button** *if currentWeapon.firingMode == 0 (ie.automatic)*. Note: Titan has infinite ammo.
- [ ] The titan can perform a "Dash" by **pressing SPACE and a movement key**, *if he Dash meter is not empty*.
- [ ] The titan loses a Dash point from the dash meter having performed a Dash.
- [ ] The Dash meter increases by 1Dash/sec. *if it is not full, and if it hasn't reached the maximum (3 Dashes)*.
- [ ] The titan cannot get hit during a Dash (invincible).
- [ ] The titan can activate defensive ability by **pressing F**, *if not during the cooldown period* (check next point).
- [ ] When the defensive ability is activated, a cooldown period of 15 seconds starts during which the titan can't activate his defensive ability again.
- [ ] The titan can activate the core ability by **pressing V**, *if the core ability meter reached 100 and it is not already active*, and then the core ability meter would be reset to zero. Note: Core ability meter decreases/does-not-change based on the type of titan.
- [ ] The titan's core ability meter increases by killing enemies, **+10** for killing pilots, **+50** for killing titans, *if the core ability is not active*.
- [ ] The titan can disembark **by pressing E**, and then become a pilot.
- [ ] The titan loses health points  when he gets hit, based on the damage amount of the weapon that attacked him. Note: Titan's health never increases.
- [ ] When the titan's health meter reaches zero, the titan is destroyed and the player is automatically disembarked. The player becomes a pilot again with his previous health, ammo, selected weapon..etc.

## Level Design

There are two levels: combat level and parkour level.

### Combat Level

- [ ] The combat level has 10 enemy pilots (scattered in the environment).
- [ ] Half of the enemy pilots use one kind of primary weapon and the other half uses the other type of primary weapon. Note: The person that will be doing the AI and enemies will create two prefabs of these enemies and your task will only be to specify their positions.
- [ ] The level has 5 enemy titans of one type (scattered in the environment).
- [ ] There should be a path visible to the player (walkable area/arrows) that lead him through the enemies to a trigger area.
- [ ] The trigger area should be inactive until the player kills all enemies, then he is able to go to the next level.

### Parkour Level

- [ ] In the parkour level the player is a pilot, and there are no enemies and no weapon.
- [ ] In the parkour level the player **must** perform all the following actions in order to pass through obstacles and get to a goal area: **wall run, jump, double jump, and crouch**.
- [ ] The environment must include an endless pit underneath, if the player falls into it then he dies.
- [ ] There should be a path visible to the player (walkable area/arrows) that lead him through the obstacles to the goal area.
- [ ] When reaching the goal area the player wins and the credits roll.

## Summary of meters

- [ ] Pilot Health bar:
  * **Initially:** 100 HP.
  * **Maximum:** 100 HP.
  * **decreases** by the damage amount of the weapon used to hit the player.
  * **increases** automatically 5HP/sec if the player does not get hit for 3 seconds straight.
- [ ] Titan Health bar:
  * **Initially:** 400 HP.
  * **Maximum:** 400 HP.
  * **decreases:** by the damage amount of the weapon used to hit the player.
  * **does not increase.**
- [ ] Titan fall meter (pilot):
  * **Initially :** 0.
  * **Maximum:** 100 HP.
  * **is reset to 0** when the "call titan button is pressed".
  * **increases** by killing enemies +10 for killing pilots, +50 for killing titans.
- [ ] Dash meter (titan):
  * **Initially:** 3.
  * **Maximum:** 3.
  * **decreases** when the titan uses a "Dash".
  * **Increases** automatically.
- [ ] Defensive ability cool down (titan):
  * 15 seconds cool down after the titan activates his defensive ability.
- [ ] Core ability meter (Titan):
  * **Initially:** 0.
  * **Maximum:** 100.
  * **reset to 0:**  when the core ability is activated.
  * **increases** by killing enemies, +10 for killing pilots, and +50 for killing enemies.
- [ ] Ammunition (pilot - primary weapon):
  * only appears when the player's current weapon is primary weapon, disappears when the player switches to heavy weapon.
  * **Maximum:** ammo count of the specific primary weapon.
  * **decreases** every second by the fire rate of the specific primary weapon, when the player fires.
  * **is refilled to the maximum** when the player reloads by pressing R.

# AI and Enemies

- [ ] Bake enemy's walkable areas in the environment.

## Enemy Pilot

- [ ] There are two type of enemy pilots with two different types of primary weapons.
- [ ] Stands idle with animation.
- [ ] Walks in a pattern using patrols with animations.
- [ ] Switches between standing idle and walking in a pattern when the player is not in his attack range.
- [ ] When the player is in his attack range, the enemy should **sprint towards** the player with animation and fire at him **every 3 seconds** with animation, *so long as he is in his attack range*.
- [ ] The enemy should have a floating HUD with his health bar (world space canvas with progress bar), which is **initially 100 HP** and can **only decrease** by the damage amount of the specific weapon used to attack him with getting hit animation. When the HP is zero the enemy dies with animation and is **disabled**. Note: don't destroy.

## Enemy Titan

- [ ] There is only one type of titan with a certain type of primary weapon.
- [ ] He can only fire with his primary weapon (ie. he can't dash,use core ability..etc).
- [ ] Stands idle with animation.
- [ ] Walks in a pattern using patrols.
- [ ] Switches between standing idle and walking in a pattern when the player is not in his attack range.
- [ ] When the player is in his attack range, the enemy should **sprint towards** the player with animation and fire at him **every 3 seconds** with animation, *so long as he is in his attack range*.
- [ ] The enemy should have a floating HUD with his health bar (world space canvas with progress bar), which is **initially 400 HP** and can **only decrease** by the damage amount of the specific weapon used to attack him with getting hit animation, *however he can only be attacked by a "heavy weapon" or a "titan weapon"*. When the HP is zero the enemy dies with animation and is **disabled**. Note: don't destroy.

## Summary Animations

- [ ] Idle
- [ ] Walking
- [ ] Sprint/Run
- [ ] Firing Weapon
- [ ] Hit Reaction
- [ ] Dying

# Weapons and Titans.

## Weapons

### Weapon Class:

#### attributes:

* Damage amount: int.

### methods:

-

### Primary Weapon sub-Class:

#### attributes:

* Firing mode: 0 (automatic) / 1 (single shot).
* Fire rate: int.
* Ammo count: int.
* Range: int.

#### methods:

-

### Heavy Weapon sub-Class:

#### attributes:

* Projectile: 0 (straight line)/1 (curve downwards)
* explosion radius: int

#### methods:

-

### Titan Weapon sub-Class:

//depends on the titan chosen.

</br>

## Titans

### Defensive Ability

-

### Core Ability

-








