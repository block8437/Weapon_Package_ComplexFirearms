package ComplexFirearmsPackage {
	function Player::activateStuff(%this) {
		%client = %this.client;

		if ( !isObject(%client) ) {
			return;
		}

		if ( isObject(%clip = %this.getMountedImage(1)) && %clip.isMag ) {
			if ( %this.bullets[%clip.ammoType] > 0 && %this.holdingPistolMagazine < %clip.maxClip ) {
				%this.holdingPistolMagazine += 1;
				%this.bullets[%clip.ammoType] -= 1;
			}

			%clip.ammoItem.UpdateAmmoPrint(%this, "", 3, 1);
			%this.pistolLoaded = false;
			%this.setImageAmmo(0, 1);
			return;
		}

		Parent::activateStuff(%this);

		%start = %this.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%this.getEyeVector(), 6));

		%mask = $TypeMasks::All | (~$TypeMasks::FxBrickAlwaysObjectType); //excludes FxBrickAlwaysObjectType from TypeMasks
		%ray = containerRayCast(%start, %end, %mask, %this);

		if ( !%ray ) return;

		%pos = getWords(%ray, 1, 3);
		initContainerRadiusSearch(
			%pos, 0.2,
			$TypeMasks::ItemObjectType
		);

		while ( isObject( %col = containerSearchNext()) ) {
			if ( %col.getDatablock().canPickUp ) {
				if ( %col.getDataBlock().isAmmo ) {
					if ( %this.bullets[%col.getDataBlock().ammotype] >= 32 )
						return;

					%this.bullets[%col.getDataBlock().ammotype]++;

					if ( %this.getImageState(0) $= "OpenBarrel") {
						%col.getDatablock().UpdateAmmoPrint(%this, "");
					}
					else {
						%col.getDatablock().UpdateAmmoPrint(%this, "", 3, 1);
					}

					%sound = "advReloadInsert" @ getRandom(1, 2) @ "Sound";
					serverPlay3d(%sound, %col.getPosition());

					if ( isObject(%col.spawnBrick) ) {
						%col.fadeOut();
						%col.schedule(%col.spawnBrick.itemRespawnTime, fadeIn);
					}
					else {
						%col.delete();
					}
				}
				else if ( %col.getDataBlock().isMag ) {
					%slot = %this.addItem(%col.getDataBlock().getID());
					if ( %slot > -1 ) {
						%this.mag[%slot] = (%col.mag $= "" ? %col.getDataBlock().image.maxClip : %col.mag);

						if ( isObject(%col.spawnBrick) ) {
							%col.fadeOut();
							%col.schedule(%col.spawnBrick.itemRespawnTime, fadeIn);
						}
						else {
							%col.delete();
						}
						return;
					}
				}
			}
		}
	}

	function Armor::onTrigger(%this, %obj, %trig, %tog) {
		Parent::onTrigger(%this, %obj, %trig, %tog);

		%image = %obj.getMountedImage(0);
		if ( %image.isMag ) {
			if ( %trig == 0 ) {
				if ( %tog ) {
					%obj.leftClickTog = true;
					%obj.AmmoTriggerLoop(%image, 1);
				}
				else {
					%obj.leftClickTog = false;
				}
			}
			else if ( %trig == 4 ) {
				if ( %tog ) {
					%obj.rightClickTog = true;
					%obj.AmmoTriggerLoop(%image, -1);
				}
				else {
					%obj.rightClickTog = false;
				}
			}
		}
	}

	function serverCmdUseTool(%client, %slot) {
		parent::serverCmdUseTool(%client, %slot);
		if ( isObject(%player = %client.player) ) {
			%tool = %player.tool[%slot];
			if ( %tool.isMag ) {
				%image = %tool.image;
				if ( %player.mag[%slot] $= "" ) {
					%player.mag[%slot] = %image.maxClip;
				}
				%image.ammoItem.UpdateAmmoPrint(%player, 0, 1);
			}
		}
	}

	function Armor::onDisabled(%this, %obj, %disabled) {
		if ( %obj.bullets["357"] > 0 && $MMDropBulletsOnDeath ) {
			for ( %i = 0; %i < %obj.bullets["357"]; %i++ ) {
				%datablock = Bullet357Item;
				%item = new Item() {
					dataBlock = %datablock;
					position = %obj.getEyePoint();
				};

				%spread = 30;
				%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
				%spread = vectorScale(%scalars, mDegToRad(%spread / 2));
				%vector = "0 0 10";
				%matrix = matrixCreateFromEuler(%spread);
				%vel = matrixMulVector(%matrix, %vector);
				%item.setVelocity(%vel);
				%position = getWords(%item.getTransform(), 0, 2);
				%item.setTransform(%position SPC eulerToAxis("0 0" SPC getRandom() * 360 - 180));

				if ( !isObject(BulletGroup) ) {
					MissionCleanup.add(new SimGroup(BulletGroup));
				}

				BulletGroup.add(%item);

				if ( !$DefaultMinigame.MMGame ) {
					%item.schedule(14000, fadeOut);
					%item.schedule(15000, delete);
				}
			}
		}
		parent::onDisabled(%this, %obj, %disabled);
	}
};
if ( isPackage(ComplexFirearmsPackage) )
	deactivatepackage(ComplexFirearmsPackage);
activatePackage(ComplexFirearmsPackage);

function Player::AmmoTriggerLoop(%this, %image, %val, %delay) { //This is the loop that lets you hold down left click to fill up the magazine.
	cancel(%this.ammoTriggerLoop);
	if ( %this.getMountedImage(0) != %image ) {
		return;
	}
	if ( !%this.leftClickTog && !%this.rightClickTog ) {
		return;
	}
	if ( %delay $= "") {
		%delay = 300;
	}

	%mag = %this.mag[%this.currTool];
	%pool = %this.bullets[%image.ammoType];

	if ( %val > 0 ) {
		if ( %pool <= 0 || %mag >= %image.maxClip ) {
			%image.ammoItem.UpdateAmmoPrint(%this, 0, 1);
			return;
		}
	}
	else {
		if ( %mag <= 0 ) {
			%image.ammoItem.UpdateAmmoPrint(%this, 0, 1);
			return;
		}
	}

	%this.mag[%this.currTool] += %val;
	%this.bullets[%image.ammoType] -= %val;
	if ( isObject( %this.client ) ) {
		%sound = "advReloadInsert" @ getRandom(1, 2) @ "Sound";
		%this.client.play3D(%sound, %this.getHackPosition());
	}
	%image.ammoItem.UpdateAmmoPrint(%this, 0, 1);

	%delay = getMax(50, %delay - 75);
	%this.ammoTriggerLoop = %this.schedule(%delay, AmmoTriggerLoop, %image, %val, %delay);
}