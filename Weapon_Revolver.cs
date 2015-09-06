exec("./RevolverPackage.cs");

//sounds
datablock AudioProfile(revolverFireSound)
{
	filename    = "./Sounds/revolverPistol.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(revolverCycleSound)
{
	filename    = "./Sounds/revolver_flick.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(revolverSpinSound1)
{
	filename    = "./Sounds/revolver_spin1.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(revolverSpinSound2)
{
	filename    = "./Sounds/revolver_spin2.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(revolverSpinSound3)
{
	filename    = "./Sounds/revolver_spin3.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(revolverSpinSound4)
{
	filename    = "./Sounds/revolver_spin4.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(revolverSpinSound5)
{
	filename    = "./Sounds/revolver_spin5.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(revolverOpenSound)
{
	filename    = "./Sounds/revolver_open.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(revolverCloseSound)
{
	filename    = "./Sounds/revolver_close.wav";
	description = AudioClose3d;
	preload = true;
};



AddDamageType("SnWRevolver",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_snwrevolver> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_snwrevolver> %1',0.2,1);
AddDamageType("SnWRevolverHeadshot",
	'<bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_snwrevolver><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',
	'%2 <bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_snwrevolver><bitmap:add-ons/Weapon_Package_ComplexFirearms/shapes/weapons/icons/ci_headshot> %1',0.2,1);
//muzzle flash effects
datablock ProjectileData(revolverProjectile : gunProjectile)
{
	impactImpulse       = 300;
	verticalImpulse     = 250;

	armingDelay         = 00;
	lifetime            = 4000;
	fadeDelay           = 3500;
	bounceElasticity    = 0.5;
	bounceFriction      = 0.20;
	isBallistic         = false;
	gravityMod = 0.0;

	particleEmitter     = advSmallBulletTrailEmitter; //bulletTrailEmitter;

	uiName = "357 Bullet";
};

//////////
// item //
//////////

datablock ItemData(revolverItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/weapons/S&W_Revolver.dts";
	iconName = "./shapes/weapons/icons/snwrevolver";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "S&W Revolver";
	//iconName = "./icons/icon_Pistol";
	doColorShift = false;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	maxmag = 6;
	ammotype = "357";
	reload = true;

	nochamber = 1;

	 // Dynamic properties defined by the scripts
	image = revolverImage;
	canDrop = true;

	clickPickUp = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(revolverImage)
{
	// Basic Item properties
	shapeFile = "./shapes/weapons/S&W_Revolver.dts";
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
	item = revolverItem;
	ammoItem = Bullet357Item;
	ammo = " ";
	projectile = revolverProjectile;
	projectileType = Projectile;
	customProjectileFire = true;

	timedCustomFire = true;
	fireSpeed = cf_muzzlevelocity_ms(396.24);
	fireGravity = "0 0" SPC cf_bulletdrop_grams(10);
	fireLifetime = 5;
	velInheritFactor = 0.25;
	projectile = revolverProjectile;

	directDamage        = 25;//8;
	directDamageType	= $DamageType::SnWRevolver;
	radiusDamageType	= $DamageType::SnWRevolver;
	headshotDamageType	= $DamageType::SnWRevolverHeadshot;
	headshotMultiplier = 3;

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
	colorShiftColor = revolverItem.colorShiftColor;//"0.400 0.196 0 1.000";

	//casing = " ";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	stateName[0]						= "Activate"; //Initial Startup State
	stateTransitionOnTimeout[0]			= "CheckChamber";
	stateSequence[0]					= "Activate";
	stateTimeoutValue[0]				= 0.15;

	stateName[1]						= "CheckChamber"; //This state makes sure there's a bullet chambered.
	// stateTransitionOnLoaded[1]			= "Ready";
	// stateTransitionOnNotLoaded[1]		= "Empty"; //This is handled in the Ready state.
	stateTransitionOnTimeout[1]			= "Ready";
	stateAllowImageChange[1]			= true;
	stateScript[1]						= "onCheckChamber";

	stateName[2]						= "Ready"; //This state allows you to fire your gun.
	stateTransitionOnTriggerDown[2]		= "Fire";
	stateTransitionOnNotLoaded[2]		= "Empty"; //In case we decide to empty the gun or something.
	stateTransitionOnNoAmmo[2]			= "OpenBarrelSequence"; //onAmmo and onNoAmmo are used for barrel.
	stateSequence[2]					= "root";
	stateAllowImageChange[2]			= true;
	stateScript[2]						= "onReady";

	stateName[3]						= "Empty";
	stateTransitionOnTriggerDown[3]		= "EmptyClick"; //State 8.
	stateTranstionOnLoaded[3]			= "CheckChamber";
	stateTransitionOnNoAmmo[3]			= "OpenBarrelSequence";
	stateSequence[3]					= "noammo";
	stateScript[3]						= "onEmpty";

	stateName[4]						= "Fire";
	stateTimeoutValue[4]				= 0.1;
	stateTransitionOnTimeout[4]			= "Smoke";
	stateFire[4]						= true; //Completely useless atm, but whatever.
	stateAllowImageChange[4]			= false;
	stateSequence[4]					= "fire";
	stateScript[4]						= "onFire";
	stateWaitForTimeout[4]				= true; //This here also makes no sense since there are no other transitions than onTimeout.
	stateEmitter[4]						= advBigBulletFireEmitter;
	stateEmitterTime[4]					= 0.05;
	stateEmitterNode[4]					= "muzzleNode";

	stateName[5]						= "Smoke";
	stateTimeoutValue[5]				= 0.01;
	stateTransitionOnTimeout[5]			= "TriggerCheck"; //Checks if the user released the trigger.
	stateEmitter[5]						= gunSmokeEmitter;
	stateEmitterTime[5]					= 0.05;
	stateEmitterNode[5]					= "muzzleNode";
	stateAllowImageChange[5]			= false;

	stateName[6]						= "TriggerCheck";
	stateTimeoutValue[6]				= 0.1;
	stateTransitionOnTriggerDown[6]		= "HammerClick";
	stateWaitForTimeout[6]				= true; //Now this is how you use it: it won't transition onTriggerUp if timeout value hasn't passed.
	stateTransitionOnNoAmmo[6]			= "OpenBarrelSequence";

	stateName[7]						= "HammerClick"; //Pulls back the hammer for the next shot.
	stateTimeoutValue[7]				= 0.12;
	stateSequence[7]					= "Clickdown";
	stateScript[7]						= "onClickdown";
	stateSound[7]						= revolverCycleSound; //I don't like using stateSound for some reason.
	stateTransitionOnTriggerUp[7]		= "CheckChamber";
	stateWaitForTimeout[7]				= true;
	stateAllowImageChange[7]			= false;

	stateName[8]						= "EmptyClick"; //*click!*
	stateTransitionOnTriggerUp[8]		= "checkChamber";
	stateTimeoutValue[8]				= 0.13;
	stateAllowImageChange[8]			= false;
	stateWaitForTimeout[8]				= true;
	stateSound[8]						= advReloadTap1Sound;
	stateScript[8]						= "onEmptyFire";
	stateSequence[8]					= "emptyFire";

	stateName[9]						= "OpenBarrel";
	//stateTransitionOnTriggerDown[9]		= "InsertBullet";
	stateTimeoutValue[9]				= 0.1;
	stateWaitForTimeout[9]				= true;
	stateTransitionOnAmmo[9]			= "CloseBarrel"; //Close barrel and allow the gun to shoot again.
	stateScript[9]						= "OpenBarrel";
	stateTransitionOnNotLoaded[9]		= "ShellCheck"; //In this "state section" we use onNotLoaded as "remove bullets from gun".

	stateName[10]						= "InsertBullet"; //Not used for now
	stateTransitionOnTriggerUp[10]		= "OpenBarrel";
	stateTimeoutValue[10]				= 0.1;
	stateWaitForTimeout[10]				= true;
	stateScript[10]						= "InsertBullet";

	stateName[11]						= "CloseBarrel";
	stateTransitionOnTimeout[11]		= "CheckChamber";
	stateTimeoutValue[11]				= 0.1;
	stateScript[11]						= "CloseBarrel";
	stateSequence[11]					= "CloseCylinder";

	stateName[12]						= "ShellCheck";
	stateTransitionOnTimeout[12]		= "EjectShells";
	stateTransitionOnLoaded[12]			= "OpenBarrel";
	stateScript[12]						= "onShellCheck";
	stateTimeoutValue[12]				= 0.01;
	stateEjectShell[12]					= false;

	stateName[13]						= "EjectShells";
	stateTransitionOnLoaded[13]			= "StopEject";
	// stateEjectShell[13]					= true; //Replaced with bullet items yay
	stateTransitionOnTimeout[13]		= "ShellCheck";
	stateTimeoutValue[13]				= 0.01;
	stateScript[13]						= "onEjectShells";
	stateSequence[13]					= "EjectShell";

	stateName[14]						= "OpenBarrelSequence";
	stateSequence[14]					= "OpenCylinder";
	stateTimeoutValue[14]				= 0.1;
	stateTransitionOnTimeout[14]		= "OpenBarrel";

	stateName[15]						= "StopEject";
	stateSequence[15]					= "StopEject";
	stateTransitionOnTimeout[15]		= "OpenBarrel";
	stateTimeoutValue[15]				= 0.1;
};

function revolverImage::onMount(%this, %obj, %slot)
{
	parent::onMount(%this,%obj,%slot);
	if(%obj.currRevolverSlot $= "")
		%obj.currRevolverSlot = 0;
	if(%obj.bullets["357"] $= "")
		%obj.bullets["357"] = 18;
	for(%i = 0; %i <= 5; %i++)
	{
		if(%obj.revolverBullet[%i] $= "")
			%obj.revolverBullet[%i] = 0;
	}
}

function revolverImage::onUnMount(%this, %obj, %slot)
{
	parent::onUnMount(%this,%obj,%slot);
	%this.ammoItem.UpdateAmmoPrint(%obj, %slot, -1);
}

function revolverImage::onClickdown(%this, %obj, %slot)
{
}

function revolverImage::onCheckChamber(%this, %obj, %slot)
{
	if(%obj.revolverBullet[%obj.currRevolverSlot] < 2)
		%obj.setImageLoaded(%slot, 0);
	else
		%obj.setImageLoaded(%slot, 1);
}

function revolverImage::onEmpty(%this, %obj, %slot)
{
	if(%obj.revolverBullet[%obj.currRevolverSlot] < 2)
		%obj.setImageLoaded(%slot, 0);
	else
		%obj.setImageLoaded(%slot, 1);
}

function revolverImage::onEmptyFire(%this, %obj, %slot)
{
	%obj.currRevolverSlot++;
	if(%obj.currRevolverSlot > 5)
	{
		%obj.currRevolverSlot = 0;
	}
}

function revolverImage::getProjectileSpread(%this, %obj, %slot)
{
	%spread = 0.4;
	return %spread; //+ vectorLen(%obj.getVelocity()) * 0.4;
}


function revolverImage::onFire(%this, %obj, %slot)
{
	if(%obj.getDamagePercent() > 1.0)
	{
		return;
	}

	//%obj.revolverSlot[%obj.currRevolverSlot] current bullet/slot
	//%obj.revolverSlot[...] 0 = empty, 1 = spent bullet, 2 = working bullet

	if(%obj.revolverBullet[%obj.currRevolverSlot] < 2)
	{
		serverPlay3d(advReloadTap1Sound, %obj.getHackPosition());
		%obj.setImageLoaded(%slot, 0);
	}
	else
	{
		%obj.playThread(2, shiftRight);
		%obj.playThread(3, shiftLeft);
		%obj.revolverBullet[%obj.currRevolverSlot] = 1;
		// playGunSound(%obj.getMuzzlePoint(%slot));
		serverplay3d(revolverFireSound, %obj.getMuzzlePoint(%slot));
		parent::onFire(%this, %obj, %slot);
	}
	%obj.currRevolverSlot++;
	if(%obj.currRevolverSlot > 5)
	{
		%obj.currRevolverSlot = 0;
	}
}

function revolverImage::InsertBullet(%this, %obj, %slot)
{
	// find the first unoccupied slot after this one
	if(%obj.bullets[%this.item.ammotype] <= 0)
		return;

	for (%i = 0; %i <= 5; %i++)
	{
		if (%obj.revolverBullet[%obj.currRevolverSlot] <= 0)
		{
			// insert a bullet there
			%obj.revolverBullet[%obj.currRevolverSlot] = 2;
			%sound = "advReloadInsert" @ getRandom(1, 2) @ "Sound";
			serverPlay3d(%sound, %obj.getHackPosition());
			%this.ammoItem.UpdateAmmoPrint(%obj, %slot);
			%obj.bullets[%this.item.ammotype]--;
			break;
		}
		// move chamber while searching
		%obj.currRevolverSlot++;
		%obj.currRevolverSlot %= 6;
	}

	// if there's no empty slot we'll end up where we already were
	// so there's no need to update ammo display
}

function revolverImage::onEjectShells(%this, %obj, %slot)
{
	for(%i = 0; %i <= 5; %i++)
	{
		if(%obj.revolverBullet[%i] >= 1)
		{
			%datablock = Shell357Item;
			if(%obj.revolverBullet[%i] == 2)
				%datablock = Bullet357Item;
			%item = new Item()
			{
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
			if(!$DefaultMinigame.MMGame)
			{
				%item.schedule(14000, fadeOut);
				%item.schedule(15000, delete);
			}
			%obj.revolverBullet[%i] = 0;
			if(%dataBlock $= Shell357Item) %item.canPickup = false;
			break;
		}
	}

	for (%i = 0; %i <= 5; %i++)
	{
		if (%obj.revolverBullet[%i] >= 1)
			break;
	}

	%this.ammoItem.UpdateAmmoPrint(%obj, %slot);

	%obj.setImageLoaded(%slot, %i > 5);
	%obj.playThread(2, "shiftRight");
}

function revolverImage::onShellCheck(%this, %obj, %slot)
{
	// no. it's checked in onEjectShells.
}

function revolverImage::OpenBarrel(%this, %obj, %slot)
{
	Bullet357Item.UpdateAmmoPrint(%obj, %slot);
	if(isObject(%obj.heldCorpse))
	{
		%obj.heldCorpse.dismount();
		%obj.heldCorpse.addVelocity(VectorScale(%obj.getForwardVector(),5));
		%obj.heldCorpse.playThread(0,"death1");
		%obj.heldCorpse = 0;
	}
}

function revolverImage::CloseBarrel(%this, %obj, %slot)
{
	%this.ammoItem.UpdateAmmoPrint(%obj, %slot, -1);
}

function revolverImage::damage(%this, %obj, %col, %pos, %normal) {
	%damageType = %this.directDamageType;
	%damage = %this.directDamage;
	
	if (%col.isCrouched() || %col.getRegion(%pos, true) $= "head")
	{
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
