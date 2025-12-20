using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class HiddenGammadisc : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        float timer = 0;
		public override void SetDefaults()
		{
			Projectile.height = 19;
			Projectile.width = 19;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 5;
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.5f, 0f, 0f);

			timer++;
			if(timer>=2)
			{
				Projectile.Kill();
				timer = 0;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(target.position, target.width, target.height, DustID.RedTorch, Scale: 2f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(target.position, target.width, target.height, DustID.RedTorch, Scale: 2f);
            }
        }
    }
}

