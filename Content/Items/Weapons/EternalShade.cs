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
	public class EternalShade : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 52;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 33;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 5;
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType <EternalShadeProjectile> ();
			Item.shootSpeed = 30;
            Item.value = 60000 * 5;
        }
	
	public override void MeleeEffects(Player player, Rectangle hitbox)
	{
		if (Main.rand.NextBool(1))
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 309, 0f, 0f, 0, new Color(255,255,255), Scale: 1.2f);
			}
	}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float projectileCount = 4 + Main.rand.Next(2); //projectile count
			float projRot = 50/(projectileCount-1); //sets angle, divides into segments

			for (int x = 0; x<projectileCount; x++)
			{
				Vector2 projectileDirection = velocity.RotatedBy(MathHelper.ToRadians(-25 + x*projRot));
				Projectile.NewProjectile(source, position, projectileDirection, type, damage, knockback, player.whoAmI);

			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Obsidian, 50);
			recipe.AddIngredient(ItemID.Ectoplasm, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}	
}
//homing projectile