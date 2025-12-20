using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class BeegVortex : ModProjectile
	{
        public override void SetDefaults()
		{
			AIType = ProjectileID.Bullet;
			Projectile.height = 250;
			Projectile.width = 250;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 75;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 2;
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 75);
        }
        public override void AI()
		{
			Projectile.rotation += 0.1f;


			// makes it not do barrel rolls
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(25);
            DrawOriginOffsetY = -(25);
        }
	}
}

