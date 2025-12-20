using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class BoulderSword : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 48;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = 1;
			Item.knockBack = 10;
			Item.value = 57415 * 5;
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.FriendlyBoulder>();
			Item.shootSpeed = 20;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 8); //2400
			recipe.AddIngredient(ItemID.StoneBlock, 99);//
			recipe.AddIngredient(ItemID.SoulofMight, 5); //40000
            recipe.AddIngredient(ItemID.Boulder, 5);//
            recipe.AddIngredient(ItemID.LifeCrystalBoulder, 1);//15000
            recipe.AddIngredient(ItemID.BouncyBoulder, 1);//15
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}