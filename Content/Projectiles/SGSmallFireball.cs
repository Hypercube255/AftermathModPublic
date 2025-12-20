using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class SGSmallFireball : ModProjectile
	{
		float timer = 0;

        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
		{	
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.hostile = true;
			Projectile.penetrate = 5;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }

        public override void AI()
		{
			DrawOriginOffsetY = -13;

            if (++Projectile.frameCounter >= 7)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

			timer++;
		}
	}
}

