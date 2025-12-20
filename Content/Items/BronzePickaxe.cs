using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class BronzePickaxe : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 13;
			Item.useAnimation = 13;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2;
			Item.value = 8100 * 5;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.pick = 55;
			Item.attackSpeedOnlyAffectsWeaponAnimation = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BronzeBar>(), 9);
            recipe.AddIngredient(ItemID.Wood, 5);
			recipe.AddTile(TileID.Anvils);
            recipe.Register();
		}
	}
}