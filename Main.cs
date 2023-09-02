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
        public override void OnTowerUpgraded(Tower tower, string upgradeName, TowerModel newBaseTowerModel)
        {

            //If our monkey was upgraded through any path
            if (tower.rootModel.Cast<TowerModel>().name.ToLower().Contains("testmonkey"))
            {
                var inRange = tower.GetTowersInRange().ToIl2CppList<Tower>();
                foreach (Tower theTower in inRange)
                {
                    MelonLogger.Msg("Tower in Range: " + theTower.towerModel.name);

                    MelonLogger.Msg("We have " + tower.rootModel.Cast<TowerModel>().GetBehavior<AttackModel>().weapons.Count + " weapons before upgrade");
                    TowerModel newModel = WeaponHelper.ModifyWeapon(theTower, tower);
                    tower.UpdateRootModel(newModel);
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
                AttackModel takeFromAttackModel = takeFromTower.rootModel.Cast<TowerModel>().GetAttackModel();
                AttackModel giveToAttackModel = giveToTowerModel.GetBehavior<AttackModel>().Duplicate();
                giveToTowerModel.RemoveBehavior<AttackModel>();
                //Duplicate the WeaponModel and add it to giveToTower weapons
                foreach (WeaponModel WP in takeFromAttackModel.weapons)
                    giveToAttackModel.AddWeapon(WP.Duplicate());

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
}