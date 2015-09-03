package FancyRevolverPackage
{
	function serverCmdShiftBrick(%this, %x, %y, %z)
	{
		if(isObject(%obj = %this.player))
		{
			%image = %obj.getMountedImage(0);
			if(%image != nameToID(revolverImage))
			{
				parent::serverCmdShiftBrick(%this, %x, %y, %z);
				return;
			}

			%state = %obj.getImageState(0);
			if(%state $= "OpenBarrel" || %state $= "Ready" || %state $= "Empty" || %state $= "TriggerCheck")
			{
				if(%x == 0 && %y == 1 && %z == 0)
				{
					if($Sim::Time - %obj.lastSpin > 0.15)
					{
						%obj.playThread(2,rotCCW);
						%obj.lastSpin = $Sim::Time;
					}
					%obj.currRevolverSlot++;
					if(%obj.currRevolverSlot > 5)
					{
						%obj.currRevolverSlot = 0;
					}
					if($Sim::Time - %obj.lastSpinSound > 0.07)
					{
						serverPlay3d(revolverSpinSound @ getRandom(1,5), %obj.getHackPosition());
						%obj.lastSpinSound = $Sim::Time;
					}
				}
				else if(%x == 0 && %y == -1 && %z == 0)
				{
					if($Sim::Time - %obj.lastSpin > 0.15)
					{
						%obj.playThread(2, rotCW);
						%obj.lastSpin = $Sim::Time;
					}
					%obj.currRevolverSlot--;
					if(%obj.currRevolverSlot < 0)
					{
						%obj.currRevolverSlot = 5;
					}
					if($Sim::Time - %obj.lastSpinSound > 0.07)
					{
						serverPlay3d(revolverSpinSound @ getRandom(1,5), %obj.getHackPosition());
						%obj.lastSpinSound = $Sim::Time;
					}
				}
				else if(%x == 0 && %y == 0 && %z == -3 && %state $= "OpenBarrel")
				{
					if($Sim::Time - %obj.lastBulletInsert < 0.1)
						return;
					%obj.lastBulletInsert = $Sim::Time;
					%image.InsertBullet(%obj, 0);
				}
				else if(%x == 1 && %y == 0 && %z == 0)
				{
					if(%state $= "Ready" || %state $= "Empty" || %state $= "TriggerCheck")
					{
						// talk("*Open Barrel*");
						%obj.setImageLoaded(0, 1);
						%obj.setImageAmmo(0, 0); //This is for opening barrel
						%obj.playThread(2,shiftLeft);
						%obj.playAudio(2,revolverCloseSound); //Reversed sound effects - they sound better this way
					}
					else if(%state $= "OpenBarrel")
					{
						// talk("*Close Barrel*");
						%obj.setImageAmmo(0, 1); //This is for closing barrel
						%obj.playThread(2,shiftRight);
						%obj.playAudio(2,revolverOpenSound); //Ditto
					}
				}
				else if(%x == -1 && %y == 0 && %z == 0)
				{
					for (%i = 0; %i <= 5; %i++)
					{
						if (%obj.revolverBullet[%i] >= 1)
							break;
					}

					%obj.setImageLoaded(0, %i > 5);
				}
				if(%state $= "OpenBarrel")
					Bullet357Item.UpdateAmmoPrint(%obj, "");
				return;
			}
		}
		parent::serverCmdShiftBrick(%this, %x, %y, %z);
	}

	function serverCmdRotateBrick(%this, %rotation)
	{
		if(isObject(%obj = %this.player))
		{
			%image = %obj.getMountedImage(0);
			if(%image != nameToID(revolverImage))
			{
				parent::serverCmdRotateBrick(%this, %rotation);
				return;
			}

			%state = %obj.getImageState(0);
			if(%state $= "OpenBarrel")
			{
				if(%rotation != 0)
				{
					%chamber = %obj.revolverBullet[%obj.currRevolverSlot];
					if(%chamber > 0)
					{
						%obj.revolverBullet[%obj.currRevolverSlot] = 0;

						if(%chamber == 2)
						{
							%sound = "advReloadInsert" @ getRandom(1, 2) @ "Sound";
							serverPlay3d(%sound, %obj.getHackPosition());
							%obj.bullets["357"]++;
						}

						if(%chamber == 1)
						{
							%item = new Item()
							{
								dataBlock = Shell357Item;

								position = vectorAdd(%obj.getMuzzlePoint(0), vectorScale(%obj.getMuzzleVector(0), -1.5));
							};
							%spread = 15;
							%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
							%spread = vectorScale(%scalars, mDegToRad(%spread / 2));

							%vector = vectorScale(%obj.getEyeVector(), -10);
							%matrix = matrixCreateFromEuler(%spread);
							%vel = matrixMulVector(%matrix, %vector);
							%item.setVelocity(%vel);
							%position = getWords(%item.getTransform(), 0, 2);
							%item.setTransform(%position SPC eulerToAxis("0 0" SPC getRandom() * 360 - 180));
							if (!isObject(BulletGroup)) {
								MissionCleanup.add(new SimGroup(BulletGroup));
							}
							BulletGroup.add(%item);
							%item.monitorShellVelocity();
							if(!$DefaultMinigame.MMGame)
							{
								%item.schedule(14000, fadeOut);
								%item.schedule(15000, delete);
							}
							%item.canPickup = false;
						}
					}
					// else
					// {
					// 	%obj.currRevolverSlot++;
					// 	if(%obj.currRevolverSlot > 5)
					// 	{
					// 		%obj.currRevolverSlot = 0;
					// 	}
					// }
				}
				Bullet357Item.UpdateAmmoPrint(%obj, "");
				return;
			}
		}
		parent::serverCmdRotateBrick(%this, %rotation);
	}

	function Armor::onTrigger(%this, %obj, %trig, %tog)
	{
		Parent::onTrigger(%this, %obj, %trig, %tog);
		%image = %obj.getMountedImage(0);
		if(%image != nameToID(revolverImage))
			return;
		if(%trig == 0) //Click
		{
			if(%tog)
			{
				%obj.revolverAmmoLoop(%image);
			}
			else
			{
				cancel(%obj.revolverAmmoLoop);
			}
		}
		if(%trig == 4) // Jet
		{
			%state = %obj.getImageState(0);
			if(%tog && %state $= "OpenBarrel")
			{
				for (%i = 0; %i <= 5; %i++)
				{
					if (%obj.revolverBullet[%i] >= 1)
						break;
				}

				%obj.setImageLoaded(0, %i > 5);
				// for(%i = 0; %i <= 5; %i++)
				// {
				// 	if(%obj.revolverBullet[%i] >= 1)
				// 		%obj.revolverBulletCount++;
				// 	%obj.revolverBullet[%i] = 0;
				// }
				// Bullet357Item.UpdateAmmoPrint(%obj, "");
			}
		}
	}

	function serverCmdLight(%this)
	{
		if(isObject(%obj = %this.player))
		{
			%image = %obj.getMountedImage(0);
			if(%image == nameToID(revolverImage))
			{
				%state = %obj.getImageState(0);
				if(%state $= "Ready" || %state $= "Empty" || %state $= "TriggerCheck")
				{
					// talk("*Open Barrel*");
					%obj.setImageLoaded(0, 1);
					%obj.setImageAmmo(0, 0); //This is for opening barrel
					%obj.playThread(2,shiftLeft);
					%obj.playAudio(2,revolverCloseSound); //Reversed sound effects - they sound better this way
				}
				else if(%state $= "OpenBarrel")
				{
					// talk("*Close Barrel*");
					%obj.setImageAmmo(0, 1); //This is for closing barrel
					%obj.playThread(2,shiftRight);
					%obj.playAudio(2,revolverOpenSound); //Ditto
				}
				return;
			}
		}
		parent::serverCmdLight(%this);
	}

	function serverCmdDropTool(%this,%slot)
	{
		if(isObject(%obj = %this.player))
		{
			%item = %obj.tool[%obj.currTool];
			if(%item != nameToID(revolverItem))
				return parent::serverCmdDropTool(%this, %slot);
			for(%i = 0; %i <= 5; %i++)
			{
				$array = $array SPC %obj.revolverBullet[%i];
				%obj.revolverBullet[%i] = 0;
			}
		}
		parent::serverCmdDropTool(%this,%slot);
	}

	function ItemData::onAdd(%this,%obj)
	{
		parent::onAdd(%this,%obj);
		if ( $array !$= "" && %this == nameToID(revolverItem) ) {
			for ( %i = 0; %i <= 5; %i++ ) {
				%obj.revolverBullet[%i] = getWord($array, %i+1);
			}
			$array = "";
		}
	}

	function Player::pickUp(%this,%item)
	{
		%data = %item.getDataBlock();
		
		if ( %data == nameToID(revolverItem) && %item.canPickup ) {
			for ( %i = 0; %i <= 5; %i++ ) {
				%this.revolverBullet[%i] = %item.revolverBullet[%i];
			}
		}
		else if ( %data.isAmmo ) {
			return;
		}
		return parent::pickUp(%this,%item);
	}

	function serverCmdSuicide(%this)
	{
		if(isObject(%obj = %this.player))
		{
			%image = %obj.getMountedImage(0);
			%state = %obj.getImageState(0);
			if(%image == nameToID(revolverImage))
			{
				if(%state !$= "Ready" && %state !$= "Empty")
				{
					return;
				}
				if(%obj.revolverBullet[%obj.currRevolverSlot] < 2)
				{
					%obj.playThread(2, plant);
					serverPlay3d(advReloadTap1Sound, %obj.getHackPosition());
				}
				else
				{
					%obj.playThread(2, shiftRight);
					%obj.playThread(3, shiftLeft);
					%obj.revolverBullet[%obj.currRevolverSlot] = 1; //Spent
					serverplay3d(revolverFireSound, %obj.getMuzzlePoint(%slot));
					serverCmdDropTool(%this, %obj.currTool);
					%obj.damage(%obj, %obj.getEyePoint(), %obj.getDataBlock().maxDamage, $DamageType::SnWRevolverHeadshot);
				}
				%obj.currRevolverSlot++;
				if(%obj.currRevolverSlot > 5)
				{
					%obj.currRevolverSlot = 0;
				}
				return;
			}
		}
		parent::serverCmdSuicide(%this);
	}
};
if ( isPackage(FancyRevolverPackage) ) {
	deactivatepackage(FancyRevolverPackage);
}
activatePackage(FancyRevolverPackage);

function Player::revolverAmmoLoop(%this, %image) { //This lets you hold left click to fill up the barrel.
	cancel(%this.revolverAmmoLoop);
	if(%this.getMountedImage(0) != %image) {
		return;
	}
	%state = %this.getImageState(0);
	if(%state !$= "OpenBarrel") {
		return;
	}
	revolverImage.InsertBullet(%this, 0);
	%this.revolverAmmoLoop = %this.schedule(150, revolverAmmoLoop, %image);
}