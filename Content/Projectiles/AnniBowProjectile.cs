using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace AftermathMod.Content.Projectiles
{
	public class AnniBowProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			
			AIType = ProjectileID.WoodenArrowFriendly;
			Projectile.height = 17;
			Projectile.width = 17;
			Projectile.friendly = true;
		}

        public override Color? GetAlpha(Color lightColor)
        {
			return lightColor = new(255, 255, 255, 255);
        }

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

			Dust dust =	Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 0, new Color(255,255,255), Scale: 1.6f);
			dust.noGravity = true;
		}
        public override bool PreKill(int timeLeft)
        {
            for(int i = 0; i<8; i++)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X-175f+i*50, Projectile.position.Y-1000-i*40), new Vector2(0, 15), ModContent.ProjectileType<FriendlyRocket>(), 110, 5, Projectile.owner);
			}
			for (int i = 0; i <75 ; i++)
			{
				Dust.NewDustDirect(Projectile.position - new Vector2(55, 55), 110, 110, DustID.LifeDrain);
			}
			return true;
        }
    }
}