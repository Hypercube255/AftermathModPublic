using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class EvilRingsRing : ModProjectile
	{
		float Angle = 0.025f;
		int timer;
		public override void SetDefaults()
		{
			Projectile.height = 33;
			Projectile.width = 33;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void AI()
        {
			timer++;

			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain);

			Projectile.rotation = Projectile.velocity.ToRotation();

			Projectile.velocity = Projectile.velocity.RotatedBy(Angle);
			Angle *= 0.995f;
			if (Angle < 0)
			{
				Angle = 0;
			}
			if(timer > 900)
			{
				Projectile.alpha+=5;
			}
			if (Projectile.alpha == 255)
			{
				Projectile.Kill();
			}
        }
    }
}

