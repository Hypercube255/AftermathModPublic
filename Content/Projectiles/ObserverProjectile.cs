using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class ObserverProjectile : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        int timer = 0;
		public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.alpha = 255;
            Projectile.tileCollide = false;
        }
		public override void AI()
		{
			timer++;

			if (Main.rand.NextFloat() < 0.6)
			{
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain, 0f, 0f, 0, new Color(255,255,255), Scale: 1.2f);
			}

			if (timer>20)
			{
				Projectile.Kill();
			}
		}
	}
}

