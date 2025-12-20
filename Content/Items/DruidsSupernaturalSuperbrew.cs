using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
    public class DruidsSupernaturalSuperbrew : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 17));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.width = 50;
            Item.height = 50;
            Item.value = 22200 * 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            float LifeRatio = player.statLife / (float)player.statLifeMax2;

            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
            player.lifeRegen += (int)MathHelper.Lerp(11, 0, LifeRatio); //more regen as life gets lower (0 -> 5 HP/s)
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe()
                    .AddIngredient(ItemID.BottledWater)
                    .AddIngredient(ItemID.ChlorophyteOre, 15)
                    .AddIngredient(ItemID.Vine, 3)
                    .AddIngredient(ItemID.Daybloom, 5)
                    .AddIngredient(ItemID.Fireblossom, 5)
                    .AddIngredient(ItemID.Mushroom, 8)
                    .AddTile(TileID.Bottles)
                    .Register();

        }
    }
}