using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Items;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Legs)]
	public class BronzeGreaves : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 16200 * 5;
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.15f; //=15%
		}
		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BronzeBar>(), 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}