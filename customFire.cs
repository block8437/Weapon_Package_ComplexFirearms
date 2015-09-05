// by Port

package CustomProjectileFire
{
	function WeaponImage::onFire(%this, %obj, %slot)
	{
		if (!%this.customProjectileFire)
			return Parent::onFire(%this, %obj, %slot);

		if (!isObject(%this.projectile))
		{
			error("ERROR: WeaponImage::onFire() - " @ %this.getName() @ " has invalid projectile '" @ %this.projectile @ "'");
			return 0;
		}

		%obj.hasShotOnce = 1;

		if (%this.minShotTime > 0)
		{
			if (getSimTime() - %obj.lastFireTime < %this.minShotTime)
				return 0;

			%obj.lastFireTime = getSimTime();
		}

		%client = %obj.client;

		if (isObject(%client.miniGame))
		{
			if (getSimTime() - %client.lastF8Time < 3000)
				return 0;
		}

		%muzzlePoint = %obj.getMuzzlePoint(%slot);
		%muzzleVector = %obj.getMuzzleVector(%slot);

		%eyePoint = %obj.getEyePoint();
		%eyeVector = %obj.getEyeVector();

		%data = %this.projectile;
		%muzzleVelocity = %data.muzzleVelocity;

		if (%this.melee)
		{
			%origin = %eyePoint;
			%vector = vectorScale(%obj.getMuzzleVector(%slot), 20);
			%mask =
				$TypeMasks::StaticObjectType |
				$TypeMasks::StaticShapeObjectType |
				$TypeMasks::FxBrickObjectType |
				$TypeMasks::VehicleObjectType |
				$TypeMasks::PlayerObjectType;

			%ray = containerRayCast(%eyePoint, vectorAdd(%eyePoint, %vector), %mask, %obj);

			if (%ray)
			{
				%pos = getWords(%ray, 1, 3);

				%diffEye = vectorDist(%eyePoint, %pos);
				%diffMuzzle = vectorDist(%muzzlePoint, %pos);

				%muzzleVelocity *= %diffEye / %diffMuzzle;
			}
		}
		else
		{
			%origin = %muzzlePoint;

			if (%obj.isFirstPerson())
			{
				%vector = vectorScale(%eyeVector, 5);
				%mount = %obj.getObjectMount();

				%mask =
					$TypeMasks::StaticObjectType |
					$TypeMasks::StaticShapeObjectType |
					$TypeMasks::FxBrickObjectType |
					$TypeMasks::VehicleObjectType |
					$TypeMasks::PlayerObjectType;

				%ray = containerRayCast(%eyePoint, vectorAdd(%eyePoint, %vector), %mask, %mount, %obj);

				if (%ray)
				{
					%pos = getWords(%ray, 1, 3);
					%targetVector = vectorSub(%pos, %eyePoint);
					//%eyeToMuzzle = vectorSub(%start, %origin);
					if (vectorLen(%targetVector) < 3.1)
					{
						%muzzleVector = %obj.getEyeVector();
						%origin = %eyePoint;
					}
				}
			}
		}

		%playerVelocity = %obj.getVelocity();
		%dot = vectorDot(%eyeVector, %obj.getMuzzleVector(%slot));

		if (%dot < 0 && vectorLen(%playerVelocity) < 14)
			%inheritFactor = 0;
		else if (isFunction(%this.getName(), "getInheritFactor"))
			%inheritFactor = %this.getInheritFactor(%obj, %slot);
		else
			%inheritFactor = %data.velInheritFactor;

		if (isFunction(%this.getName(), "getProjectileCount"))
			%count = %this.getProjectileCount();
		else
			%count = %this.projectileCount $= "" ? 1 : %this.projectileCount;

		%baseVector = %muzzleVector;

		for (%i = 0; %i < %count; %i++)
		{
			if (isFunction(%this.getName(), "getProjectileSpread"))
				%spread = %this.getProjectileSpread(%obj, %slot);
			else
				%spread = %this.projectileSpread;

			%scalars = getRandomScalar() SPC getRandomScalar() SPC getRandomScalar();
			%spread = vectorScale(%scalars, mDegToRad(%spread / 2));

			%matrix = matrixCreateFromEuler(%spread);
			%vector = matrixMulVector(%matrix, %baseVector);

			%velocity = %data.muzzleVelocity * getWord(%obj.getScale(), 2);
			%velocity = vectorScale(%vector, %velocity);
			%className = %this.projectileType;
			%velocity = vectorAdd(%velocity, vectorScale(%playerVelocity, %inheritFactor));

			%projectile = new (%className)()
			{
				datablock = %data;

				initialVelocity = %velocity;
				initialPosition = %origin;

				sourceObject = %obj;
				sourceSlot = %slot;

				client = %client;
			};

			MissionCleanup.add(%projectile);
			%projectile.setScale(%obj.getScale());
		}

		return %projectile;
	}
};

activatePackage("CustomProjectileFire");

function getRandomScalar()
{
	return getRandom() * 2 - 1;
}