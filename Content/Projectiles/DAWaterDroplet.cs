using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class DAWaterDroplet : ModProjectile
	{
		int timer = 0;
        public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
		}

        public override void AI()
		{
			timer++;

			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			DrawOriginOffsetY = -7;

			if(timer > 16 && Projectile.velocity.Y < 16)
			{
				Projectile.velocity.Y += 0.4f;
			}

			if (Main.rand.NextFloat() < 0.6)
			{
				Dust.NewDustPerfect(Projectile.Center, DustID.Water);
			}
		}
	}
}

