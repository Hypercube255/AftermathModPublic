using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class I1SBigFireball : ModProjectile
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
        int SmallFireballDMG
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        int SpawnInterval = Main.expertMode ? 25 : 30;

        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
		{	
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.hostile = true;
			Projectile.penetrate = 5;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }

        public override void AI()
		{
            DrawOffsetX = -2;
            DrawOriginOffsetY = -42;

            if (timer == 0)
            {
                timer = 10;//headstart, so that there isn't a large gap without fireballs in the middle
            }

            if (Main.rand.NextFloat() < 0.5f)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LavaMoss);
            }

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

			timer++;

            if (Gravity == 1 && timer > 16 && Projectile.velocity.Y < 15)
            {
                Projectile.velocity.Y += 0.075f;
            }

            if (timer % SpawnInterval == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.UnitY, ModContent.ProjectileType<I1SSmallFireball>(), SmallFireballDMG, 1, ai0: 1);
            }
		}
	}
}

