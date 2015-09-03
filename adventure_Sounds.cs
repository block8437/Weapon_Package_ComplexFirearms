// function playGunSound(%origin)
// {
//    %count = ClientGroup.getCount();
 
//    for (%i = 0; %i < %count; %i++)
//    {
//       %client = ClientGroup.getObject(%i);
//       %control = %client.getControlObject();

//       if (isObject(%control))
//       {
//          %dist = vectorDist(%origin, %control.getWorldBoxCenter());
//       }
      
//       if (%dist >= 30)
//       {
//          %sound = revolverFireSound;//"GunFarSound" @ getRandom(1,3);
//       }
//       else if (%dist >= 10)
//       {
//          %sound = revolverFireSound;//"GunDistantSound" @ getRandom(1,3);
//       }
//       else
//       {
//          %sound = revolverFireSound;
//       }
//       // talk(%sound);
//       if (isObject(%sound))
//       {
//          %client.play3D(%sound, %origin);
//          // talk(%client.getPlayerName());
//       }
//    }
// }

//Distant gunshots
// datablock audioDescription(audioDistant3d : audioClose3D) {
//    maxDistance = 100;
//    referenceDistance = 25;
//    volume = 1.2;
// };
// datablock audioProfile(GunDistantSound1) {
//    fileName = "./sounds/Reflection_Distant-01.wav";
//    description = audioDistant3d;
//    preload = true;
// };
// datablock audioProfile(GunDistantSound2) {
//    fileName = "./sounds/Reflection_Distant-02.wav";
//    description = audioDistant3d;
//    preload = true;
// };
// datablock audioProfile(GunDistantSound3) {
//    fileName = "./sounds/Reflection_Distant-03.wav";
//    description = audioDistant3d;
//    preload = true;
// };

// datablock audioDescription(audioFar3d : audioClose3D) {
//    maxDistance = 200;
//    referenceDistance = 60;
//    volume = 1.1;
// };
// datablock audioProfile(GunFarSound1) {
//    fileName = "./sounds/Reflection_Far-01.wav";
//    description = audioFar3d;
//    preload = true;
// };
// datablock audioProfile(GunFarSound2) {
//    fileName = "./sounds/Reflection_Far-02.wav";
//    description = audioFar3d;
//    preload = true;
// };
// datablock audioProfile(GunFarSound3) {
//    fileName = "./sounds/Reflection_Far-03.wav";
//    description = audioFar3d;
//    preload = true;
// };

  //// reload taps
datablock AudioProfile(advReloadTap0Sound)
{
   filename    = "./Sounds/reload_tap0.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReloadTap1Sound)
{
   filename    = "./Sounds/reload_tap1.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReloadTap2Sound)
{
   filename    = "./Sounds/reload_tap2.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReloadTap3Sound)
{
   filename    = "./Sounds/reload_tap3.wav";
   description = AudioClose3d;
   preload = true;
};


  //// reload outs
datablock AudioProfile(advReloadOut0Sound)
{
   filename    = "./Sounds/reload_Out0.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReloadOut1Sound)
{
   filename    = "./Sounds/reload_Out1.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReloadOut2Sound)
{
   filename    = "./Sounds/reload_Out2.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReloadOut3Sound)
{
   filename    = "./Sounds/reload_Out3.wav";
   description = AudioClose3d;
   preload = true;
};

  //// miscellenious reload parts (from tier+tactical)
datablock AudioProfile(advReload0Sound)
{
   filename    = "./Sounds/reload_1.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReload1Sound)
{
   filename    = "./Sounds/reload_2.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReload2Sound)
{
   filename    = "./Sounds/reload_3.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReload3Sound)
{
   filename    = "./Sounds/reload_4.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReload4Sound)
{
   filename    = "./Sounds/reload_5.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReload5Sound)
{
   filename    = "./Sounds/reload_6.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReload6Sound)
{
   filename    = "./Sounds/reload_7.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReload7Sound)
{
   filename    = "./Sounds/reload_8.wav";
   description = AudioClose3d;
   preload = true;
};

//Shell Casings and stuff (from BF4)
datablock AudioProfile(advReloadCasingSound1)
{
   filename    = "./Sounds/shellCasing1.wav";
   description = AudioClosest3d;
   preload = true;
};
datablock AudioProfile(advReloadCasingSound2)
{
   filename    = "./Sounds/shellCasing2.wav";
   description = AudioClosest3d;
   preload = true;
};
datablock AudioProfile(advReloadCasingSound3)
{
   filename    = "./Sounds/shellCasing3.wav";
   description = AudioClosest3d;
   preload = true;
};
datablock AudioProfile(advReloadInsert1Sound)
{
   filename    = "./Sounds/bullet_insert.wav";
   description = AudioClose3d;
   preload = true;
};
datablock AudioProfile(advReloadInsert2Sound)
{
   filename    = "./Sounds/bullet_insert2.wav";
   description = AudioClose3d;
   preload = true;
};