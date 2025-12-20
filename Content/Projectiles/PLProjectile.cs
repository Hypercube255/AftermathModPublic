using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Threading;

namespace AftermathMod.Content.Projectiles
{
	public class PLProjectile : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        int timer = 0;
		int ChooseDustColor = Main.rand.Next(1, 4);
		Color DustColor;
        Vector2 offset = new(2, 2);
        public override void SetDefaults()
		{
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			timer++;

			switch (ChooseDustColor)
			{
				case 1:
					DustColor = new(200, 255, 255);
					break;

                case 2:
                    DustColor = new(200, 255, 255);
                    break;

                case 3:
                    DustColor = new(220, 200, 255);
                    break;
            }

			if (Main.rand.NextFloat() < 0.6)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position + offset, Projectile.width-4, Projectile.height-4, DustID.PortalBoltTrail, 0f, 0f, 0, DustColor, Scale: 1.2f);
			}
        }
	}
}

