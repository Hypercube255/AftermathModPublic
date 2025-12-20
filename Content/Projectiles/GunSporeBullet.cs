using AftermathMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class GunSporeBullet : ModProjectile
	{
		int timer = 0;
        public override void SetDefaults()
		{
			Projectile.height = 6;
			Projectile.width = 6;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
		}

        public override Color? GetAlpha(Color lightColor)
        {
			return Color.White;
        }

        public override void OnKill(int timeLeft)
        {
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<GunSporeCloud>(), (int)(Projectile.damage * 0.5f), 0.5f);
            SoundEngine.PlaySound(SoundID.Item49, Projectile.Center);
        }

        public override void AI()
		{
			timer++;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if (Main.rand.NextFloat() < 0.6)
			{
				Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Spores>(), Scale: 0.7f);
			}
		}
	}
}

