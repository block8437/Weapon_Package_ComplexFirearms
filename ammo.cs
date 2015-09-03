datablock ItemData(Shell357Item) {
	shapeFile = "./shapes/items/357_shell.dts";
	rotate = false;
	mass = 0.5;
	density = 0.1;
	elasticity = 0.4;
	friction = 0.2;
	emap = true;

	ammoType = "357";
	uiName = "";
	canPickUp = false;

	hasShellSounds = true;
};

datablock ItemData(Bullet357Item) {
	shapeFile = "./shapes/items/357_bullet.dts";
	rotate = false;
	mass = 0.5;
	density = 0.1;
	elasticity = 0.4;
	friction = 0.2;
	emap = true;

	ammoType = "357";
	uiName = "Ammo: Bullet 357";
	canPickUp = true;
	isAmmo = true;
	hasShellSounds = true;
};

function Bullet357Item::UpdateAmmoPrint(%this, %obj, %slot, %tog, %ammo) {
	%client = %obj.client;

	if ( %tog < 0 ) {
		%client.centerprint("", 0, 1);
		return;
	}

	if ( isObject(%image = %obj.getMountedImage(0)) && %image == nameToID(revolverImage) && !%ammo ) {
		%color[0] = "<color:222222>";//grey
		%color[1] = "<color:FF6622>";//orange
		%color[2] = "<color:FFFF00>";//yellow

		%colorSelected[0] = "<color:888888>";//no bullet
		%colorSelected[1] = "<color:FFCC77>";//spent
		%colorSelected[2] = "<color:FFFFDD>";//usable

		%currSlot = %obj.currRevolverSlot;
		%currBullet = %obj.revolverBullet[%currSlot];

		for ( %i = 0; %i <= 5; %i++ ) {
			%start[%i] = %color[%obj.revolverBullet[%i]];

			if ( %i == %currSlot ) {
				%start[%i] = "<shadow:0:3>" @ %start[%i];
				%end[%i] = "<shadow:0:0>";
			}
		}
		%str = "<br><br><br><just:center><font:Arial:35><shadowcolor:FFFFFF>" @
				%start[5] @ "O" @ %end[5] @ " " @ %start[4] @ "O" @ %end[4] @ "<br>"@ %start[0] @"O" @ %end[0] @ "     " @ %start[3] @ "O" @ %end[3] @ "<br>" @ %start[1] @"O" @ %end[1] @ " " @ %start[2] @ "O" @ %end[2];
	}

	for ( %i = 0; %i < %obj.bullets["357"]; %i++ ) {
		%bullets = %bullets @ "|";
	}

	//nightmare fuel below caution
	%client.centerprint("<just:left><font:Arial:17>\c3357:" @ %bullets @ %str, %tog, 1);
}

datablock ItemData(Shell45Item) {
	shapeFile = "./shapes/items/45_shell.dts";
	rotate = false;
	mass = 0.5;
	density = 0.1;
	elasticity = 0.4;
	friction = 0.2;
	emap = true;

	ammoType = "45";
	uiName = "";
	canPickUp = false;

	hasShellSounds = true;
};

datablock ItemData(Bullet45Item) {
	shapeFile = "./shapes/items/45_bullet.dts";
	rotate = false;
	mass = 0.5;
	density = 0.1;
	elasticity = 0.4;
	friction = 0.2;
	emap = true;

	ammoType = "45";
	uiName = "Ammo: Bullet 45";
	canPickUp = true;
	isAmmo = true;
	hasShellSounds = true;
};

function Bullet45Item::UpdateAmmoPrint(%this, %obj, %slot, %tog) {
	%client = %obj.client;

	// if ( %tog < 0 ) {
	// 	%client.centerprint("", 0, 1);
	// 	return;
	// }

	for ( %i = 0; %i < %obj.bullets["45"]; %i++ ) {
		%bullets = %bullets @ "|";
	}

	if ( isObject(%clip = %obj.getMountedImage(0)) && %clip.isMag && %clip.ammoType $= "45" ) {
		%bullets = %bullets @ "<br><color:ffa500>[<color:ffff00>";

		for ( %i = 0; %i < %clip.maxClip; %i++) {
			if ( %i == %obj.mag[%obj.currTool] ) {
				%bullets = %bullets @ "<color:333333>";
			}

			%bullets = %bullets @ "|";
		}

		%bullets = %bullets @ "<color:ffa500>]";
	}

	//nightmare fuel below caution
	%client.centerprint("<just:left><font:Arial:17>\c345:" @ %bullets, %tog, 1);
}

datablock ItemData(Shell30Item) {
	shapeFile = "./shapes/items/30-06_casing.dts";
	rotate = false;
	mass = 0.5;
	density = 0.1;
	elasticity = 0.4;
	friction = 0.2;
	emap = true;

	ammoType = "30-06";
	uiName = "";
	canPickUp = false;

	hasShellSounds = true;
};

datablock ItemData(Bullet30Item) {
	shapeFile = "./shapes/items/30-06_bullet.dts";
	rotate = false;
	mass = 0.5;
	density = 0.1;
	elasticity = 0.4;
	friction = 0.2;
	emap = true;

	ammoType = "30-06";
	uiName = "Ammo: Bullet 30-06";
	canPickUp = true;
	isAmmo = true;
	hasShellSounds = true;
};

function Bullet30Item::UpdateAmmoPrint(%this, %obj, %slot, %tog) {
	%client = %obj.client;

	// if ( %tog < 0 ) {
	// 	%client.centerprint("", 0, 1);
	// 	return;
	// }

	for ( %i = 0; %i < %obj.bullets["30-06"]; %i++ ) {
		%bullets = %bullets @ "|";
	}

	if ( isObject(%clip = %obj.getMountedImage(0)) && %clip.isMag && %clip.ammoType $= "30-06" ) {
		%bullets = %bullets @ "<br><color:ffa500>[";

		for ( %i = 0; %i < %clip.maxClip; %i++) {
			if ( %i < %obj.mag[%obj.currTool] ) {
				%bullets = %bullets @ "<color:ffff00>|";
			}
			else {
				%bullets = %bullets @ "<color:333333>|";
			}
		}

		%bullets = %bullets @ "<color:ffa500>]";
	}

	//nightmare fuel below caution
	%client.centerprint("<just:left><font:Arial:17>\c330-06:" @ %bullets, %tog, 1);
}

datablock ItemData(ShellBuckshotItem) {
	shapeFile = "./shapes/items/buckshot_spent.dts";
	rotate = false;
	mass = 0.5;
	density = 0.1;
	elasticity = 0.4;
	friction = 0.2;
	emap = true;

	ammoType = "buckshot";
	uiName = "";
	canPickUp = false;

	hasShellSounds = true;
};

datablock ItemData(BulletBuckshotItem) {
	shapeFile = "./shapes/items/buckshot.dts";
	rotate = false;
	mass = 0.5;
	density = 0.1;
	elasticity = 0.4;
	friction = 0.2;
	emap = true;

	ammoType = "buckshot";
	uiName = "Ammo: Bullet Buckshot";
	canPickUp = true;
	isAmmo = true;
	hasShellSounds = true;
};

function BulletBuckshotItem::UpdateAmmoPrint(%this, %obj, %slot, %tog) {
	%client = %obj.client;

	// if ( %tog < 0 ) {
	// 	%client.centerprint("", 0, 1);
	// 	return;
	// }

	for ( %i = 0; %i < %obj.bullets["buckshot"]; %i++ ) {
		%bullets = %bullets @ "|";
	}

	if ( isObject(%clip = %obj.getMountedImage(0)) && %clip.item.ammoType $= "buckshot" ) {
		%bullets = %bullets @ "<br><color:ffa500>[";

		for ( %i = 0; %i < %clip.item.maxmag; %i++) {
			if ( %i < %obj.currRemmingtonHold ) {
				%bullets = %bullets @ "<color:ffff00>|";
			}
			else {
				%bullets = %bullets @ "<color:333333>|";
			}
		}

		%bullets = %bullets @ "<color:ffa500>]";
	}

	//nightmare fuel below caution
	%client.centerprint("<just:left><font:Arial:17>\c3Buckshot:" @ %bullets, %tog, 1);
}


function Item::monitorShellVelocity(%this, %before) {
	cancel(%this.monitorShellVelocity);

	%now = vectorLen(%this.getVelocity());
	%delta = %before - %now;

	if ( %delta >= 2 ) {
		%sound = advReloadCasingSound @ getRandom(1, 3);
		serverPlay3D(%sound, %this.getPosition());
	}

	%this.monitorShellVelocity = %this.schedule(50, monitorShellVelocity, %now);
}

package fancyBulletsPackage {
	function ItemData::onAdd(%this, %obj) {
		parent::onAdd(%this, %obj);

		if ( %this.hasShellSounds ) {
			%obj.monitorShellVelocity();
		}
	}
};
activatePackage(fancyBulletsPackage);
