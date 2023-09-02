using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;

namespace TestMonkey.Upgrades.BottomPath
{
    public class CardCounting : ModUpgrade<TestMonkey>
    {
        public override int Path => BOTTOM;
        public override int Tier => 1;
        public override int Cost => 300;

        public override string Description => "Throws cards faster";

        public override string Portrait => "CardMonkey-Portrait";

        public override void ApplyUpgrade(TowerModel tower)
        {
          
        }
    }
}