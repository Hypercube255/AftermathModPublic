using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Projectiles;

namespace AftermathMod.Content.Items.Weapons
{
	public class Revolution : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 13;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 25;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 3;
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<RevolutionProjectile>();
			Item.shootSpeed = 17;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = Item.CommonMaxStack;
            Item.value = 30 * 5;
        }
	
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(20);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 5);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
        }
	}
}