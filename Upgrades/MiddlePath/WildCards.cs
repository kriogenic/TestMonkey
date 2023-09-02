using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using TestMonkey.Displays.Projectiles;
using Il2Cpp;

namespace TestMonkey.Upgrades.MiddlePath
{
    public class WildCards : ModUpgrade<TestMonkey>
    {
        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override int Cost => 1000;

        public override string Description => "Cards can damage all Bloon types";

        public override string Portrait => "CardMonkey-Portrait";

        public override void ApplyUpgrade(TowerModel tower)
        {

        }
    }
}