//sounds
datablock AudioProfile(thompsonRifleFireSound) {
	filename    = "./Sounds/thompson_fire.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(thompsonRifleFireLastSound) {
	filename    = "./Sounds/thompson_firelast.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(thompsonRifleBoltSound)
{
	filename    = "./Sounds/bolt.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(thompsonRifleClipInSound)
{
	filename    = "./Sounds/clipin.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(thompsonRifleClipOutSound)
{
	filename    = "./Sounds/clipout.wav";
	description = AudioClose3d;
	preload = true;
};


AddDamageType("ThompsonRifle",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_thompson> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_thompson> %1',0.2,1);
AddDamageType("ThompsonRifleHeadshot",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_thompson><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_thompson><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',0.2,1);
//muzzle flash effects
datablock ProjectileData(thompsonRifleProjectile : gunProjectile) {
	directDamageType	= $DamageType::ThompsonRifle;
	radiusDamageType	= $DamageType::ThompsonRifle;
	headshotDamageType	= $DamageType::ThompsonRifleHeadshot;
	directDamage        = 8;

	impactImpulse       = 100;
	verticalImpulse     = 50;

	muzzleVelocity      = 200;
	velInheritFactor    = 0.6;

	armingDelay         = 00;
	lifetime            = 4000;
	fadeDelay           = 3500;
	bounceElasticity    = 0.5;
	bounceFriction      = 0.20;
	isBallistic         = false;
	gravityMod = 0.0;

	particleEmitter     = advSmallBulletTrailEmitter; //bulletTrailEmitter;
	headshotMultiplier = 2;
};

//////////
// item //
//////////

datablock ItemData(thompsonRifleItem) {
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/weapons/thompson_mg.dts";
	iconName = "./shapes/weapons/icons/thompson";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Thompson";
	//iconName = "./icons/icon_Pistol";
	doColorShift = false;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	magItem = thompsonRifleMagazineItem;
	reload = true;

	maxmag = 20;
	ammotype = "45";
	nochamber = 1;

	 // Dynamic properties defined by the scripts
	image = thompsonRifleImage;
	canDrop = true;
};

function thompsonRifleItem::onAdd(%this, %obj) {
	parent::onAdd(%this, %obj);
	if(!%obj.hasMag) {
		%obj.hideNode("clip");
	}
}

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(thompsonRifleImage) {
	// Basic Item properties
	shapeFile = "./shapes/weapons/thompson_mg.dts";
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
	item = thompsonRifleItem;
	ammo = " ";
	projectile = thompsonRifleProjectile;
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
	colorShiftColor = thompsonRifleItem.colorShiftColor;//"0.400 0.196 0 1.000";

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
	stateTimeoutValue[4]				= 0.03;
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
	stateTimeoutValue[5]				= 0.05;
	stateWaitForTimeout[5]				= true;
	stateTransitionOnTimeout[5]			= "CheckChamber";

	stateName[6]						= "CheckTrigger";
	stateTransitionOnTriggerUp[6]		= "CheckChamber";

	stateName[7]						= "EmptyClick"; //*click!*
	stateTransitionOnTriggerUp[7]		= "checkChamber";
	// stateSequence[7]					= "Fire";
	stateTimeoutValue[7]				= 0.13;
	stateAllowImageChange[7]			= false;
	stateWaitForTimeout[7]				= true;
	stateSound[7]						= "none";
	stateScript[7]						= "onEmptyFire";

	stateName[8]						= "PullBackSlide";
	stateScript[8]						= "onReload";
	stateSequence[8]					= "Fire";
	stateTransitionOnTimeout[8]			= "CheckTrigger";
	stateSound[8]						= thompsonRifleBoltSound;
	stateTimeoutValue[8]				= 0.2;
	stateWaitForTimeout[8]				= true;
};

datablock ItemData(thompsonRifleMagazineItem) {
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./shapes/items/thompson_clip.dts";
	iconName = "./shapes/weapons/icons/thompson_clip";
	uiName = "45 Thompson Magazine";
	doColorShift = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	maxmag = 20;
	ammotype = "45";
	isMag = true;

	image = thompsonRifleMagazineImage;
	canDrop = true;
	canPickup = true;
};

datablock ShapeBaseImageData(thompsonRifleMagazineImage) {
	// Basic Item properties
	shapeFile = "./shapes/items/thompson_clip.dts";
	emap = true;
	mountPoint = 0;
	className = "WeaponImage";
	armReady = true;
	doColorShift = false;

	isMag = true;
	ammoType = "45";
	maxClip = 30;
	ammoItem = Bullet45Item;
	item = thompsonRifleMagazineItem;
};

function thompsonRifleMagazineImage::onMount(%this, %obj, %slot) {
	if ( %obj.mag[%obj.currTool] $= "" ) {
		%obj.mag[%obj.currTool] = %this.maxClip;
	}
	parent::onMount(%this,%obj,%slot);
	thompsonRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
}

  ////// ammo display functions
function thompsonRifleImage::onMount(%this, %obj, %slot) {
	if ( %obj.hasMag[%obj.currTool] $= "" )
	{
		%obj.hasMag[%obj.currTool] = false;
	}
	if ( %obj.hasMag[%obj.currTool] == false )
	{
		%obj.mountImage(thompsonRifleEmptyImage, 0);
		return;
	}
	parent::onMount(%this,%obj,%slot);
}

function thompsonRifleImage::onUnMount(%this, %obj, %slot) {
	parent::onUnMount(%this,%obj,%slot);
}

function thompsonRifleImage::onCheckChamber(%this, %obj, %slot) {
	if ( !%obj.thompsonLoaded ) {
		%obj.setImageLoaded(0, 0);
	}
	else {
		%obj.setImageLoaded(0, 1);
	}
}

function thompsonRifleImage::onEmptyFire(%this, %obj, %slot) {
	if ( %obj.currthompsonMagazine > 0 ) { //if there's any bullets to cycle through...
		%obj.setImageAmmo(0, 0); //Force a cycle if we try to fire before cycling manually
		return;
	}
	serverPlay3d(advReloadTap1Sound,%obj.getHackPosition());
}

function thompsonRifleImage::onReload(%this, %obj, %slot) {
	if ( %obj.thompsonLoaded ) {
		%obj.thompsonLoaded = false;

		%datablock = Bullet45Item;

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

	if ( %obj.currthompsonMagazine > 0 ) {
		%obj.currthompsonMagazine--;
		%obj.thompsonLoaded = true;
	}

	%obj.setImageAmmo(0, 1);
}

function thompsonRifleImage::getProjectileSpread(%this, %obj, %slot) {
	%spread = 2.5; //Rather high spread
	return %spread; //+ vectorLen(%obj.getVelocity()) * 0.2;
}

function thompsonRifleImage::onFire(%this, %obj, %slot) {
	if ( %obj.getDamagePercent() > 1.0 ) {
		return;
	}

	parent::onFire(%this, %obj, %slot);

	%obj.playThread(2, plant);

	//Eject the shell after every shot
	%datablock = Shell45Item;

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

	%obj.thompsonLoaded = false;

	if ( %obj.currThompsonMagazine > 0 ) {
		serverPlay3d(thompsonRifleFireSound, %obj.getHackPosition());
		%obj.currThompsonMagazine--;
		%obj.thompsonLoaded = true;
	}
	else {
		serverPlay3d(thompsonRifleFireLastSound, %obj.getHackPosition());
	}
}

function thompsonRifleProjectile::damage(%this, %obj, %col, %fade, %pos, %normal) {
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


datablock ShapeBaseImageData(thompsonRifleEmptyImage : thompsonRifleImage)
{
	stateSequence[0]					= "noclip";
	stateSequence[2]					= "noclip";
	stateSequence[4]					= "noclip_fire";
	stateSequence[8]					= "noclip_fire";
};

function thompsonRifleEmptyImage::getProjectileSpread(%this, %obj, %slot)
{
	thompsonRifleImage::getProjectileSpread(%this, %obj, %slot);
}

function thompsonRifleEmptyImage::onFire(%this, %obj, %slot)
{
	thompsonRifleImage::onFire(%this, %obj, %slot);
}

function thompsonRifleEmptyImage::damage(%this, %obj, %slot)
{
	thompsonRifleImage::damage(%this, %obj, %slot);
}

function thompsonRifleEmptyImage::onReload(%this, %obj, %slot)
{
	thompsonRifleImage::onReload(%this, %obj, %slot);
}

function thompsonRifleEmptyImage::onEmptyFire(%this, %obj, %slot)
{
	thompsonRifleImage::onEmptyFire(%this, %obj, %slot);
}

function thompsonRifleEmptyImage::onCheckChamber(%this, %obj, %slot)
{
	thompsonRifleImage::onCheckChamber(%this, %obj, %slot);
}

package thompsonRiflePackage {
	function serverCmdLight(%this) {
		if(isObject(%obj = %this.player)) {
			%image = %obj.getMountedImage(0);

			if ( %image == nameToID(ThompsonRifleImage) || %image == nameToID(ThompsonRifleEmptyImage) ) {
				%state = %obj.getImageState(0);

				if ( %state $= "Ready" || %state $= "Empty" ) {
					if ( !%obj.hasMag[%obj.currTool] ) {
						%amt = -1;
						%curr = -1;
						for ( %i = 0; %i < %obj.getDatablock().maxTools; %i++ ) {
							if ( %obj.tool[%i] == ThompsonRifleMagazineItem.getID() ) {
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
							%obj.currThompsonMagazine = %obj.mag[%curr];
							%obj.mag[%curr] = 0;
							%obj.removeItemSlot(%curr);
							%obj.hasMag[%obj.currTool] = true;

							%obj.mountImage(thompsonRifleImage, 0);
							%obj.playThread(2,shiftleft);
							%obj.playAudio(2,thompsonRifleClipInSound);

							return;
						}

						centerPrint(%obj.client, "<color:ffffff>This rifle doesn't have a clip!", 2);
						return;
					}

					%slot = %obj.addItem(ThompsonRifleMagazineItem.getID());

					if ( %slot == -1 ) {
						centerPrint(%obj.client, "<color:ffffff>You don't have any available slots for a clip", 3);
						return;
					}

					%obj.mountImage(thompsonRifleEmptyImage, 0);
					%obj.playThread(2,shiftright);
					%obj.playAudio(2,thompsonRifleClipOutSound);

					%obj.mag[%slot] = %obj.currThompsonMagazine;
					%obj.currThompsonMagazine = 0;
					%obj.hasMag[%obj.currTool] = false;
				}
				return;
			}
			else if ( %image == nameToID(ThompsonRifleMagazineImage) ) {
				%slot = -1;

				for ( %i = 0; %i < %obj.getDatablock().maxTools; %i++ ) {
					if ( %obj.tool[%i] == ThompsonRifleItem.getID() ) {
						if ( !%obj.hasMag[%i] ) {
							%slot = %i;
							break;
						}
						else {
							%newclip = %obj.mag[%obj.currTool];
							%obj.removeItemSlot(%obj.currTool);

							%oldclip = %obj.currThompsonMagazine;
							%newslot = %obj.addItem(ThompsonRifleMagazineItem.getID());

							%obj.mag[%newslot] = %oldclip;
							%obj.currThompsonMagazine = %newclip;

							serverCmdUseTool(%obj.client, %slot);

							%obj.playThread(2,shiftleft);
							%obj.playAudio(2,thompsonRifleClipOutSound);

							return;
						}
					}
				}

				if ( %slot == -1 ) {
					centerPrint(%obj.client, "<color:ffffff>You don't have any available Thompsons to put this clip into!", 1);
					return;
				}

				%obj.playThread(2,shiftleft);
				%obj.playAudio(2,thompsonRifleClipInSound);

				%obj.currThompsonMagazine = %obj.mag[%obj.currTool];
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

		if ( %image == nameToID(thompsonRifleImage) || %image == nameToID(thompsonRifleEmptyImage) ) {
			if ( %trig == 4 && %tog ) {
				%state = %obj.getImageState(0);

				if ( %state $= "Ready" || %state $= "Empty" ) {
					%obj.setImageAmmo(0, 0);
				}
			}
		}
		// else if ( %image == thompsonRifleMagazineImage.getID() ) {
		// 	if ( %trig == 0 && %tog ) {
		// 		%mag = %obj.mag[%obj.currTool];
		// 		%pool = %obj.bullets["45"];

		// 		if ( %pool <= 0 || %mag >= thompsonRifleMagazineImage.maxClip ) {
		// 			thompsonRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 			return;
		// 		}

		// 		%obj.mag[%obj.currTool] += 1;
		// 		%obj.bullets[thompsonRifleMagazineImage.ammoType] -= 1;

		// 		thompsonRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 	}
		// 	else if ( %trig == 4 && %tog ) {
		// 		%mag = %obj.mag[%obj.currTool];
		// 		%pool = %obj.bullets[thompsonRifleMagazineImage.ammoType];

		// 		if ( %mag <= 0 ) {
		// 			thompsonRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 			return;
		// 		}

		// 		%obj.mag[%obj.currTool] -= 1;
		// 		%obj.bullets[thompsonRifleMagazineImage.ammoType] += 1;

		// 		thompsonRifleMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 	}
		// }
	}

	function serverCmdDropTool(%client, %slot) {
		if ( isObject(%player = %client.player) ) {
			%item = %player.tool[%slot];

			if ( nameToID(thompsonRifleItem) == %item ) {
				%clip = %player.hasMag[%slot];
				%bullets = %player.currthompsonMagazine;
				%loaded = %player.thompsonLoaded;

				%player.hasMag[%slot] = "";
				%player.currthompsonMagazine = 0;
				%player.thompsonLoaded = false;

				$thompsonDropInfo = %clip SPC %bullets SPC %loaded;

				%player.unMountImage(0); //This makes sure that even if we have the empty thompson it'll still be unmounted.
			}
			else if ( nameToID(thompsonRifleMagazineItem) == %item ) {
				$thompsonMagDropInfo = %player.mag[%slot];
				%player.mag[%slot] = "";
			}
		}
		parent::serverCmdDropTool(%client, %slot);
	}

	function ItemData::onAdd(%this, %obj) {
		parent::onAdd(%this, %obj);

		if ( %this == nameToID(thompsonRifleItem) && $thompsonDropInfo !$= "" ) {
			%obj.hasMag = getWord($thompsonDropInfo, 0);
			%obj.currThompsonMagazine = getWord($thompsonDropInfo, 1);
			%obj.thompsonLoaded = getWord($thompsonDropInfo, 2);

			$thompsonDropInfo = "";
		}
		else if ( %this == nameToID(thompsonRifleMagazineItem) ) {
			%obj.mag = $thompsonMagDropInfo;

			$thompsonMagDropInfo = "";
		}
	}

	function Player::pickUp(%player, %item) {
		%db = %item.getDatablock();
		%client = %player.client;
		if ( nameToID(thompsonRifleItem) == %db && %item.canPickup ) {
			%newslot = %player.addItem(thompsonRifleItem.getID());
			if ( %item.thompsonLoaded !$= "" ) {
				%player.thompsonLoaded = %item.thompsonLoaded;
				%player.currthompsonMagazine = %item.currthompsonMagazine;
				%player.hasMag[%newslot] = %item.hasMag;
			}
			else {
				%player.thompsonLoaded = false;
				%player.currthompsonMagazine = 0;
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
		else if ( nameToID(thompsonRifleMagazineItem) == %db ) {
			return;
		}
		else {
			return parent::pickUp(%player, %item);
		}
	}

	function serverCmdUseTool(%client, %slot) {
		parent::serverCmdUseTool(%client, %slot);
	}

	function serverCmdUnuseTool(%this) {
		parent::serverCmdUnuseTool(%this);
	}
};
if ( isPackage(thompsonRiflePackage) ) {
	deactivatepackage(thompsonRiflePackage);
}
activatePackage(thompsonRiflePackage);
