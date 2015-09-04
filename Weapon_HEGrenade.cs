datablock AudioProfile(HEGrenadePinOutSound)
{
	filename    = "./Sounds/pinOut.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock ParticleData(HEGrenadeExplosionParticle)
{
	dragCoefficient		= 1.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= -0.2;
	inheritedVelFactor	= 0.1;
	constantAcceleration	= 0.0;
	lifetimeMS		= 4000;
	lifetimeVarianceMS	= 3990;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/cloud";

	// Interpolation variables
	colors[0]	= "0.2 0.2 0.2 1.0";
	colors[1]	= "0.25 0.25 0.25 0.2";
	colors[2]	= "0.4 0.4 0.4 0.0";

	sizes[0]	= 2.0;
	sizes[1]	= 10.0;
	sizes[2]	= 13.0;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(HEGrenadeExplosionEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	lifeTimeMS	   = 21;
	ejectionVelocity = 10;
	velocityVariance = 1.0;
	ejectionOffset   = 1.0;
	thetaMin         = 0;
	thetaMax         = 0;
	phiReferenceVel  = 0;
	phiVariance      = 90;
	overrideAdvance = false;
	particles = "HEGrenadeExplosionParticle";
};

datablock ExplosionData(HEGrenadeExplosion) {
	explosionShape = "Add-Ons/Weapon_Rocket_Launcher/explosionsphere1.dts";
	lifeTimeMS = 150;

	soundProfile = rocketExplodeSound;
	
	//emitter[1] = "";
	//emitter[2] = "";
	//emitter[0] = "";

	particleEmitter = HEGrenadeExplosionEmitter;
	particleDensity = 10;
//   particleDensity = 0;
	particleRadius = 1.0;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "7.0 8.0 7.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 15.0;

	// Dynamic light
	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";

	//impulse
	impulseRadius = 17;
	impulseForce = 4000;

	//radius damage
	damageRadius = 16;
	radiusDamage = 200;
};

datablock ProjectileData(HEGrenadeProjectile) {
	projectileShapeName = "./shapes/weapons/grenade.dts";
	directDamage        = 0;
	directDamageType = $DamageType::RocketDirect;
	radiusDamageType = $DamageType::RocketRadius;
	impactImpulse	   = 200;
	verticalImpulse	   = 200;
	explosion           = HEGrenadeExplosion;
	particleEmitter     = "";
	sound				= "";

	brickExplosionRadius = 10;
	brickExplosionImpact = false;
	brickExplosionForce  = 25;
	brickExplosionMaxVolume = 100;
	brickExplosionMaxVolumeFloating = 60;

	muzzleVelocity      = 35;
	velInheritFactor    = 1;
	explodeOnDeath = true;

	armingDelay         = 3000; 
	lifetime            = 3000;
	fadeDelay           = 3000;
	bounceElasticity    = 0.4;
	bounceFriction      = 0.3;
	isBallistic         = true;
	gravityMod = 1;

	hasLight    = true;
	lightRadius = 1.0;
	lightColor  = "1 0 0";

	uiName = "HE Grenade Projectile";
};

//////////
// item //
//////////
datablock ItemData(HEGrenadeItem) {
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./shapes/weapons/grenade.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "HE Grenade";
	// iconName = "./shapes/weapons/icons/grenade";
	doColorShift = false;
	colorShiftColor = "0.400 0.196 0 1.000";

	image = HEGrenadeImage;
	canDrop = true;
};

function HEGrenadeItem::onAdd(%this, %obj) {
	parent::onAdd(%this, %obj);

	if ( $HEGrenadeDropInfo !$= "" ) {
		%obj.hideNode("ring");
		%obj.explosionTimer = $HEGrenadeDropInfo;
		%obj.schedule(%obj.explosionTimer, BlowUp);

		$HEGrenadeDropInfo = "";
	}
}

function HEGrenadeItem::BlowUp(%data, %obj) {
	%projectile = new projectile() {
		dataBlock = HEGrenadeProjectile;
		initialPosition = %obj.getPosition();
		initialVelocity = %obj.getVelocity();
		sourceObject = %obj;
	};
	MissionCleanup.add(%projectile);
	%projectile.explode();
	%obj.delete();
}

datablock ShapeBaseImageData(HEGrenadeImage) {
	// Basic Item properties
	shapeFile = "./shapes/weapons/grenade.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0 0";
	//eyeOffset = "0.1 0.2 -0.55";

	correctMuzzleVector = true;

	className = "WeaponImage";

	// Projectile && Ammo.
	item = HEGrenadeItem;
	ammo = " ";
	projectile = HEGrenadeProjectile;
	projectileType = Projectile;
	melee = false;
	armReady = true;

	//casing = " ";
	doColorShift = false;
	colorShiftColor = "0.400 0.196 0 1.000";

	// Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.2;
	stateTransitionOnNotLoaded[0]	= "Nopin";
	stateTransitionOnTimeout[0]		= "Ready";
	stateSequence[0]				= "activate";
	// stateSound[0]					= "";

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Charge";
	stateTransitionOnNotLoaded[1]	= "Nopin";
	stateAllowImageChange[1]		= true;

	stateName[2]					= "Nopin";
	stateTransitionOnTriggerDown[2]	= "Charge";
	stateSequence[2]				= "noring";
	stateAllowImageChange[2]		= false;

	stateName[3]					= "Charge";
	stateTransitionOnTimeout[3]		= "Armed";
	stateTimeoutValue[3]			= 0.7;
	stateWaitForTimeout[3]			= false;
	stateTransitionOnTriggerUp[3]	= "AbortCharge";
	stateScript[3]					= "onCharge";
	stateAllowImageChange[3]		= false;
	
	stateName[4]					= "AbortCharge";
	stateTransitionOnTimeout[4]		= "Ready";
	stateTimeoutValue[4]			= 0.3;
	stateWaitForTimeout[4]			= true;
	stateScript[4]					= "onAbortCharge";
	stateAllowImageChange[4]		= false;

	stateName[5]					= "Armed";
	stateTransitionOnTriggerUp[5]	= "Fire";
	stateAllowImageChange[5]		= false;

	stateName[6]					= "Fire";
	//stateTransitionOnTimeout[6]	= "Ready";
	stateTimeoutValue[6]			= 0.5;
	stateFire[6]					= true;
	// stateSequence[6]				= "fire";
	stateScript[6]					= "onFire";
	stateWaitForTimeout[6]			= true;
	stateAllowImageChange[6]		= false;


	grenadeLifeTime = 3000;
};

function HEGrenadeImage::onCharge(%this, %obj, %slot) {
	%obj.playthread(2, "spearReady");
	%obj.setImageLoaded(0, 1);
}

function HEGrenadeImage::onAbortCharge(%this, %obj, %slot) {
	%obj.playthread(2, "root");
}

function HEGrenadeImage::onFire(%this, %obj, %slot) {
	%obj.playthread(2, "spearThrow");
	// Parent::OnFire(%this, %obj, %slot);

	%obj.tool[%obj.currTool] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%obj.currTool,0);
	%obj.setImageLoaded(0, 1);
	if(!isEventPending(%obj.HEGrenadeSchedule)) {
		%noboom = true;
	}
	cancel(%obj.HEGrenadeSchedule);

	%vector = %obj.getEyeVector();
	%velocity = vectorScale(%vector, HEGrenadeProjectile.muzzleVelocity);
	%velocity = vectorAdd(%velocity, vectorScale(HEGrenadeProjectile.velInheritFactor, %obj.getVelocity()));
	%projectile = new projectile()
	{
		datablock = HEGrenadeProjectile;

		initialVelocity = %velocity;
		initialPosition = %obj.getEyePoint();

		sourceObject = %obj;
		sourceSlot = %slot;

		client = %obj.client;
	};
	MissionCleanup.add(%projectile);
	if(%noboom == true) {
		%projectile.noExplode = true;
	}
	else {
		%delay = 3 - ($Sim::Time - %obj.HEGrenadeStarted);
		talk(%delay);
		%projectile.schedule(mClamp(%delay, 0, 3) * 1000, explode);
	}
	%obj.HEGrenadeStarted = "";
	serverCmdUnUseTool(%obj.client);
}

function HEGrenadeProjectile::Explode(%this, %obj) {
	talk("This is a thing");
	parent::Explode(%this, %obj);
}

function Player::HEGrenadeBlowUp(%obj) {
	// if(%obj.getMountedImage(0) != nameToID(HEGrenadeImage)) {
	// 	return;
	// }
	%obj.tool[%obj.currTool] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%obj.currTool,0);
	%obj.setImageLoaded(0, 1);
	serverCmdUnUseTool(%obj.client);
	%projectile = new projectile() {
		dataBlock = HEGrenadeProjectile;
		initialPosition = %obj.getHackPosition();
		initialVelocity = %obj.getVelocity();
		sourceObject = %obj;
		client = %obj.client;
	};
	MissionCleanup.add(%projectile);
	%projectile.explode();
}

package HEGrenadePackage {
	function Armor::onTrigger(%this, %obj, %slot, %val)
	{
		%image = %obj.getMountedImage(0);
		if(%image == nameToID(HEGrenadeImage) && %slot $= 4 && %val) {
			talk(%obj.getImageState(0));
			if(%obj.getImageState(0) $= "Ready") {
				%obj.playThread(2, "shiftRight");
				%obj.playAudio(2,HEGrenadePinOutSound);
				%obj.setImageLoaded(0, 0); //Shit's live, throw it or die!
				%obj.HEGrenadeStarted = $Sim::Time;
				%obj.HEGrenadeSchedule = %obj.schedule(%image.grenadeLifeTime, HEGrenadeBlowUp);
			}
		}
		Parent::onTrigger(%this, %obj, %slot, %val);
	}

	function serverCmdDropTool(%client, %slot) {
		if ( isObject(%player = %client.player) ) {
			%item = %player.tool[%slot];

			if ( nameToID(HEGrenadeItem) == %item ) {
				$HEGrenadeDropInfo = %player.explosionTimer[%slot];
				%player.explosionTimer[%slot] = "";
				%player.setImageLoaded(0, 1);
				cancel(%player.HEGrenadeSchedule);
			}
		}
		parent::serverCmdDropTool(%client, %slot);
	}

	function serverCmdUseTool(%client, %slot) {
		%obj = %client.player;
		if(isObject(%obj) && %obj.getMountedImage(0) == nameToID(HEGrenadeImage) && %obj.getImageLoaded(0) == 0) {
			return;
		}
		parent::serverCmdUseTool(%client, %slot);
	}

	function serverCmdUnuseTool(%client, %slot) {
		%obj = %client.player;
		if(isObject(%obj) && %obj.getMountedImage(0) == nameToID(HEGrenadeImage) && %obj.getImageLoaded(0) == 0) {
			return;
		}
		parent::serverCmdUnuseTool(%client, %slot);
	}

	function HEGrenadeProjectile::radiusDamage(%this, %obj, %col, %factor, %pos, %damage) {
		if (obstructRadiusDamageCheck(%pos, %col)) {
			Parent::radiusDamage(%this, %obj, %col, %factor, %pos, %damage);
		}
	}

	function HEGrenadeProjectile::radiusImpulse(%this, %obj, %col, %factor, %pos, %force) {
		if (obstructRadiusDamageCheck(%pos, %col)) {
			Parent::radiusImpulse(%this, %obj, %col, %factor, %pos, %force);
		}
	}
};

activatePackage("HEGrenadePackage");