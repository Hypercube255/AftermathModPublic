using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AftermathMod.Content.Projectiles
{
	public class BronzeJavelinProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
            Projectile.friendly = true;
        }
        public override void AI()
        {
			Projectile.ai[0] += 1;

			if (Projectile.ai[0] > 16 && Projectile.velocity.Y <= 12)
			{
				Projectile.velocity.Y += 0.2f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

        }
        public override void OnKill(int timeLeft)
        {
			for (int x = 0; x <= 8; x++)
			{
                Dust.NewDust(Projectile.Center, 20, 20, DustID.Gold);
            }
			SoundEngine.PlaySound(SoundID.NPCHit7, Projectile.position);
        }
    }
}

