using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class BronzeJavelin : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 48;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2;
			Item.value = 30 * 5;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.BronzeJavelinProjectile>();
			Item.consumable = true;
			Item.shootSpeed = 15;
			Item.maxStack = Item.CommonMaxStack;
			Item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(30);
			recipe.AddIngredient(ModContent.ItemType<BronzeBar>());
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}