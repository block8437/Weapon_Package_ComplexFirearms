function Player::addItem(%player,%image)
{
	%suc = false;
	%client = %player.client;
	%slot = -1;
	for(%i = 0; %i < %player.getDatablock().maxTools; %i++)
	{
		%tool = %player.tool[%i];
		if(%tool == 0)
		{
			%player.tool[%i] = %image;
			%player.weaponCount++;
			messageClient(%client,'MsgItemPickup','',%i,%image, 0);
			%slot = %i;
			break;
		}
	}
	return %slot;
}

function Player::findItem(%this,%item) //Returns the item slot
{
	for(%i=0;%i<%this.getDatablock().maxTools;%i++)
	{
		if(isObject(%this.tool[%i]))
		{
			%tool=%this.tool[%i].getID();
			if(%tool==%item.getID())
			{
				return %i;
			}
		}
	}
	return -1;
}

function Player::removeItem(%this,%item)
{
	if(!isObject(%this) || !isObject(%item.getID()))
		return;
	%i = findItem(%this,%item);
	if(%i >= 0)
	{
		%this.tool[%i]=0;
		messageClient(%this.client,'MsgItemPickup','',%i, 0, 1);
		if(%this.currTool==%i)
		{
			%this.updateArm(0);
			%this.unMountImage(0);
		}
	}
}

function Player::removeItemSlot(%this, %slot) {
	%this.tool[%slot] = 0;
	messageClient(%this.client, 'MsgItemPickup', '', %slot, 0, 1);
	if(%this.currTool == %slot) {
		%this.updateArm(0);
		%this.unMountImage(0);
	}

	%this.weaponCount--;
}
