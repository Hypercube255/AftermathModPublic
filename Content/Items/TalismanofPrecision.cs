using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
    public class TalismanofPrecision : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.width = 32;
            Item.height = 40;
            Item.value = 25000 * 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Generic) += 6;
            player.GetArmorPenetration(DamageClass.Generic) += 3;
        }
    }
}