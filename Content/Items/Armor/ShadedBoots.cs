using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Items;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Legs)]
	public class ShadedBoots : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 144000 * 5;
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 6;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.20f; //=20%
		}
		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadedBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}