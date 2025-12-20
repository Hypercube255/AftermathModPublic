using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
    public class TalismanofPerseverance : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.width = 38;
            Item.height = 38;
            Item.value = 25000 * 5;
            Item.accessory = true;
            Item.defense = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 = 11 * (player.statLifeMax2/10); //10% increased health
        }
    }
}