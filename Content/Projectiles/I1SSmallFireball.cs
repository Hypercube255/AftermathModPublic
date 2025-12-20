using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class I1SSmallFireball : ModProjectile
	{
        float Gravity// 0 = no, 1 = yes
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        float timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

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
            DrawOffsetX = -2;

            if (++Projectile.frameCounter >= 7)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }

            if (Main.rand.NextFloat() < 0.15f)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LavaMoss);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

			timer++;

            if (Gravity == 1 && timer > 16 && Projectile.velocity.Y < 15)
            {
                Projectile.velocity.Y += 0.15f;
            }
		}
	}
}

