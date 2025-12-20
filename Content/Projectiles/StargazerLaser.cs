using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class StargazerLaser : ModProjectile
	{
		int timer = 0;
        public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

        public override void AI()
		{
			timer++;
		}
	}
}

