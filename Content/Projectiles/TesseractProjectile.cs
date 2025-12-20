using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AftermathMod.Assets;
using AftermathMod.Content.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace AftermathMod.Content.Projectiles
{
	public class TesseractProjectile : ModProjectile
	{
		int timer = 0;
        public override string Texture => "AftermathMod/Assets/EmptySprite";

		float Opacity = 0f;

        public override void SetDefaults()
		{
			Projectile.width = 77;
			Projectile.height = Projectile.width;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
			Projectile.ArmorPenetration = 1000;
		}

        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D ray = AssetDatabase.TesseractRay.Value;

			Vector2 Origin = new Vector2(ray.Width * 0.5f, ray.Height - 25);

			for (int i = 0; i < 8; i++)
			{
                Main.EntitySpriteDraw(ray, Projectile.Center - Main.screenPosition, null, Color.Cyan with { A = 0 } * Opacity, timer * 0.04f + i * MathHelper.PiOver4, Origin, 0.8f, SpriteEffects.None);
            }
            for (int i = 0; i < 6; i++)
            {
                Main.EntitySpriteDraw(ray, Projectile.Center - Main.screenPosition, null, Color.CornflowerBlue with { A = 0 } * Opacity * 0.6f, -timer * 0.03f + i * (MathHelper.TwoPi / 6), Origin, 1.3f, SpriteEffects.None);
            }
            for (int i = 0; i < 4; i++)
            {
                Main.EntitySpriteDraw(ray, Projectile.Center - Main.screenPosition, null, Color.Blue with { A = 0 } * Opacity * 0.4f, timer * 0.02f + i * (MathHelper.TwoPi / 4), Origin, 1.5f, SpriteEffects.None);
            }

            return false;
        }

        public override void AI()
		{
			timer++;

			if (Opacity < 0.3f)
			{
				Opacity += 0.025f;
			}

			if(timer % 30 == 0 || timer == 1)
			{
				SoundEngine.PlaySound(SoundID.Item82, Projectile.Center);
			}

			Player player = Main.player[Projectile.owner];

			Projectile.Center = Main.MouseWorld;

			if (!player.controlUseItem || player.HeldItem.type != ModContent.ItemType<Tesseract>() || player.dead)
			{
				Projectile.Kill();
			}
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Projectile.WithinRange(targetHitbox.ClosestPointInRect(Projectile.Center), Projectile.width * 0.5f); // circular hitboxes <3 <3 <3
        }
    }
}

