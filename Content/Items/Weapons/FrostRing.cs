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
	public class FrostRing : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 33;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Magic;
			Item.knockBack = 2;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FrostRingProjectile>();
			Item.shootSpeed = 25;
			Item.noMelee = true;
			Item.mana = 9;
			Item.value = 60500 * 5;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float projectileCount = 8; //projectile count
			float projRot = 315/(projectileCount-1); //sets angle, divides into segments

			for (int x = 0; x<projectileCount; x++)
			{
                Vector2 projectileDirection = velocity.RotatedBy(MathHelper.ToRadians(x * projRot));
                if (x==0)
				{
                    Projectile.NewProjectile(source, position, projectileDirection*2f, ModContent.ProjectileType<FrostRingProjectilePen>(), damage, knockback, player.whoAmI);
				}
				else
				{
                    Projectile.NewProjectile(source, position, projectileDirection, type, damage, knockback, player.whoAmI);
                }
					

			}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Snowblast>(), 1);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddIngredient(ItemID.IceBlock, 50);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}
	}	
}
//3 tiers - 1. short range ring (from dusts), 2. this. 3. something krazy