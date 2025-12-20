using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
    public class SecondChance : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.width = 32;
            Item.height = 40;
            Item.value = 72000 * 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AftermathPlayer>().HasSecondChance = true;

            player.statLifeMax2 += 80;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe()
                    .AddIngredient(ItemID.HallowedBar, 12)
                    .AddIngredient(ItemID.SoulofSight, 1)
                    .AddIngredient(ItemID.SoulofMight, 1)
                    .AddIngredient(ItemID.SoulofFright, 1)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
        }
    }
}