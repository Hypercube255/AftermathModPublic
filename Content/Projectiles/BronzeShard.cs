using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AftermathMod.Content.Projectiles
{
	public class BronzeShard : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 2;
            Projectile.friendly = true;
        }
        public override void AI()
        {
			Projectile.ai[0] += 1;

			if (Projectile.ai[0] > 14 && Projectile.velocity.Y <= 16)
			{
				Projectile.velocity.Y += 0.3f;
			}

			Projectile.rotation += 0.4f;
        }
        public override void OnKill(int timeLeft)
        {
			for (int x = 0; x <= 4; x++)
			{
                Dust.NewDust(Projectile.Center, 20, 20, DustID.Gold);
            }
			SoundEngine.PlaySound(SoundID.NPCHit7, Projectile.position);
        }
    }
}

