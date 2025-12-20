using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace AftermathMod.Content.Projectiles
{
	public class WisdomProjectile : ModProjectile
	{
		int timer = 0;
        public override bool PreDraw(ref Color lightColor)
        {
			return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Projectiles/WisdomProjectile", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f), new Rectangle(0, 0, texture.Width, texture.Height), Projectile.GetAlpha(new Color(255, 255, 255)), Projectile.rotation, texture.Size() * 0.5f, new Vector2(1, 1), SpriteEffects.None);
        }
        public override void SetDefaults()
		{
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.penetrate = 7;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			timer++;

			if(timer<120)
			{
                Projectile.Opacity = (((float)Math.Sin(timer / 6) + 1.5f) / 3);
            }
			else
			{
				Projectile.Opacity -= 0.03f;
			}

			if (Projectile.Opacity <= 0)
			{
				Projectile.Kill();
			}

			if (timer > 20)
			{
				Projectile.velocity *= 0.970f;
			}
		}
	}
}

