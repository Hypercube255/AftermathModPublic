using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class DisappointmentProjectile : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        int timer = 0;
		public override void SetDefaults()
		{
			Projectile.height = 32;
			Projectile.width = 32;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.alpha = 255;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 7;
		}
		public override void AI()
		{
			timer++;

			for(int i = 0; i < 4; i++)
			{
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f, 0, new Color(255, 255, 255), Scale: 2f);
                dust.noGravity = true;
            }

            if (timer>15 && Projectile.velocity.Y < 20)
			{
				Projectile.velocity.Y += 0.5f;
			}
		}
	}
}

