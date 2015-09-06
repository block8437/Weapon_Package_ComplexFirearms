%error = forceRequiredAddOn("Weapon_Gun");

if (%error == $Error::AddOn_Disabled)
{
	GunItem.uiName = "";
}
else if (%error == $Error::AddOn_NotFound)
{
	error("ERROR: Weapon_Package_ComplexFirearms - required add-on Weapon_Gun not found");
	return;
}

exec("./lib/lag-compensation.cs");
exec("./lib/timed-raycast.cs");
exec("./lib/timed-custom-fire.cs");
exec("./lib/player-hit-region.cs");
// exec("./customFire.cs");
exec("./calculations.cs");
exec("./adventure_Effects.cs");
exec("./adventure_Sounds.cs");
exec("./addItem.cs");
exec("./package.cs");
exec("./ammo.cs");
exec("./Weapon_Revolver.cs");
exec("./Weapon_Colt1911.cs");
exec("./Weapon_M1Garand.cs");
exec("./Weapon_Thompson.cs");
exec("./Weapon_Remmington.cs");
exec("./Weapon_HEGrenade.cs");

function serverCmdReload(%client)
{
	if (%client.isAdmin)
		exec("./server.cs");
}

// Axis stuff
function eulerToAxis(%euler)
{
	%euler = VectorScale(%euler,$pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	return getWords(%matrix,3,6);
}

function axisToEuler(%axis)
{
	%angleOver2 = getWord(%axis,3) * 0.5;
	%angleOver2 = -%angleOver2;
	%sinThetaOver2 = mSin(%angleOver2);
	%cosThetaOver2 = mCos(%angleOver2);
	%q0 = %cosThetaOver2;
	%q1 = getWord(%axis,0) * %sinThetaOver2;
	%q2 = getWord(%axis,1) * %sinThetaOver2;
	%q3 = getWord(%axis,2) * %sinThetaOver2;
	%q0q0 = %q0 * %q0;
	%q1q2 = %q1 * %q2;
	%q0q3 = %q0 * %q3;
	%q1q3 = %q1 * %q3;
	%q0q2 = %q0 * %q2;
	%q2q2 = %q2 * %q2;
	%q2q3 = %q2 * %q3;
	%q0q1 = %q0 * %q1;
	%q3q3 = %q3 * %q3;
	%m13 = 2.0 * (%q1q3 - %q0q2);
	%m21 = 2.0 * (%q1q2 - %q0q3);
	%m22 = 2.0 * %q0q0 - 1.0 + 2.0 * %q2q2;
	%m23 = 2.0 * (%q2q3 + %q0q1);
	%m33 = 2.0 * %q0q0 - 1.0 + 2.0 * %q3q3;
	return mRadToDeg(mAsin(%m23)) SPC mRadToDeg(mAtan(-%m13, %m33)) SPC mRadToDeg(mAtan(-%m21, %m22));
}
