using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class BronzeBar : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.Green;
            Item.width = 30;
            Item.height = 24;
            Item.value = 900 * 5;
        }
		public override void AddRecipes()
		{
			CreateRecipe(3)
				.AddIngredient(ItemID.CopperBar, 2)
                .AddIngredient(ItemID.TinBar, 2)
				.AddTile(TileID.Furnaces)
                .Register();
		}
	}
}