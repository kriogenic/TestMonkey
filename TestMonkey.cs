using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using TestMonkey.Displays.Projectiles;
using MelonLoader;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using MelonLoader.ICSharpCode.SharpZipLib.Zip;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;

namespace TestMonkey
{
    /// <summary>
    /// The main class that adds the core tower to the game
    /// </summary>
    public class TestMonkey : ModTower
    {
        // public override string Portrait => "Don't need to override this, using the default of Name-Portrait";
        // public override string Icon => "Don't need to override this, using the default of Name-Icon";

        public override TowerSet TowerSet => TowerSet.Primary;
        public override bool Use2DModel => true;
        public override string BaseTower => TowerType.BananaFarm;
        public override int Cost => 400;

        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;
        public override string Description => "Absorbs monkeys to randomly throw out their current tier weapon";















        // public override string DisplayName => "Don't need to override this, the default turns it into 'Card Monkey'"

        public override ParagonMode ParagonMode => ParagonMode.Base555;

        public override string Get2DTexture(int[] tiers)
        {
            return "CompassMonkey";
        }
        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {

           // towerModel.RemoveBehaviors<AttackModel>();

            //towerModel.cost = 100;
            //towerModel.radius = 8;
            //towerModel.radiusSquared = 64;
            //towerModel.range = 34;
            //towerModel.ignoreBlockers = true;
            //towerModel.isGlobalRange = false;
            //towerModel.ignoreTowerForSelection = false;
            //towerModel.doesntRotate = true;

            ////towerModel.isGlobalRange = true;

            var attackModel = towerModel.GetAttackModel();
            if (attackModel != default(AttackModel))
            {
               // attackModel.weapons.Clear();
            }
            //    attackModel.range = 34;
            //    attackModel.targetProvider = null;
            //    attackModel.offsetX = 0;
            //    attackModel.offsetY = 0;
            //    attackModel.offsetZ = 0;
            //    attackModel.attackThroughWalls = true;
            //    attackModel.fireWithoutTarget = false;
            //    attackModel.framesBeforeRetarget = 0;
            //    attackModel.addsToSharedGrid = true;
            //    attackModel.sharedGridRange = 0;
            //}


            //Sets the first spike weapon to be a wizard monkey
            //var toTake = Game.instance.model.GetTower(TowerType.WizardMonkey, 0, 0, 0);
            //var toTakeProjectile = toTake.GetAttackModel().weapons[0].projectile.Duplicate();
            //towerModel.GetBehavior<AttackModel>().weapons[0].projectile = toTakeProjectile;

            ////Copies the first weapon

            //toTake = Game.instance.model.GetTower(TowerType.DartMonkey, 2, 0, 0);
            //toTakeProjectile = toTake.GetAttackModel().weapons[0].projectile.Duplicate();

            ////Adding a towers weapon!
            //var dupeWeapon = towerModel.GetBehavior<AttackModel>().weapons[0].Duplicate();
            //dupeWeapon.projectile = toTakeProjectile;
            //towerModel.GetBehavior<AttackModel>().AddWeapon(dupeWeapon);
         

            //When absorbing a tower
                //Add its weapon projectile
                //Use an equation to add speed and range to tower based on absorbed tower
        }

        /// <summary>
        /// Make Card Monkey go right after the Boomerang Monkey in the shop
        /// <br />
        /// If we didn't have this, it would just put it at the end of the Primary section
        /// </summary>
        public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
        {
            return towerSet.First(model => model.towerId == TowerType.BoomerangMonkey).towerIndex + 1;
        }

        /// <summary>
        /// Support the Ultimate Crosspathing Mod
        /// <br />
        /// That mod will handle actually allowing the upgrades to happen in the UI
        /// </summary>
        public override bool IsValidCrosspath(int[] tiers) =>
            ModHelper.HasMod("UltimateCrosspathing") || base.IsValidCrosspath(tiers);
    }
}