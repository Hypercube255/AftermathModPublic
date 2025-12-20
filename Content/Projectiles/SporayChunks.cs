using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AftermathMod.Content.Dusts;
using Terraria.Audio;

namespace AftermathMod.Content.Projectiles
{
	public class SporayChunks : ModProjectile
	{
		int timer = 0;
        public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
		}

        public override void AI()
		{
			timer++;

			Projectile.rotation += 0.15f;

			if(timer > 12 && Projectile.velocity.Y < 16)
			{
				Projectile.velocity.Y += 0.4f;
			}

			if (Main.rand.NextFloat() < 0.4)
			{
				Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Spores>(), Scale: 0.75f);
			}
		}

        public override void OnKill(int timeLeft)
        {
			for (int i = 0; i < 5; i++)
			{
                Vector2 DustCircleVelocity = (Vector2.UnitY * 7).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Spores>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, Scale: 1.5f);
            }
			SoundEngine.PlaySound(SoundID.Item49, Projectile.Center);
        }
	}
}

