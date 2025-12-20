using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AftermathMod.Content.Projectiles
{
	public class RevolutionProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(99);
			AIType = 99;
			Projectile.height = 42;
			Projectile.width = 42;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.maxPenetrate = 10;
		}
        public override bool CanHitPlayer(Player target)
        {
			return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			return false;
        }
        public override void AI()
        {
			if (Projectile.velocity.Length() <= 2f)
			{
				Projectile.Kill();
			}
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(4);
            DrawOriginOffsetY = -(4);
        }
        public override void OnKill(int timeLeft)
        {
			for (int x = 0; x <= 10; x++)
			{
                Dust.NewDust(Projectile.Center, 20, 20, DustID.WoodFurniture);
            }
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }
    }
}

