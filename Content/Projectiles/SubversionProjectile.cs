using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class SubversionProjectile : ModProjectile
	{
		float timer = 0;
        Vector2 offset = new(1, 1);
        public override void SetDefaults()
		{
			
			Projectile.CloneDefaults (ProjectileID.Bullet);
			AIType = ProjectileID.Bullet;
			Projectile.height = 18;
			Projectile.width = 16;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.light = 1f;
			Projectile.penetrate = 5;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void AI()
		{
			timer+=1f;
			if(timer >= 15f)
			{
				if (Main.rand.NextFloat() < 0.6)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position + offset, Projectile.width-2, Projectile.height-2, 59, 0f, 0f, 0, new Color(255,255,255), Scale: 2f);
                    dust.noGravity = true;
                }
			}
		}
	}
}

