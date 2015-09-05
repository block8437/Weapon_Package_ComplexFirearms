datablock AudioProfile(HEGrenadePinOutSound)
{
	filename    = "./Sounds/pinOut.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(HEGrenadeBounceSound)
{
	filename    = "./Sounds/grenadeBounce.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(HEGrenadeClickSound)
{
	filename = "base/data/sound/clickSuperMove.wav";
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
	projectileShapeName = "./shapes/weapons/grenade_projectile.dts";
	directDamage        = 0;
	directDamageType = $DamageType::RocketDirect;
	radiusDamageType = $DamageType::RocketRadius;
	impactImpulse	   = 200;
	verticalImpulse	   = 200;
	explosion           = "";//HEGrenadeExplosion;
	particleEmitter     = "";
	sound				= "";

	brickExplosionRadius = 10;
	brickExplosionImpact = false;
	brickExplosionForce  = 25;
	brickExplosionMaxVolume = 100;
	brickExplosionMaxVolumeFloating = 60;

	muzzleVelocity      = 20;
	velInheritFactor    = 0.5;
	explodeOnDeath		= true;

	armingDelay         = 5000; 
	lifetime            = 5000;
	fadeDelay           = 5000;
	bounceElasticity    = 0.4;
	bounceFriction      = 0.3;
	isBallistic         = true;
	gravityMod = 1;

	hasLight    = false;
	lightRadius = 1.0;
	lightColor  = "1 0 0";

	uiName = "HE Grenade Projectile";
};

datablock ProjectileData(HEExplosionProjectile) {
	directDamageType	= $DamageType::RocketDirect;
	radiusDamageType	= $DamageType::RocketRadius;
	explosion 			= HEGrenadeExplosion;
	particleEmitter     = "";
	sound				= "";

	brickExplosionRadius = 10;
	brickExplosionImpact = false;
	brickExplosionForce  = 25;
	brickExplosionMaxVolume = 100;
	brickExplosionMaxVolumeFloating = 60;
	velInheritFactor    = 1;
	explodeOnDeath		= true;

	armingDelay         = 0; 
	lifetime            = 0;
	fadeDelay           = 0;
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

	grenadeLifeTime = 5000;
};

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
	stateTimeoutValue[3]			= 0.6;
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
};

function HEGrenadeImage::onCharge(%this, %obj, %slot) {
	%obj.playthread(2, "spearReady");
	//%obj.setImageLoaded(0, 1);
}

function HEGrenadeImage::onAbortCharge(%this, %obj, %slot) {
	%obj.playthread(2, "root");
}

function HEGrenadeImage::onFire(%this, %obj, %slot) {
	%obj.playthread(2, "spearThrow");
	// Parent::OnFire(%this, %obj, %slot);
	%currslot = %obj.currTool;
	if(%obj.liveGrenadeSlot !$= "") {
		%currslot = %obj.liveGrenadeSlot;
	}
	%obj.tool[%currslot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currslot,0);
	%obj.setImageLoaded(0, 1);
	if(!isEventPending(%obj.HEGrenadeSchedule)) {
		%noboom = true;
	}
	cancel(%obj.HEGrenadeSchedule);

	%vector = %obj.getEyeVector();
	%velocity = vectorScale(%vector, HEGrenadeProjectile.muzzleVelocity);
	%inherit = vectorScale(%obj.getVelocity(), HEGrenadeProjectile.velInheritFactor);
	%velocity = vectorAdd(%velocity, %inherit);
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
		%delay = (%this.item.grenadeLifeTime/1000) - ($Sim::Time - %obj.HEGrenadeStarted);
		%projectile.lastActivated = %obj.HEGrenadeStarted;
		%projectile.schedule(mClamp(%delay * 1000, 0, %this.item.grenadeLifeTime), explode);
	}
	%obj.HEGrenadeStarted = "";
	%obj.liveGrenadeSlot = "";
	%obj.liveGrenadeClient = "";
	serverCmdUnUseTool(%obj.client);
}

function HEGrenadeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	%obj.bounces += 1;
	serverPlay3D(HEGrenadeBounceSound,%obj.getTransform());
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	if(%obj.bounces >= 5 || VectorLen(%obj.getVelocity()) <= 3) {
		%this.MakeItem(%obj);
		%obj.delete();
	}
}

function HEGrenadeProjectile::MakeItem(%this, %obj) {
	%item = new item() {
		dataBlock = HEGrenadeItem;
		sourceObject = %obj;
		client = %obj.client;
		lastActivated = %obj.lastActivated;
	};
	%item.setTransform(%obj.getTransform());
	%item.setVelocity(%obj.getVelocity());
	if(%item.lastActivated !$= "") {
		%item.hideNode("ring");
		schedule(mClamp(((HEGrenadeItem.grenadeLifeTime/1000) - ($Sim::Time - %item.lastActivated)) * 1000, 0, HEGrenadeItem.grenadeLifeTime), 0, HEGrenadeBlowUp, %item);
		return;
	}
	%item.schedule(14000, fadeOut);
	%item.schedule(15000, delete);
}

function HEGrenadeProjectile::onExplode(%this, %obj) {
	if(%obj.noExplode) {
		%this.MakeItem(%obj);
		return;
	}
	%projectile = new projectile() {
		dataBlock = HEExplosionProjectile;
		initialPosition = %obj.getPosition();
		initialVelocity = %obj.getVelocity();
		sourceObject = %obj;
		client = %obj.client;
	};
	MissionCleanup.add(%projectile);
	%projectile.explode();
	// %obj.delete();
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
		dataBlock = HEExplosionProjectile;
		initialPosition = vectorAdd(%obj.getPosition(), "0 0 2");
		initialVelocity = %obj.getVelocity();
		sourceObject = %obj;
		client = %obj.liveGrenadeClient !$= "" ? %obj.liveGrenadeClient : %obj.client;
	};
	MissionCleanup.add(%projectile);
	%projectile.explode();
}

function HEGrenadeBlowUp(%obj) {
	if(isObject(%obj) && %obj.GetType() & $TypeMasks::ItemObjectType) {
		%projectile = new projectile() {
			dataBlock = HEExplosionProjectile;
			initialPosition = %obj.getPosition();
			initialVelocity = %obj.getVelocity();
			sourceObject = %obj;
			client = %obj.client;
		};
		MissionCleanup.add(%projectile);
		%projectile.explode();
		%obj.delete();
	}
}

package HEGrenadePackage {
	function Armor::onTrigger(%this, %obj, %slot, %val)
	{
		%image = %obj.getMountedImage(0);
		if(%image == nameToID(HEGrenadeImage) && %slot $= 4) {
			if(%val) {
				if(%obj.getImageState(0) $= "Ready") {
					%obj.playThread(2, "shiftRight");
					%obj.playAudio(2,HEGrenadePinOutSound);
					%obj.setImageLoaded(0, 0); //IT BEGINS.
				}
			}
			else if(%obj.getImageLoaded(0) == 0) {
				%obj.playThread(3, "shiftLeft");
				%obj.playAudio(2, HEGrenadeClickSound);
				%obj.HEGrenadeStarted = $Sim::Time;
				%obj.liveGrenadeSlot = %obj.currTool;
				%obj.HEGrenadeSchedule = %obj.schedule(%image.item.grenadeLifeTime, HEGrenadeBlowUp);
			}
		}
		Parent::onTrigger(%this, %obj, %slot, %val);
	}

	function serverCmdDropTool(%client, %slot) {
		if ( isObject(%obj = %client.player) ) {
			if(%obj.getMountedImage(0) == nameToID(HEGrenadeImage) && %obj.getImageLoaded(0) == 0) {
				%slot = %obj.liveGrenadeSlot;
			}
			%item = %obj.tool[%slot];
			if ( nameToID(HEGrenadeItem) == %item && %obj.HEGrenadeStarted !$= "") {
				$HEGrenadeDropInfo = %obj.HEGrenadeStarted SPC %client;
				%obj.HEGrenadeStarted = "";
				%obj.liveGrenadeSlot = "";
				%obj.liveGrenadeClient = "";
				%obj.setImageLoaded(0, 1);
				cancel(%obj.HEGrenadeSchedule);
			}
		}
		parent::serverCmdDropTool(%client, %slot);
	}

	function Armor::onCollision(%this, %obj, %col, %a, %b, %c, %d, %e, %f) {
		%db = %col.getDatablock();
		%client = %obj.client;
		if ( nameToID(HEGrenadeItem) == %db && %col.canPickup && %col.lastActivated !$= "") {
			if ( miniGameCanDamage(%col,%obj) == 1 ) {
				%newslot = %obj.addItem(HEGrenadeItem.getID());
				%obj.HEGrenadeStarted = %col.lastActivated;
				%obj.liveGrenadeSlot = %newslot;
				%obj.liveGrenadeClient = %col.client;
				serverCmdUseTool(%client, %newslot);
				%obj.HEGrenadeSchedule = %obj.schedule(mClamp(((%db.grenadeLifeTime/1000) - ($Sim::Time - %obj.HEGrenadeStarted)) * 1000, 0, %db.grenadeLifeTime), HEGrenadeBlowUp);
				%obj.setImageLoaded(0, 0);
				%col.delete();
			}
			return;
		}
		Parent::onCollision(%this, %obj, %col, %a, %b, %c, %d, %e, %f);
	}

	function Player::pickUp(%obj, %item) {
		%db = %item.getDatablock();
		%client = %obj.client;
		if ( nameToID(HEGrenadeItem) == %db && %item.canPickup && %item.lastActivated !$= "") {
			%newslot = %obj.addItem(HEGrenadeItem.getID());
			%obj.HEGrenadeStarted = %item.lastActivated;
			%obj.liveGrenadeSlot = %newslot;
			%obj.liveGrenadeClient = %item.client;
			serverCmdUseTool(%client, %newslot);
			%obj.HEGrenadeSchedule = %obj.schedule(mClamp(((%db.grenadeLifeTime/1000) - ($Sim::Time - %obj.HEGrenadeStarted)) * 1000, 0, %db.grenadeLifeTime), HEGrenadeBlowUp);
			%obj.setImageLoaded(0, 0);
			%item.delete();
		}
		else {
			return parent::pickUp(%obj, %item);
		}
	}

	function ItemData::onAdd(%this, %obj) {
		parent::onAdd(%this, %obj);
		
		if ( %this == nameToID(HEGrenadeItem) && $HEGrenadeDropInfo !$= "" ) {
			%obj.hideNode("ring");
			%obj.lastActivated = getWord($HEGrenadeDropInfo, 0);
			%obj.client = getWord($HEGrenadeDropInfo, 1);
			schedule(mClamp(((%this.grenadeLifeTime/1000) - ($Sim::Time - $HEGrenadeDropInfo)) * 1000, 0, %this.grenadeLifeTime), 0, HEGrenadeBlowUp, %obj);

			$HEGrenadeDropInfo = "";
		}
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
};

activatePackage("HEGrenadePackage");