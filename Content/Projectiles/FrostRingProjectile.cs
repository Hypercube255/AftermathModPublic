using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class FrostRingProjectile : ModProjectile
	{
		float timer = 0;
        Vector2 offset = new(2, 2);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (float i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Projectiles/FrostRingProjectile").Value;

                Color TrailColor = Color.White * MathHelper.Lerp(1f, 0, i / 10);

                Vector2 DrawPosition = (Projectile.oldPos[(int)i] - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

                Vector2 Origin = texture.Size() * 0.5f;

                Main.EntitySpriteDraw(texture, DrawPosition, null, TrailColor with { A = 0 }, Projectile.rotation, Origin, 1, SpriteEffects.None);
            }

            return true;
        }

        public override void SetDefaults()
		{
			
			Projectile.CloneDefaults (ProjectileID.Bullet);
			AIType = ProjectileID.Bullet;
			Projectile.height = 17;
			Projectile.width = 17;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.penetrate = 5;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void AI()
		{
			if (Main.rand.NextFloat() == 0.486) //funny 1/1000 aurora jumpscare
			{
                Lighting.AddLight(Projectile.Center, new Vector3(0, 91, 255));
            }
			else
			{
                Lighting.AddLight(Projectile.Center, new Vector3(0f, 0.36f, 1f));
            }

				timer += 1f;

			if(timer >= 15f)
			{
				if (Main.rand.NextFloat() < 0.6)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position + offset, Projectile.width - 4, Projectile.height - 4, 59, 0f, 0f, 0, Color.White, Scale: 2.5f);
                    dust.noGravity = true;
                }
			}
		}
	}
}

