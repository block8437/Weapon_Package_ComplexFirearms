//sounds
datablock AudioProfile(ColtPistolFireSound) {
	filename    = "./Sounds/m1911_fire.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(ColtPistolFireLastSound) {
	filename    = "./Sounds/m1911_fireLast.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(ColtPistolSlidepullSound)
{
	filename    = "./Sounds/pistol_slidepull.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(ColtPistolClipInSound)
{
	filename    = "./Sounds/pistol_clipin.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(ColtPistolClipOutSound)
{
	filename    = "./Sounds/pistol_clipout.wav";
	description = AudioClose3d;
	preload = true;
};

//muzzle flash effects

AddDamageType("ColtPistol",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_colt> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_colt> %1',0.2,1);
AddDamageType("ColtPistolHeadshot",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_colt><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_colt><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',0.2,1);
datablock ProjectileData(ColtPistolProjectile : gunProjectile) {
	directDamageType	= $DamageType::ColtPistol;
	radiusDamageType	= $DamageType::ColtPistol;
	headshotDamageType	= $DamageType::ColtPistolHeadshot;
	directDamage        = 16;

	impactImpulse       = 100;
	verticalImpulse     = 50;

	muzzleVelocity      = 200;
	velInheritFactor    = 0.25;

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

datablock ItemData(ColtPistolItem) {
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/weapons/Colt_1911.dts";
	iconName = "./shapes/weapons/icons/colt";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Colt 1911 Pistol";
	//iconName = "./icons/icon_Pistol";
	doColorShift = false;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	magItem = ColtPistolMagazineItem;
	reload = true;

	maxmag = 20;
	ammotype = "45";
	nochamber = 1;

	 // Dynamic properties defined by the scripts
	image = ColtPistolImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(ColtPistolImage) {
	// Basic Item properties
	shapeFile = "./shapes/weapons/Colt_1911.dts";
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
	item = ColtPistolItem;
	ammo = " ";
	projectile = ColtPistolProjectile;
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
	colorShiftColor = ColtPistolItem.colorShiftColor;//"0.400 0.196 0 1.000";

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
	stateSequence[3]					= "noammo";
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
	stateSequence[6]					= "emptyFire";

	stateName[7]						= "PullBackSlide";
	stateSound[7]						= ColtPistolSlidepullSound;
	stateScript[7]						= "onReload";
	stateSequence[7]					= "ejectShell";
	stateTransitionOnTimeout[7]			= "CheckTrigger";
	stateTimeoutValue[7]				= 0.2;

	stateName[8]						= "CheckTrigger";
	stateTransitionOnTriggerUp[8]		= "CheckChamber";
};

datablock ItemData(ColtPistolMagazineItem) {
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./shapes/items/colt_magazine.dts";
	iconName = "./shapes/weapons/icons/colt_clip";
	uiName = "45 Magazine";
	doColorShift = true;
	colorShiftColor = "0.749 0.749 0.749 1";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	maxmag = 20;
	ammotype = "45";
	isMag = true;

	image = ColtPistolMagazineImage;
	canDrop = true;
	canPickup = true;
};

datablock ShapeBaseImageData(ColtPistolMagazineImage) {
	// Basic Item properties
	shapeFile = "./shapes/items/colt_magazine.dts";
	emap = true;
	mountPoint = 0;
	className = "WeaponImage";
	armReady = true;
	doColorShift = true;
	colorShiftColor = "0.749 0.749 0.749 1";

	isMag = true;
	ammoType = "45";
	maxClip = 11; //Realistically it should be 7 but it clogs up the inventory too much, making revolver a much better sidearm overall
	ammoItem = Bullet45Item;
	item = ColtPistolMagazineItem;
};

function ColtPistolMagazineImage::onMount(%this, %obj, %slot) {
	parent::onMount(%this,%obj,%slot);
	ColtPistolMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
}

  ////// ammo display functions
function ColtPistolImage::onMount(%this, %obj, %slot) {
	if ( %obj.hasMag[%obj.currTool] $= "" )
		%obj.hasMag[%obj.currTool] = false;

	parent::onMount(%this,%obj,%slot);
}

function ColtPistolImage::onUnMount(%this, %obj, %slot) {
	parent::onUnMount(%this,%obj,%slot);
}

function ColtPistolImage::onCheckChamber(%this, %obj, %slot) {
	if ( !%obj.coltLoaded ) {
		%obj.setImageLoaded(0, 0);
	}
	else {
		%obj.setImageLoaded(0, 1);
	}
}

function ColtPistolImage::onEmptyFire(%this, %obj, %slot) {
	if ( %obj.currColtMagazine > 0 ) { //if there's any bullets to cycle through...
		%obj.setImageAmmo(0, 0); //Force a cycle if we try to fire before cycling manually
		return;
	}
	serverPlay3d(advReloadTap1Sound,%obj.getHackPosition());
}

function ColtPistolImage::onReload(%this, %obj, %slot) {
	if ( %obj.coltLoaded ) {
		%obj.coltLoaded = false;

		%datablock = Bullet45Item;

		%item = new Item() {
			dataBlock = %datablock;
			position = vectorAdd(%obj.getMuzzlePoint(0), vectorScale(%obj.getMuzzleVector(0), -1));
		};

		%spread = 15;
		%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
		%spread = vectorScale(%scalars, mDegToRad(%spread / 2));

		%vector = vectorAdd(vectorScale(%obj.getEyeVector(), -2), "0 0 -5");
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

	if ( %obj.currColtMagazine > 0 ) {
		%obj.currColtMagazine--;
		%obj.coltLoaded = true;
	}

	%obj.setImageAmmo(0, 1);
}

function ColtPistolImage::getProjectileSpread(%this, %obj, %slot) {
	%spread = 0.4;
	return %spread; // + vectorLen(%obj.getVelocity()) * 0.25;
}

function ColtPistolImage::onFire(%this, %obj, %slot) {
	if ( %obj.getDamagePercent() > 1.0 ) {
		return;
	}

	parent::onFire(%this, %obj, %slot);

	%obj.playThread(2, shiftForward);

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

	%obj.coltLoaded = false;

	if ( %obj.currColtMagazine > 0 ) {
		serverPlay3d(ColtPistolFireSound, %obj.getHackPosition());
		%obj.currColtMagazine--;
		%obj.coltLoaded = true;
	}
	else {
		serverPlay3d(ColtPistolFireLastSound, %obj.getHackPosition());
	}
}

function ColtPistolProjectile::damage(%this, %obj, %col, %fade, %pos, %normal) {
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

package ColtPistolPackage {
	function serverCmdLight(%this) {
		if(isObject(%obj = %this.player)) {
			%image = %obj.getMountedImage(0);

			if ( %image == nameToID(ColtPistolImage) ) {
				%state = %obj.getImageState(0);

				if ( %state $= "Ready" || %state $= "Empty" ) {
					if ( !%obj.hasMag[%obj.currTool] ) {
						%amt = -1;
						%curr = -1;
						for ( %i = 0; %i < %obj.getDatablock().maxTools; %i++ ) {
							if ( %obj.tool[%i] == ColtPistolMagazineItem.getID() ) {
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
							%obj.currColtMagazine = %obj.mag[%curr];
							%obj.mag[%curr] = 0;
							%obj.removeItemSlot(%curr);
							%obj.hasMag[%obj.currTool] = true;

							%obj.playThread(2,shiftleft);
							%obj.playAudio(2,ColtPistolClipInSound);

							return;
						}

						centerPrint(%obj.client, "<color:ffffff>This pistol doesn't have a magazine!", 2);
						return;
					}

					%slot = %obj.addItem(ColtPistolMagazineItem.getID());

					if ( %slot == -1 ) {
						centerPrint(%obj.client, "<color:ffffff>You don't have any available slots for a magazine", 3);
						return;
					}

					%obj.playThread(2,shiftright);
					%obj.playAudio(2,ColtPistolClipOutSound);

					%obj.mag[%slot] = %obj.currColtMagazine;
					%obj.currColtMagazine = 0;
					%obj.hasMag[%obj.currTool] = false;
				}
				return;
			}
			else if ( %image == nameToID(ColtPistolMagazineImage) ) {
				%slot = -1;

				for ( %i = 0; %i < %obj.getDatablock().maxTools; %i++ ) {
					if ( %obj.tool[%i] == ColtPistolItem.getID() ) {
						if ( !%obj.hasMag[%i] ) {
							%slot = %i;
							break;
						}
						else {
							%newclip = %obj.mag[%obj.currTool];
							%obj.removeItemSlot(%obj.currTool);

							%oldclip = %obj.currColtMagazine;
							%newslot = %obj.addItem(ColtPistolMagazineItem.getID());

							%obj.mag[%newslot] = %oldclip;
							%obj.currColtMagazine = %newclip;

							serverCmdUseTool(%obj.client, %slot);

							%obj.playThread(2,shiftleft);
							%obj.playAudio(2,ColtPistolClipInSound);

							return;
						}
					}
				}

				if ( %slot == -1 ) {
					centerPrint(%obj.client, "<color:ffffff>You don't have any available Colts to put this magazine into!", 1);
					return;
				}

				%obj.playThread(2,shiftleft);
				%obj.playAudio(2,ColtPistolClipInSound);

				%obj.currColtMagazine = %obj.mag[%obj.currTool];
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

		if ( %image == nameToID(ColtPistolImage) ) {
			if ( %trig == 4 && %tog ) {
				%state = %obj.getImageState(0);

				if ( %state $= "Ready" || %state $= "Empty" ) {
					%obj.setImageAmmo(0, 0);
				}
			}
		}
		// else if ( %image == ColtPistolMagazineImage.getID() ) {
		// 	if ( %trig == 0 && %tog ) {
		// 		%mag = %obj.mag[%obj.currTool];
		// 		%pool = %obj.bullets["45"];

		// 		if ( %pool <= 0 || %mag >= ColtPistolMagazineImage.maxClip ) {
		// 			ColtPistolMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 			return;
		// 		}

		// 		%obj.mag[%obj.currTool] += 1;
		// 		%obj.bullets[ColtPistolMagazineImage.ammoType] -= 1;

		// 		ColtPistolMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 	}
		// 	else if ( %trig == 4 && %tog ) {
		// 		%mag = %obj.mag[%obj.currTool];
		// 		%pool = %obj.bullets[ColtPistolMagazineImage.ammoType];

		// 		if ( %mag <= 0 ) {
		// 			ColtPistolMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 			return;
		// 		}

		// 		%obj.mag[%obj.currTool] -= 1;
		// 		%obj.bullets[ColtPistolMagazineImage.ammoType] += 1;

		// 		ColtPistolMagazineImage.ammoItem.UpdateAmmoPrint(%obj, 0, 1);
		// 	}
		// }
	}

	function serverCmdDropTool(%client, %slot) {
		if ( isObject(%player = %client.player) ) {
			%item = %player.tool[%slot];

			if ( nameToID(ColtPistolItem) == %item ) {
				%clip = %player.hasMag[%slot];
				%bullets = %player.currColtMagazine;
				%loaded = %player.coltLoaded;

				%player.hasMag[%slot] = "";
				%player.currColtMagazine = 0;
				%player.coltLoaded = false;

				$ColtDropInfo = %clip SPC %bullets SPC %loaded;
			}
			else if ( nameToID(ColtPistolMagazineItem) == %item ) {
				$ColtMagDropInfo = %player.mag[%slot];
				%player.mag[%slot] = "";
			}

			parent::serverCmdDropTool(%client, %slot);
		}
	}

	function ItemData::onAdd(%this, %obj) {
		parent::onAdd(%this, %obj);

		if ( %this == nameToID(ColtPistolItem) && $ColtDropInfo !$= "" ) {
			%obj.hasMag = getWord($ColtDropInfo, 0);
			%obj.currColtMagazine = getWord($ColtDropInfo, 1);
			%obj.coltLoaded = getWord($ColtDropInfo, 2);

			$ColtDropInfo = "";
		}
		else if ( %this == nameToID(ColtPistolMagazineItem) ) {
			%obj.mag = $ColtMagDropInfo;

			$ColtMagDropInfo = "";
		}
	}

	function Player::pickUp(%player, %item) {
		%db = %item.getDatablock();
		%client = %player.client;

		if ( nameToID(ColtPistolItem) == %db && %item.canPickup ) {
			%newslot = %player.addItem(ColtPistolItem.getID());
			if ( %item.coltLoaded !$= "" ) {
				%player.coltLoaded = %item.coltLoaded;
				%player.currColtMagazine = %item.currColtMagazine;
				%player.hasMag[%newslot] = %item.hasMag;
			}
			else {
				%player.coltLoaded = false;
				%player.currColtMagazine = 0;
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
		else if ( nameToID(ColtPistolMagazineItem) == %db ) {
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
if ( isPackage(ColtPistolPackage) ) {
	deactivatepackage(ColtPistolPackage);
}
activatePackage(ColtPistolPackage);
