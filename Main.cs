using BTD_Mod_Helper;
using TestMonkey;
using MelonLoader;
using Il2CppAssets.Scripts.Models.Towers;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.Scenes;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Text.RegularExpressions;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppTMPro;
using Il2CppAssets.Scripts.Models.Towers.Upgrades;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Towers;
using System;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Powers;
using static Il2CppAssets.Scripts.Utils.ObjectCache;
using static MelonLoader.MelonLogger;
using Il2CppNinjaKiwi.LiNK.Lobbies;

[assembly: MelonInfo(typeof(CardMonkeyMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace TestMonkey
{
    public class CardMonkeyMod : BloonsTD6Mod
    {
        //public override void OnTitleScreen()
        //{
        //    base.OnTitleScreen();

        //    var models = Game.instance.model.towers;
        //    foreach (var model in models)
        //    {
        //        MelonLogger.Msg(model.name);
        //        if (model.name.ToLower().Equals("testmonkey-testmonkey"))
        //        {
        //            // MelonLogger.Msg("Tryiplplplpl");

        //            return;
        //        }
        //    }
        //}


        static TowerToSimulation lastSelected;

        public override void OnUpdate()
        {
            base.OnUpdate();
            bool inAGame = InGame.instance != null && InGame.instance.bridge != null;
            if (inAGame)
            {
                if (InGame.instance.inputManager.SelectedTower != null)
                {
                    lastSelected = InGame.instance.inputManager.SelectedTower;
                    //lastSelected.tower.GetTowersInRange


                }
            }

        }

        public override void OnTowerUpgraded(Tower tower, string upgradeName, TowerModel newBaseTowerModel)
        {

            //If our monkey was upgraded through any path
            if (tower.rootModel.Cast<TowerModel>().name.ToLower().Contains("testmonkey"))
            {
                var inRange = tower.GetTowersInRange().ToIl2CppList<Tower>();
                foreach (Tower theTower in inRange)
                {
                    MelonLogger.Msg("Tower in Range: " + theTower.towerModel.name);

                    //Outputs 1 always
                    MelonLogger.Msg("We have " + tower.rootModel.Cast<TowerModel>().GetBehavior<AttackModel>().weapons.Count + " weapons before upgrade");
                    TowerModel newModel = WeaponHelper.ModifyWeapon(theTower, tower);
                    tower.UpdateRootModel(newModel);
                    // tower.UpdateRootModel(tower.rootModel.Cast<TowerModel>());
                    MelonLogger.Msg("We added another attack and now have " + tower.rootModel.Cast<TowerModel>().GetBehavior<AttackModel>().weapons.Count);
                   
                }

                for(int i = inRange.Count - 1; i >= 0; i--)
                {
                    inRange[i].Destroy();
                }
            }
        }


    }

    public static class WeaponHelper
    {
        public static TowerModel? ModifyWeapon(Tower takeFromTower, Tower giveToTower)
        {
            //If we are a pilot, ace or upgraded dartling\boomerang don't take it
            if (takeFromTower.towerModel.name.ToLower().Contains("helipilot")) return null;
            if (takeFromTower.towerModel.name.ToLower().Contains("monkeyace")) return null;
            if (Regex.IsMatch(takeFromTower.towerModel.name, "DartlingGunner-4..") || Regex.IsMatch(takeFromTower.towerModel.name, "DartlingGunner-5..") ||
                Regex.IsMatch(takeFromTower.towerModel.name, "BoomerangMonkey-5..")) return null;

            try
            {
                TowerModel giveToTowerModel = giveToTower.rootModel.Cast<TowerModel>().Duplicate();
                
                //We want to make a copy of the Dart Monkeys weapon
                // TowerModel baseModel = Game.instance.model.GetTowerFromId("DartMonkey");
                AttackModel takeFromAttackModel = takeFromTower.rootModel.Cast<TowerModel>().GetAttackModel();
                AttackModel giveToAttackModel = giveToTowerModel.GetBehavior<AttackModel>().Duplicate();
                giveToTowerModel.RemoveBehavior<AttackModel>();
                //Duplicate the WeaponModel and add it to giveToTower weapons
                foreach (WeaponModel WP in takeFromAttackModel.weapons)
                {
                    giveToAttackModel.AddWeapon(WP.Duplicate());
                }
                //WeaponModel takeFromWeaponModel = baseAttack.weapons[0].Duplicate();
                

               
                //Not sure if this works? removes original AttackModel and replaces it with modified one
               
                giveToTowerModel.AddBehavior(giveToAttackModel);
                return giveToTowerModel;
               
            }
            catch (Exception e)
            {
                MelonLogger.Msg($"Failed to add {takeFromTower.towerModel.name} attacks!");
                MelonLogger.BigError("[TestMonkey-FatalError]", e.Message);
                return giveToTower.rootModel.Cast<TowerModel>();
            }
        }

    }



    // static TowerModel? baseSpac;

    //public static void ModifyWeapon(TowerModel takeFromTower, TowerModel baseSpac)
    //{



    //    MelonLogger.Msg("Trying to take weapons from " + takeFromTower.name);

    //    //If we are a pilot, ace or upgraded dartling\boomerang don't take it
    //    if (takeFromTower.name.ToLower().Contains("helipilot")) return;
    //    if (takeFromTower.name.ToLower().Contains("monkeyace")) return;
    //    if (Regex.IsMatch(takeFromTower.name, "DartlingGunner-4..") || Regex.IsMatch(takeFromTower.name, "DartlingGunner-5..") || Regex.IsMatch(takeFromTower.name, "BoomerangMonkey-5..")) return;
    //    //Console.WriteLine(tower.name);


    //    //Gets the BASE SpikeFactory model
    //    //   baseSpac = Game.instance.model.GetTowerFromId("SpikeFactory");

    //    MelonLogger.Msg("We got baseSpac: " + baseSpac.baseId);
    //    try
    //    {
    //        //if tower to absorb weapons from has an attack model
    //        if (takeFromTower.HasBehavior<AttackModel>())
    //        {
    //            MelonLogger.Msg($"{takeFromTower.name} has an AttackModel Behavior");
    //            MelonLogger.Msg("Trying to get base duplicates");
    //            //Get a clone of the SpikeFactory base AttackModel and WeaponModel
    //            var baseSpacAttackClone = baseSpac.GetBehavior<AttackModel>().Duplicate();
    //            var baseSpacAttackWeapon0Clone = baseSpac.GetBehavior<AttackModel>().weapons[0].Duplicate();
    //            bool hasProjectiles = false;

    //            if (baseSpacAttackClone == null || baseSpacAttackWeapon0Clone == null)
    //            {
    //                MelonLogger.Msg("Could not get Attack or Weapon Clone!!");
    //                return;
    //            }

    //            var towerProjectiles = takeFromTower.GetBehavior<AttackModel>().GetDescendants<ProjectileModel>().ToIl2CppReferenceArray<ProjectileModel>();
    //            foreach (var proj in towerProjectiles)
    //            {
    //                if (proj.HasBehavior<TravelStraitModel>() || takeFromTower.name.ToLower().Contains("boomer"))
    //                {

    //                    hasProjectiles = true;

    //                }
    //            }
    //            if (hasProjectiles)
    //            {

    //                MelonLogger.Msg("Tower has Projectiles!!!");

    //                //Stores the original attack of the tower
    //                var oldAttack = takeFromTower.GetBehavior<AttackModel>().Duplicate();
    //                baseSpacAttackClone.range = oldAttack.range; //Copies its range to baseclose


    //                int j = 1;
    //                bool modified = false;
    //                //Loops through each weapon of selected tower
    //                foreach (var wep in takeFromTower.GetBehavior<AttackModel>().weapons)
    //                {
    //                    //If its a traveling straight projectile....
    //                    if (wep.projectile.HasBehavior<TravelStraitModel>() || takeFromTower.name.ToLower().Contains("boomer"))
    //                    {

    //                        //Adds another weapon to modify, if the absorbed tower has multiple

    //                        baseSpacAttackClone.AddWeapon(baseSpacAttackWeapon0Clone.Duplicate());

    //                        //After the first run, always add a new weapon before continuing
    //                        modified = true;
    //                        //Copies the firerate
    //                        baseSpacAttackClone.weapons[j].Rate = wep.Rate;
    //                        int pierceMultiplier = 1;

    //                        //Attempts to get a piece multiplier from different weaponmodel emissions
    //                        try { pierceMultiplier = wep.emission.Cast<RandomArcEmissionModel>().Count; } catch { }
    //                        try { pierceMultiplier = wep.emission.Cast<ArcEmissionModel>().Count; } catch { }
    //                        try { pierceMultiplier = wep.emission.Cast<RandomEmissionModel>().count; } catch { }
    //                        try { pierceMultiplier = wep.emission.Cast<AdoraEmissionModel>().count; } catch { }
    //                        try { pierceMultiplier = wep.emission.Cast<AlternatingArcEmissionModel>().count; } catch { }


    //                        //baseSpacAttackClone.weapons[0].projectile.GetBehavior<SetSpriteFromPierceModel>().sprites = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStringArray(8) { proj.GetBehavior<DisplayModel>().display, proj.GetBehavior<DisplayModel>().display, proj.GetBehavior<DisplayModel>().display, proj.GetBehavior<DisplayModel>().display, proj.GetBehavior<DisplayModel>().display, proj.GetBehavior<DisplayModel>().display, proj.GetBehavior<DisplayModel>().display, proj.GetBehavior<DisplayModel>().display };
    //                        //baseSpacAttackClone.weapons[0].projectile.GetBehavior<DisplayModel>().display = proj.GetBehavior<DisplayModel>().display;
    //                        //baseSpacAttackClone.weapons[0].projectile.RemoveBehavior<SetSpriteFromPierceModel>();
    //                        //baseSpacAttackClone.weapons[0].projectile.RemoveBehavior<DisplayModel>();


    //                        //Removes the different weapon behaviors
    //                        wep.projectile.RemoveBehavior<TravelStraitModel>();
    //                        wep.projectile.RemoveBehavior<FollowPathModel>();
    //                        // wep.projectile.RemoveBehavior<DisplayModel>();
    //                        wep.projectile.RemoveBehavior<TrackTargetWithinTimeModel>();
    //                        wep.projectile.RemoveBehavior<TrackTargetModel>();

    //                        //Set weapon projectile collisions
    //                        baseSpacAttackClone.weapons[j].projectile.collisionPasses = wep.projectile.collisionPasses;


    //                        //If weapon has a damagemodel set damage and immune properties
    //                        if (wep.projectile.HasBehavior<DamageModel>())
    //                        {
    //                            baseSpacAttackClone.weapons[j].projectile.GetBehavior<DamageModel>().damage = wep.projectile.GetBehavior<DamageModel>().damage;
    //                            baseSpacAttackClone.weapons[j].projectile.GetBehavior<DamageModel>().immuneBloonProperties = wep.projectile.GetBehavior<DamageModel>().immuneBloonProperties;
    //                        }
    //                        else
    //                        {
    //                            baseSpacAttackClone.weapons[j].projectile.RemoveBehavior<DamageModel>();
    //                        }

    //                        //Add the behavior of the absorbed towers projectiles
    //                        foreach (var bev in wep.projectile.behaviors)
    //                        {
    //                            baseSpacAttackClone.weapons[j].projectile.AddBehavior(bev.Duplicate());
    //                        }


    //                        //Set the max pierce of projectile
    //                        baseSpacAttackClone.weapons[j].projectile.pierce = wep.projectile.pierce * pierceMultiplier;
    //                        baseSpacAttackClone.weapons[j].projectile.maxPierce = wep.projectile.maxPierce * pierceMultiplier;


    //                        j++;

    //                    }
    //                }

    //                takeFromTower.RemoveBehavior<AttackModel>();
    //                takeFromTower.AddBehavior(baseSpacAttackClone);
    //                takeFromTower.TargetTypes = baseSpac.TargetTypes.Duplicate();
    //            }




    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        MelonLogger.Msg($"Modifying {takeFromTower.name} attacks for absorbsion failed!");
    //        MelonLogger.BigError("FATAL", e.Message);
    //    }





    //}



}