using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class BronzeGladius : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 48;
			Item.DamageType = DamageClass.Melee;
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 4;
			Item.value = 4500 * 5;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.BronzeGladiusProjectile>();
			Item.shootSpeed = 2.4f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity*4, ModContent.ProjectileType<BronzeShard>(), 15, 0f);
			return true;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BronzeBar>(), 5);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}