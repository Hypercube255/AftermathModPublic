using AftermathMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class SporaySpores : ModProjectile
	{
		int timer;
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        public override void SetDefaults()
		{
			Projectile.height = 45;
			Projectile.width = 45;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
		}

        public override void AI()
		{
			timer++;

			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Spores>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, Scale: 1.5f);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Spores>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, Scale: 1.5f);

            if (timer > 36)
			{
				Projectile.Kill();
			}
		}
	}
}

