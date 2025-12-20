using AftermathMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class GunSporeCloud : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        float timer = 0;
		public override void SetDefaults()
		{
			Projectile.height = 80;
			Projectile.width = 80;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
		public override void AI()
		{
			timer++;
			if(timer >= 90)
			{
				Projectile.Kill();
				timer = 0;
			}

			for (int i = 0; i < 2; i++)
			{
                Vector2 DustCircleVelocity = (Vector2.UnitY * 13).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<Spores>(), SpeedX: DustCircleVelocity.X, SpeedY: DustCircleVelocity.Y, Scale: 1.5f);
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Projectile.WithinRange(targetHitbox.ClosestPointInRect(Projectile.Center), 40); // circular hitboxes <3 <3 <3
        }
    }
}

