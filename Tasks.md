# Game-Engine and Level-Design
## Pilot and Titan Controls
- [ ] The pilot/titan have idle animation.
- [ ] The pilot/titan controls the 1st perspective camera **using the mouse cursor**.
- [ ] The pilot/titan moves **using arrow keys and WASD** (preferably by the animator).
- [ ] The pilot/titan can sprint by **holding down the left shift**, *if he's crouching, the crouch is undone*.
- [ ] The pilot/titan can fire **using the left mouse button** *if he has sufficient ammo*. (check ray-cast target with colliders).
- [ ] The pilot/titan can long-fire **by holding the left mouse button** *if he has sufficient ammo and if currentWeapon.firingMode == 0 (ie.automatic)*.
- [ ] The pilot/titan can reload **using R**, *if  ammo is not full*.

- [ ] The pilot can jump **using SPACE**.
- [ ] The pilot can double jump **using double SPACE** (SPACE in mid air).
- [ ] The pilot can wall run **by being mid-air and sprinting forward** (SPACE -> left Shift + FWD arrow).
- [ ] The pilot can crouch with animation **using C**, *however he can't sprint*.
- [ ] The pilot can switch weapon (primary <-> heavy) **using Z**.
- [ ] The pilot kills enemies which increases his titanfall meter by **+10** for killing pilots and **+50** for killing titans.
- [ ] The pilot gets hit by enemies and loses HP according to the weapon used.
- [ ] The health meter increases automatically by 5HP/sec., *if the pilot doesn't get hit for 3 seconds straight and if his health isn't already full (reached 100)*.
- [ ] The pilot dies when the Health meter reaches zero and "Game Over" is shown.
- [ ] The "call titan" button becomes *iteractable* when:
      * the pilot is not already in the titan.
      * the titan fall meter is full (reached 100).
- [ ] When the "call titan" button is pressed, the button's *interactable* property is set to false, the titan appears next to the pilot and the titan fall meter is reset to 0.
- [ ] The pilot embarks the titan with animation by **pressing E**, *if he is close to the titan*, and then he becomes the titan.

- [ ] The titan can perform a "Dash" by **pressing SPACE and a movement key**, *if he Dash meter is not empty*.
- [ ] The titan loses a Dash point from the dash meter having performed a Dash.
- [ ] The Dash meter increases by 1Dash/sec. *if it is not full, and if it hasn't reached the maximum (3 Dashes)*.
- [ ] The titan cannot get hit during a Dash (invincible).
- [ ] The titan
