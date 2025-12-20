using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using AftermathMod.Content.Items.Weapons;
using System.Security.Cryptography.X509Certificates;

namespace AftermathMod.Content.Projectiles
{
	public class AnnihilationProjectileBig : ModProjectile
	{
		Vector2 offset = new(2, 2);
		public override void SetDefaults()
		{
			
			Projectile.CloneDefaults (ProjectileID.Bullet);
			AIType = ProjectileID.Bullet;
			Projectile.height = 27;
			Projectile.width = 27;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void OnKill(int timeLeft)
        {
            for (int x = 0; x < 2; x++)
            {
                Vector2 direction = Projectile.velocity.RotatedBy(-MathHelper.PiOver4).SafeNormalize(Vector2.One);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction.RotatedByRandom(MathHelper.PiOver2) * 105f, ModContent.ProjectileType<AnnihilationProjectile>(), 60, 0);
            }
			for (int x = 0; x < 30; x++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 309, newColor: new Color(255, 0, 0), Scale: 1.5f);
			}
        }
        public override void AI()
		{
			if (Main.rand.NextFloat() < 0.6)
			{
				Dust.NewDust(Projectile.position + offset, Projectile.width - 4, Projectile.height - 4, 309, newColor: new(255, 255, 255), Scale: 1.2f);
            }
        }
	}
}

