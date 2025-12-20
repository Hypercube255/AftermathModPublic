using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using AftermathMod.Content.Dusts;

namespace AftermathMod.Content.Projectiles
{
	public class SporeDart : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.ai[0] % 4 == 0)
            {
                Dust.NewDust(Projectile.Center - new Vector2(4,4), 8, 8, ModContent.DustType<Spores>(), Scale: 2.5f);
            }
        }
        public override void OnKill(int timeLeft)
        {
			for (int x = 0; x <= 4; x++)
			{
                Dust.NewDust(Projectile.Center, 20, 20, ModContent.DustType<Spores>(), Scale: 2.5f);
            }
        }
    }
}

