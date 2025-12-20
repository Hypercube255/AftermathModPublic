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
	public class BronzeBroadsword : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 17;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 7f;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = false;
			Item.value = 7200 * 5;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BronzeBar>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}	
}