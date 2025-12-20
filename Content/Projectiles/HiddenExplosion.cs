using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class HiddenExplosion : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        float timer = 0;
		public override void SetDefaults()
		{
			Projectile.height = 500;
			Projectile.width = 500;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.OnFire3, 300);
        }

		public override void AI()
		{
			timer++;
			if(timer>=4)
			{
				Projectile.Kill();
				timer = 0;
			}
		}
	}
}

