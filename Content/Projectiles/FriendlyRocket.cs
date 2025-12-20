using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AftermathMod.Content.Projectiles
{
	public class FriendlyRocket : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.rotation = MathHelper.ToRadians(90);
			Projectile.friendly = true;
		}

        public override bool CanHitPlayer(Player target)
        {
			return false;
        }
        public override bool PreKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HiddenExplosion>(), 100, 0f, Projectile.owner);

            SoundEngine.PlaySound(SoundID.NPCDeath14);

            for (int i = 0; i < 66; ++i)
            {
                Dust.NewDust(Projectile.position - new Vector2(250, 250), 400, 400, 286, 0f, 0f, 0, default, 2);
            }
            return true;
        }
    }
}

