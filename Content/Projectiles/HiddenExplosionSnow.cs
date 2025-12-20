using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AftermathMod.Content.Projectiles
{
	public class HiddenExplosionSnow : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        float timer = 0;
		public override void SetDefaults()
		{
			Projectile.height = 250;
			Projectile.width = 250;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 21;
        }
		public override void AI()
		{
			timer++;
			if(timer>=20)
			{
				Projectile.Kill();
				timer = 0;
			}
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Projectile.WithinRange(targetHitbox.ClosestPointInRect(Projectile.Center), 130); // circular hitboxes <3 <3 <3
        }
    }
}

