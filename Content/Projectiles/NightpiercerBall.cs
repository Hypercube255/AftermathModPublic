using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using AftermathMod.Content.Dusts;
using System;

namespace AftermathMod.Content.Projectiles
{
	public class NightpiercerBall : ModProjectile
	{
        int timer;

        float ExtraPierce
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

		public override void SetDefaults()
		{
            Projectile.aiStyle = -1;
            Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void AI()
        {
            if(timer == 0)
            {
                Projectile.penetrate += (int)ExtraPierce;
            }

            timer++;
            Projectile.rotation += 0.5f;

            if (Main.rand.NextFloat() < 0.5f)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ShadedSmoke>(), Scale: 3f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ShadedSparkles>(), Scale: 1.5f);
            }

            if (timer > 25)
            {
                Projectile.Kill();
            }
        }
        public override void OnKill(int timeLeft)
        {
			for (int i = 0; i <= 6; i++)
			{
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ShadedSmoke>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, Scale: 3f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ShadedSparkles>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, Scale: 1.5f);
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
    }
}

