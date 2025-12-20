using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class BronzeHamaxe : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 25;
			Item.useAnimation = Item.useTime;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5.5f;
			Item.value = 7200 * 5;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			Item.axe = 13;
			Item.hammer = 60;
			Item.attackSpeedOnlyAffectsWeaponAnimation = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BronzeBar>(), 8);
            recipe.AddIngredient(ItemID.Wood, 4);
			recipe.AddTile(TileID.Anvils);
            recipe.Register();
		}
	}
}