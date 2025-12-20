using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class Tesseract : ModItem
	{
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Tesseract", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.damage = 143;
			Item.width = 38;
			Item.height = 42;
			Item.useTime = 1;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Generic;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.Red;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.noMelee = true;
			Item.value = 123456 * 5;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<TesseractProjectile>()] == 0)
			{
				Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<TesseractProjectile>(), damage, knockback, player.whoAmI);
			}

			return false;
        }

        public override bool? UseItem(Player player)
        {
			float DirOffset = player.direction == 1 ? 0 : -5;

            Vector2 DustVelocity = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * 20;
			Vector2 SpawnPos = player.Center + new Vector2(DirOffset + 23 * player.direction, -23);
            Dust dust = Dust.NewDustDirect(SpawnPos, 0, 0, DustID.Electric, (int)DustVelocity.X, (int)DustVelocity.Y, newColor: Color.Blue);
            dust.noGravity = true;

            return true;
        }
	}	
}