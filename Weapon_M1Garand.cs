//sounds
datablock AudioProfile(garandRifleFireSound) {
	filename    = "./Sounds/Fire.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(garandRifleBoltSound)
{
	filename    = "./Sounds/bolt.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(garandRifleFireLastSound)
{
	filename    = "./Sounds/FireLast.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(garandRifleClipInSound)
{
	filename    = "./Sounds/clipin.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(garandRifleClipOutSound)
{
	filename    = "./Sounds/clipout.wav";
	description = AudioClose3d;
	preload = true;
};

AddDamageType("GarandRifle",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_m1garand> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_m1garand> %1',0.2,1);
AddDamageType("GarandRifleHeadshot",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_m1garand><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_m1garand><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',0.2,1);
//muzzle flash effects
datablock ProjectileData(garandRifleProjectile : gunProjectile) {
	directDamageType	= $DamageType::GarandRifle;
	radiusDamageType	= $DamageType::GarandRifle;
	headshotDamageType	= $DamageType::GarandRifleHeadshot;
	directDamage        = 30;//8;

	impactImpulse       = 100;
	verticalImpulse     = 50;

	muzzleVelocity      = 200;
	velInheritFactor    = 0.5;

	armingDelay         = 00;
	lifetime            = 4000;
	fadeDelay           = 3500;
	bounceElasticity    = 0.5;
	bounceFriction      = 0.20;
	isBallistic         = false;
	gravityMod = 0.0;

	particleEmitter     = advBigBulletTrailEmitter; //bulletTrailEmitter;
	headshotMultiplier = 3;
};

//////////
// item //
//////////

datablock ItemData(garandRifleItem) {
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/weapons/m1garand.dts";
	iconName = "./shapes/weapons/icons/m1garand";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "M1 Garand";
	//iconName = "./icons/icon_Pistol";
	doColorShift = false;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	magItem = garandRifleMagazineItem;
	reload = true;

	maxmag = 20;
	ammotype = "30-06";
	nochamber = 1;

	 // Dynamic properties defined by the scripts
	image = garandRifleImage;
	canDrop = true;

	clickPickUp = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(garandRifleImage) {
	// Basic Item properties
	shapeFile = "./shapes/weapons/m1garand.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0; //"0.7 1.2 -0.5";
	rotation = eulerToMatrix( "0 0 0" );

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = garandRifleItem;
	ammo = " ";
	projectile = garandRifleProjectile;
	projectileType = Projectile;
	customProjectileFire = true;

	casing = gunShellDebris;
	shellExitDir        = "1.0 -1.3 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 15.0;
	shellVelocity       = 7.0;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	doColorShift = false;
	colorShiftColor = garandRifleItem.colorShiftColor;//"0.400 0.196 0 1.000";

	//casing = " ";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state

	stateName[0]						= "Activate";
	stateTimeoutValue[0]				= 0.2;
	stateTransitionOnTimeout[0]			= "CheckChamber";
	stateSequence[0]					= "activate";

	stateName[1]						= "CheckChamber"; //This state makes sure there's a bullet chambered.
	stateTransitionOnTimeout[1]			= "Ready";
	stateAllowImageChange[1]			= true;
	stateScript[1]						= "onCheckChamber";

	stateName[2]						= "Ready"; //This state allows you to fire your gun.
	stateTransitionOnTriggerDown[2]		= "Fire";
	stateTransitionOnNotLoaded[2]		= "Empty"; //In case we decide to empty the gun or something.
	stateTransitionOnNoAmmo[2]			= "PullBackSlide"; //onAmmo and onNoAmmo are used for ejecting shell.
	stateSequence[2]					= "root";
	stateAllowImageChange[2]			= true;
	stateScript[2]						= "onReady";

	stateName[3]						= "Empty";
	stateTransitionOnTriggerDown[3]		= "EmptyClick"; //State 8.
	stateTranstionOnLoaded[3]			= "CheckChamber";
	stateTransitionOnNoAmmo[3]			= "PullBackSlide";
	stateScript[3]						= "onEmpty";

	stateName[4]						= "Fire";
	stateTimeoutValue[4]				= 0.15;
	stateTransitionOnTimeout[4]			= "Smoke";
	stateFire[4]						= true;
	stateAllowImageChange[4]			= false;
	stateSequence[4]					= "Fire";
	stateScript[4]						= "onFire";
	stateWaitForTimeout[4]				= true;
	stateEmitter[4]						= advBigBulletFireEmitter;
	stateEmitterTime[4]					= 0.05;
	stateEmitterNode[4]					= "muzzleNode";

	stateName[5]						= "Smoke"; //Acts as a fire delay
	stateEmitter[5]						= advBigBulletSmokeEmitter;
	stateEmitterTime[5]					= 0.05;
	stateEmitterNode[5]					= "muzzleNode";
	stateTimeoutValue[5]				= 0.3;
	stateWaitForTimeout[5]				= true;
	stateTransitionOnTimeout[5]			= "CheckTrigger"; //Alright, delay passed, let's check if the dude released the trigger yet

	stateName[6]						= "CheckTrigger";
	stateTransitionOnTriggerUp[6]		= "CheckChamber";

	stateName[7]						= "EmptyClick"; //*click!*
	stateTransitionOnTriggerUp[7]		= "checkChamber";
//	stateSequence[7]					= "";
	stateTimeoutValue[7]				= 0.13;
	stateAllowImageChange[7]			= false;
	stateWaitForTimeout[7]				= true;
	stateSound[7]						= "none";
	stateScript[7]						= "onEmptyFire";

	stateName[8]						= "PullBackSlide";
	stateScript[8]						= "onReload";
	stateSequence[8]					= "eject";
	stateTransitionOnTimeout[8]			= "CheckTrigger";
	stateSound[8]						= garandRifleBoltSound;
	stateTimeoutValue[8]				= 0.2;
	stateWaitForTimeout[8]				= true;
};

datablock ItemData(garandRifleMagazineItem) {
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./shapes/items/m1garand_clip.dts";
	iconName = "./shapes/weapons/icons/m1garand_clip";
	uiName = "30-06 Magazine";
	doColorShift = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	maxmag = 20;
	ammotype = "30-06";
	isMag = true;

	image = garandRifleMagazineImage;
	canDrop = true;
	canPickup = true;
};

datablock ShapeBaseImageData(garandRifleMagazineImage) {
	// Basic Item properties
	shapeFile = "./shapes/items/m1garand_clip.dts";
	emap = true;
	mountPoint = 0;
	className = "WeaponImage";
	armReady = true;
	doColorShift = false;

	isMag = true;
	ammoType = "30-06";
	maxClip = 8;
	ammoItem = Bullet30Item;
	item = garandRifleMagazineItem;
};

function garandRifleMagazineItem::onAdd(%this, %obj) {
	parent::onAdd(%this, %obj);
	if ( %obj.mag $= "" ) {
		%obj.mag = %this.image.maxClip;
	}
	if ( %obj.mag <= 0 ) {
		%obj.hideNode("bullets");
	}
}

function garandRifleMagazineImage::onMount(%this, %obj, %slot) {
	if ( %obj.mag[%obj.currTool] $= "" ) {
		%obj.mag[%obj.currTool] = %this.maxClip;
	}
	parent::onMount(%this,%obj,%slot);
	garandRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
}

  ////// ammo display functions
function garandRifleImage::onMount(%this, %obj, %slot) {
	if ( %obj.hasMag[%obj.currTool] $= "" )
		%obj.hasMag[%obj.currTool] = false;

	parent::onMount(%this,%obj,%slot);
}

function garandRifleImage::onUnMount(%this, %obj, %slot) {
	parent::onUnMount(%this,%obj,%slot);
}

function garandRifleImage::onCheckChamber(%this, %obj, %slot) {
	if ( !%obj.garandLoaded ) {
		%obj.setImageLoaded(0, 0);
	}
	else {
		%obj.setImageLoaded(0, 1);
	}
}

function garandRifleImage::onEmptyFire(%this, %obj, %slot) {
	if ( %obj.currgarandMagazine > 0 ) { //if there's any bullets to cycle through...
		%obj.setImageAmmo(0, 0); //Force a cycle if we try to fire before cycling manually
		return;
	}
	serverPlay3d(advReloadTap1Sound,%obj.getHackPosition());
}

function garandRifleImage::onReload(%this, %obj, %slot) {
	if ( %obj.garandLoaded ) {
		%obj.garandLoaded = false;

		%datablock = Bullet30Item;

		%item = new Item() {
			dataBlock = %datablock;
			position = vectorAdd(%obj.getMuzzlePoint(0), vectorScale(%obj.getMuzzleVector(0), -2));
		};

		%spread = 15;
		%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
		%spread = vectorScale(%scalars, mDegToRad(%spread / 2));

		%vector = vectorAdd(vectorScale(%obj.getEyeVector(), -2), "0 0 5");
		%matrix = matrixCreateFromEuler(%spread);
		%vel = matrixMulVector(%matrix, %vector);
		%item.setVelocity(%vel);
		%position = getWords(%item.getTransform(), 0, 2);
		%item.setTransform(%position SPC eulerToAxis("0 0" SPC getRandom() * 360 - 180));

		if (!isObject(BulletGroup)) {
			MissionCleanup.add(new SimGroup(BulletGroup));
		}

		BulletGroup.add(%item);

		%item.schedule(14000, fadeOut);
		%item.schedule(15000, delete);
	}

	if ( %obj.currgarandMagazine > 0 ) {
		%obj.currgarandMagazine--;
		%obj.garandLoaded = true;
	}

	%obj.setImageAmmo(0, 1);
}

function garandRifleImage::getProjectileSpread(%this, %obj, %slot) {
	%spread = 0;
	return %spread; // + vectorLen(%obj.getVelocity()) * 0.3;
}

function garandRifleImage::onFire(%this, %obj, %slot) {
	if ( %obj.getDamagePercent() > 1.0 ) {
		return;
	}

	parent::onFire(%this, %obj, %slot);

	%obj.playThread(2, shiftAway);

	//Eject the shell after every shot
	%datablock = Shell30Item;

	%item = new Item() {
		dataBlock = %datablock;
		position = vectorAdd(%obj.getMuzzlePoint(0), vectorScale(%obj.getMuzzleVector(0), -2));
	};

	%spread = 15;
	%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
	%spread = vectorScale(%scalars, mDegToRad(%spread / 2));

	%vector = vectorAdd(vectorScale(%obj.getEyeVector(), -2), "0 0 5");
	%matrix = matrixCreateFromEuler(%spread);
	%vel = matrixMulVector(%matrix, %vector);
	%item.setVelocity(%vel);
	%position = getWords(%item.getTransform(), 0, 2);
	%item.setTransform(%position SPC eulerToAxis("0 0" SPC getRandom() * 360 - 180));

	if (!isObject(BulletGroup)) {
		MissionCleanup.add(new SimGroup(BulletGroup));
	}

	BulletGroup.add(%item);
	%item.canPickup = false;

	%item.schedule(14000, fadeOut);
	%item.schedule(15000, delete);

	%obj.garandLoaded = false;

	if ( %obj.currGarandMagazine > 0 ) {
		serverPlay3d(garandRifleFireSound, %obj.getHackPosition());
		%obj.currGarandMagazine--;
		%obj.garandLoaded = true;
	}
	else {
		serverPlay3d(garandRifleFireLastSound, %obj.getHackPosition());
	}
}

function garandRifleProjectile::damage(%this, %obj, %col, %fade, %pos, %normal) {
	%damageType = %this.directDamageType;
	%damage = %this.directDamage;
	%scale = getWord(%col.getScale(), 2);

	if ( %col.isCrouched() || (getword(%pos, 2) > getword(%col.getWorldBoxCenter(), 2) - 3.4 * %scale) ) {
		if ( %col.isCrouched() ) {
			%damage = %damage/2;
		}
		%damage *= %this.headshotMultiplier;
		%headshot = true;
		if ( %this.headshotDamageType !$= "" ) {
			%damageType = %this.headshotDamageType;
		}
	}

	if(%col.getDamageLevel() + %damage >= %col.getDatablock().maxDamage && %headshot) {
		%col.isDying = true;
	}

	%col.damage( %obj, %pos, %damage, %damageType );
}

package garandRiflePackage {
	function serverCmdLight(%this) {
		if(isObject(%obj = %this.player)) {
			%image = %obj.getMountedImage(0);

			if ( %image == nameToID(GarandRifleImage) ) {
				%state = %obj.getImageState(0);

				if ( %state $= "Ready" || %state $= "Empty" ) {
					if ( !%obj.hasMag[%obj.currTool] ) {
						%amt = -1;
						%curr = -1;
						for ( %i = 0; %i < %obj.getDatablock().maxTools; %i++ ) {
							if ( %obj.tool[%i] == GarandRifleMagazineItem.getID() ) {
								if ( %obj.mag[%i] $= "" ) {
									%obj.mag[%i] = %obj.tool[%i].image.maxClip;
								}
								if ( %obj.mag[%i] > %amt ) {
									%amt = %obj.mag[%i];
									%curr = %i;
								}
							}
						}

						if ( %curr != -1 ) {
							%obj.currGarandMagazine = %obj.mag[%curr];
							%obj.mag[%curr] = 0;
							%obj.removeItemSlot(%curr);
							%obj.hasMag[%obj.currTool] = true;

							%obj.playThread(2,shiftleft);
							%obj.playAudio(2,garandRifleClipInSound);

							return;
						}

						centerPrint(%obj.client, "<color:ffffff>This rifle doesn't have a clip!", 2);
						return;
					}

					%slot = %obj.addItem(GarandRifleMagazineItem.getID());

					if ( %slot == -1 ) {
						centerPrint(%obj.client, "<color:ffffff>You don't have any available slots for a clip", 3);
						return;
					}

					%obj.playThread(2,shiftright);
					%obj.playAudio(2,garandRifleClipOutSound);

					%obj.mag[%slot] = %obj.currGarandMagazine;
					%obj.currGarandMagazine = 0;
					%obj.hasMag[%obj.currTool] = false;
				}
				return;
			}
			else if ( %image == nameToID(GarandRifleMagazineImage) ) {
				%slot = -1;

				for ( %i = 0; %i < %obj.getDatablock().maxTools; %i++ ) {
					if ( %obj.tool[%i] == GarandRifleItem.getID() ) {
						if ( !%obj.hasMag[%i] ) {
							%slot = %i;
							break;
						}
						else {
							%newclip = %obj.mag[%obj.currTool];
							%obj.removeItemSlot(%obj.currTool);

							%oldclip = %obj.currGarandMagazine;
							%newslot = %obj.addItem(GarandRifleMagazineItem.getID());

							%obj.mag[%newslot] = %oldclip;
							%obj.currGarandMagazine = %newclip;

							serverCmdUseTool(%obj.client, %slot);

							%obj.playThread(2,shiftleft);
							%obj.playAudio(2,garandRifleClipOutSound);

							return;
						}
					}
				}

				if ( %slot == -1 ) {
					centerPrint(%obj.client, "<color:ffffff>You don't have any available Garands to put this clip into!", 1);
					return;
				}

				%obj.playThread(2,shiftleft);
				%obj.playAudio(2,garandRifleClipInSound);

				%obj.currGarandMagazine = %obj.mag[%obj.currTool];
				%obj.mag[%obj.currTool] = 0;

				%obj.removeItemSlot(%obj.currTool);
				%obj.hasMag[%slot] = true;

				serverCmdUseTool(%obj.client, %slot);

				return;
			}
		}
		parent::serverCmdLight(%this);
	}

	function Armor::onTrigger(%this, %obj, %trig, %tog) {
		Parent::onTrigger(%this, %obj, %trig, %tog);

		%image = %obj.getMountedImage(0);

		if ( %image == nameToID(garandRifleImage) ) {
			if ( %trig == 4 && %tog ) {
				%state = %obj.getImageState(0);

				if ( %state $= "Ready" || %state $= "Empty" ) {
					%obj.setImageAmmo(0, 0);
				}
			}
		}
		// else if ( %image == garandRifleMagazineImage.getID() ) {
		// 	if ( %trig == 0 && %tog ) {
		// 		%mag = %obj.mag[%obj.currTool];
		// 		%pool = %obj.bullets["30-06"];

		// 		if ( %pool <= 0 || %mag >= garandRifleMagazineImage.maxClip ) {
		// 			garandRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 			return;
		// 		}

		// 		%obj.mag[%obj.currTool] += 1;
		// 		%obj.bullets[garandRifleMagazineImage.ammoType] -= 1;

		// 		garandRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 	}
		// 	else if ( %trig == 4 && %tog ) {
		// 		%mag = %obj.mag[%obj.currTool];
		// 		%pool = %obj.bullets[garandRifleMagazineImage.ammoType];

		// 		if ( %mag <= 0 ) {
		// 			garandRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 			return;
		// 		}

		// 		%obj.mag[%obj.currTool] -= 1;
		// 		%obj.bullets[garandRifleMagazineImage.ammoType] += 1;

		// 		garandRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 	}
		// }
	}

	function serverCmdDropTool(%client, %slot) {
		if ( isObject(%player = %client.player) ) {
			%item = %player.tool[%slot];

			if ( nameToID(garandRifleItem) == %item ) {
				%clip = %player.hasMag[%slot];
				%bullets = %player.currgarandMagazine;
				%loaded = %player.garandLoaded;

				%player.hasMag[%slot] = "";
				%player.currgarandMagazine = 0;
				%player.garandLoaded = false;

				$garandDropInfo = %clip SPC %bullets SPC %loaded;
			}
			else if ( nameToID(garandRifleMagazineItem) == %item ) {
				$garandMagDropInfo = %player.mag[%slot];
				%player.mag[%slot] = "";
			}
		}
		parent::serverCmdDropTool(%client, %slot);
	}

	function ItemData::onAdd(%this, %obj) {
		parent::onAdd(%this, %obj);

		if ( %this == nameToID(garandRifleItem) && $garandDropInfo !$= "" ) {
			%obj.hasMag = getWord($garandDropInfo, 0);
			%obj.currGarandMagazine = getWord($garandDropInfo, 1);
			%obj.garandLoaded = getWord($garandDropInfo, 2);

			$garandDropInfo = "";
		}
		else if ( %this == nameToID(garandRifleMagazineItem) ) {
			%obj.mag = $garandMagDropInfo;

			$garandMagDropInfo = "";
		}
	}

	function Player::pickUp(%player, %item) {
		%db = %item.getDatablock();
		%client = %player.client;

		if ( nameToID(garandRifleItem) == %db && %item.canPickup ) {
			%newslot = %player.addItem(garandRifleItem.getID());
			if ( %item.garandLoaded !$= "" ) {
				%player.garandLoaded = %item.garandLoaded;
				%player.currgarandMagazine = %item.currgarandMagazine;
				%player.hasMag[%newslot] = %item.hasMag;
			}
			else {
				%player.garandLoaded = false;
				%player.currgarandMagazine = 0;
				%player.hasMag[%newslot] = false;
			}

			if ( isObject(%item.spawnBrick) ) {
				%item.fadeOut();
				%item.schedule(%item.spawnBrick.itemRespawnTime, fadeIn);
			}
			else {
				%item.delete();
			}
			return;
		}
		else if ( nameToID(garandRifleMagazineItem) == %db ) {
			return;
		}
		else {
			return parent::pickUp(%player, %item);
		}
	}

	function serverCmdUseTool(%client, %slot) {
		parent::serverCmdUseTool(%client, %slot);
	}
};
if ( isPackage(garandRiflePackage) ) {
	deactivatepackage(garandRiflePackage);
}
activatePackage(garandRiflePackage);
