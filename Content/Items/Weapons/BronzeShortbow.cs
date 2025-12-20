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
	public class BronzeShortbow : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.width = 24;
			Item.height = 36;
			Item.useTime = 20;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 1f;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.shootSpeed = 30;
			Item.useAmmo = AmmoID.Arrow;
			Item.shoot = 10;
			Item.value = 6300 * 5;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Main.rand.NextBool(5))
			{
                velocity *= Main.rand.NextFloat(0.95f, 1.05f);
                velocity = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f));

                Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
            }
			
			return true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BronzeBar>(), 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}	
}
