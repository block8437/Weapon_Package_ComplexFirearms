//sounds
datablock AudioProfile(RemmingtonShotgunFireSound) {
	filename    = "./Sounds/shotgun_fire.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(RemmingtonShotgunPumpSound)
{
	filename    = "./Sounds/shotgun_pump.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(RemmingtonShotgunInsertSound)
{
	filename    = "./Sounds/shotgun_reload.wav";
	description = AudioClose3d;
	preload = true;
};

AddDamageType("Remmington",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_remmington> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_remmington> %1',0.2,1);
// AddDamageType("RemmingtonHeadshot",
// 	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_remmington><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',
// 	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_remmington><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',0.2,1);
//muzzle flash effects
datablock ProjectileData(RemmingtonShotgunProjectile : gunProjectile) {
	directDamageType	= $DamageType::Remmington;
	radiusDamageType	= $DamageType::Remmington;
	// headshotDamageType	= $DamageType::RemmingtonHeadshot;
	directDamage        = 9;

	impactImpulse       = 100;
	verticalImpulse     = 50;

	muzzleVelocity      = 125;
	velInheritFactor    = 0.75;

	armingDelay         = 00;
	lifetime            = 4000;
	fadeDelay           = 3500;
	bounceElasticity    = 0.5;
	bounceFriction      = 0.20;
	isBallistic         = false;
	gravityMod = 0.0;

	particleEmitter     = advSmallBulletTrailEmitter; //bulletTrailEmitter;
	headshotMultiplier = 1;
};

//////////
// item //
//////////

datablock ItemData(RemmingtonShotgunItem) {
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/weapons/remmington870.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Remmington 870";
	iconName = "./shapes/weapons/icons/remmington";
	doColorShift = false;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	ammoItem = BulletBuckshotItem;
	reload = true;

	maxmag = 6;
	ammotype = "buckshot";
	nochamber = 1;

	 // Dynamic properties defined by the scripts
	image = RemmingtonShotgunImage;
	canDrop = true;

	clickPickUp = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(RemmingtonShotgunImage) {
	// Basic Item properties
	shapeFile = "./shapes/weapons/remmington870.dts";
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
	item = RemmingtonShotgunItem;
	ammo = " ";

	timedCustomFire = true;
	fireSpeed = cf_muzzlevelocity_ms(472.44);
	fireGravity = "0 0" SPC cf_bulletdrop_grams(25);
	fireLifetime = 1;
	velInheritFactor = 0.75;
	projectile = RemmingtonShotgunProjectile;
	projectileCount = 8;

	directDamageType	= $DamageType::Remmington;
	radiusDamageType	= $DamageType::Remmington;
	// headshotDamageType	= $DamageType::RemmingtonHeadshot;
	directDamage        = 9;
	headshotMultiplier = 1;

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
	colorShiftColor = RemmingtonShotgunItem.colorShiftColor;//"0.400 0.196 0 1.000";

	//casing = " ";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state

	stateName[0]						= "Activate";
	stateTimeoutValue[0]				= 0.15;
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
	stateSequence[3]					= "root";
	stateScript[3]						= "onEmpty";

	stateName[4]						= "Fire";
	stateTimeoutValue[4]				= 0.12;
	stateTransitionOnTimeout[4]			= "Smoke";
	stateFire[4]						= true;
	stateAllowImageChange[4]			= false;
	stateSequence[4]					= "Fire";
	stateScript[4]						= "onFire";
	stateWaitForTimeout[4]				= true;
	stateEmitter[4]						= advSmallBulletFireEmitter;
	stateEmitterTime[4]					= 0.05;
	stateEmitterNode[4]					= "muzzleNode";
	// stateEjectShell[4]					= true;

	stateName[5]						= "Smoke";
	stateEmitter[5]						= advSmallBulletSmokeEmitter;
	stateEmitterTime[5]					= 0.05;
	stateEmitterNode[5]					= "muzzleNode";
	stateTimeoutValue[5]				= 0.1;
	stateTransitionOnTriggerUp[5]		= "CheckChamber";
	stateWaitForTimeout[5]				= true;

	stateName[6]						= "EmptyClick"; //*click!*
	stateTransitionOnTriggerUp[6]		= "checkChamber";
	stateTimeoutValue[6]				= 0.13;
	stateAllowImageChange[6]			= false;
	stateWaitForTimeout[6]				= true;
	// stateSound[6]						= "";
	stateScript[6]						= "onEmptyFire";
	stateSequence[6]					= "root";

	stateName[7]						= "PullBackSlide";
	stateSound[7]						= RemmingtonShotgunPumpSound;
	stateScript[7]						= "onReload";
	stateSequence[7]					= "Pump";
	stateTransitionOnTimeout[7]			= "CheckTrigger";
	stateTimeoutValue[7]				= 0.2;

	stateName[8]						= "CheckTrigger";
	stateTransitionOnTriggerUp[8]		= "CheckChamber";
};

  ////// ammo display functions
function RemmingtonShotgunImage::onMount(%this, %obj, %slot) {
	parent::onMount(%this,%obj,%slot);
}

function RemmingtonShotgunImage::onUnMount(%this, %obj, %slot) {
	parent::onUnMount(%this,%obj,%slot);
}

function RemmingtonShotgunImage::onCheckChamber(%this, %obj, %slot) {
	if ( !%obj.remmingtonLoaded ) {
		%obj.setImageLoaded(0, 0);
	}
	else {
		%obj.setImageLoaded(0, 1);
	}
}

function RemmingtonShotgunImage::onEmptyFire(%this, %obj, %slot) {
	if ( %obj.currRemmingtonHold > 0 ) { //if there's any bullets to cycle through...
		%obj.setImageAmmo(0, 0); //Force a cycle if we try to fire before cycling manually
		return;
	}

	serverPlay3d(advReloadTap1Sound,%obj.getHackPosition());
}

function RemmingtonShotgunImage::onReload(%this, %obj, %slot) {
	%muzzleVector = %obj.getMuzzleVector(0);
	%cross = vectorCross(%muzzleVector, "0 0 1");
	%ejectPos = vectorAdd(%obj.getMuzzlePoint(0), vectorScale(%muzzleVector, -1.5));
	%vector = vectorAdd(vectorScale(%obj.getEyeVector(), -2), "0 0 5");
	%vector = vectorAdd(%vector, %cross);
	if ( %obj.remmingtonLoaded ) {
		%obj.remmingtonLoaded = false;

		%datablock = BulletBuckshotItem;

		%item = new Item() {
			dataBlock = %datablock;
			position = %ejectPos;
		};

		%spread = 15;
		%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
		%spread = vectorScale(%scalars, mDegToRad(%spread / 2));
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
	else if ( %obj.remmingtonShell ) {
		//Eject the shell after every shot
		%datablock = ShellBuckshotItem;

		%item = new Item() {
			dataBlock = %datablock;
			position = %ejectPos;
		};

		%spread = 15;
		%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
		%spread = vectorScale(%scalars, mDegToRad(%spread / 2));
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

		%obj.remmingtonShell = false;
	}

	if ( %obj.currRemmingtonHold > 0 ) {
		%obj.currRemmingtonHold--;
		%obj.remmingtonLoaded = true;
	}

	%obj.setImageAmmo(0, 1);
}

function RemmingtonShotgunImage::getProjectileSpread(%this, %obj, %slot) {
	%spread = 11;
	return %spread; // + vectorLen(%obj.getVelocity()) * 0.25;
}

function RemmingtonShotgunImage::onFire(%this, %obj, %slot) {
	if ( %obj.getDamagePercent() > 1.0 ) {
		return;
	}

	parent::onFire(%this, %obj, %slot);

	%obj.playThread(2, shiftForward);

	%obj.remmingtonShell = true;
	%obj.remmingtonLoaded = false;

	serverPlay3d(RemmingtonShotgunFireSound, %obj.getHackPosition());

}
//
// function RemmingtonShotgunProjectile::damage(%this, %obj, %col, %fade, %pos, %normal) {
// 	%damage = %this.directDamage;
// 	%scale = getWord(%col.getScale(), 2);
//
// 	if ( %col.isCrouched() || (getword(%pos, 2) > getword(%col.getWorldBoxCenter(), 2) - 3.4 * %scale) ) {
// 		if ( %col.isCrouched() ) {
// 			%damage = %damage/2;
// 		}
// 		%damage *= %this.headshotMultiplier;
// 		%headshot = true;
// 	}
//
// 	if(%col.getDamageLevel() + %damage >= %col.getDatablock().maxDamage && %headshot) {
// 		%col.isDying = true;
// 	}
//
// 	%col.damage( %obj, %pos, %damage, %this.directDamageType );
// }

package RemmingtonShotgunPackage {
	function serverCmdLight(%this) {
		if(isObject(%obj = %this.player)) {
			%image = %obj.getMountedImage(0);

			if ( %image == nameToID(RemmingtonShotgunImage) ) {
				%state = %obj.getImageState(0);

				if ( %state $= "Ready" || %state $= "Empty" ) {
					%pool = %obj.bullets["buckshot"];

					if ( %obj.currRemmingtonHold >= RemmingtonShotgunItem.maxmag || %pool <= 0 )
						return;

					%now = getSimTime();
					if ( %now - %obj.lastRemmingtonReload <= 200 )
						return;

					%obj.lastRemmingtonReload = %now;

					%obj.currRemmingtonHold += 1;
					%obj.bullets["buckshot"] -= 1;

					serverPlay3d(RemmingtonShotgunInsertSound, %obj.getHackPosition());
					%obj.playThread(0, shiftLeft);

					BulletBuckshotItem.UpdateAmmoPrint(%obj, 0, 3);
				}
				return;
			}
		}
		parent::serverCmdLight(%this);
	}

	function Armor::onTrigger(%this, %obj, %trig, %tog) {
		Parent::onTrigger(%this, %obj, %trig, %tog);

		%image = %obj.getMountedImage(0);

		if ( %image == nameToID(RemmingtonShotgunImage) ) {
			if ( %trig == 4 && %tog ) {
				%state = %obj.getImageState(0);

				if ( %state $= "Ready" || %state $= "Empty" ) {
					%obj.setImageAmmo(0, 0);
				}
			}
		}
	}

	function serverCmdDropTool(%client, %slot) {
		if ( isObject(%player = %client.player) ) {
			%item = %player.tool[%slot];

			if ( nameToID(RemmingtonShotgunItem) == %item ) {
				%bullets = %player.currRemmingtonHold;
				%loaded = %player.remmingtonLoaded;
				%shell = %player.remmingtonShell;

				%player.currRemmingtonHold = 0;
				%player.remmingtonLoaded = false;
				%player.remmingtonShell = false;

				$RemmingtonDropInfo = %bullets SPC %loaded SPC %shell;
			}
		}
		parent::serverCmdDropTool(%client, %slot);
	}

	function ItemData::onAdd(%this, %obj) {
		parent::onAdd(%this, %obj);

		if ( %this == nameToID(RemmingtonShotgunItem) && $RemmingtonDropInfo !$= "" ) {
			%obj.currRemmingtonHold = getWord($RemmingtonDropInfo, 0);
			%obj.remmingtonLoaded = getWord($RemmingtonDropInfo, 1);
			%obj.remmingtonShell = getWord($RemmingtonDropInfo, 2);

			$RemmingtonDropInfo = "";
		}
	}

	function Player::pickUp(%player, %item) {
		%db = %item.getDatablock();
		%client = %player.client;

		if ( nameToID(RemmingtonShotgunItem) == %db ) {
			%newslot = %player.addItem(RemmingtonShotgunItem.getID());
			if ( %item.remmingtonLoaded !$= "" ) {
				%player.remmingtonLoaded = %item.remmingtonLoaded;
				%player.currRemmingtonHold = %item.currRemmingtonHold;
				%player.remmingtonShell = %item.remmingtonShell;
			}
			else {
				%player.remmingtonLoaded = false;
				%player.currRemmingtonHold = 0;
				%player.remmingtonShell = false;
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
		else {
			return parent::pickUp(%player, %item);
		}
	}

	function serverCmdUseTool(%client, %slot) {
		parent::serverCmdUseTool(%client, %slot);
	}
};
if ( isPackage(RemmingtonShotgunPackage) ) {
	deactivatepackage(RemmingtonShotgunPackage);
}
activatePackage(RemmingtonShotgunPackage);
