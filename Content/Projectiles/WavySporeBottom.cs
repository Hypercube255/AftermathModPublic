using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AftermathMod.Content.Dusts;
using System;

namespace AftermathMod.Content.Projectiles
{
	public class WavySporeBottom : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

        float timer;
		float OriginalPosition;

        Vector2 offset = new(2, 2);

        public override void SetDefaults()
		{
			Projectile.height = 25;
			Projectile.width = 25;
			Projectile.hostile = true;
			Projectile.alpha = 255;
            Projectile.tileCollide = false;
        }
		public override void AI()
		{
			timer += 0.03f;
			if(timer == 0.03f)
			{
				OriginalPosition = Projectile.position.Y;
			}

			Projectile.position.Y = OriginalPosition + (float)Math.Sin(timer) * 350;

			Dust.NewDust(Projectile.position-offset, Projectile.width+4, Projectile.height+4, ModContent.DustType<Spores>(), Scale: 2.5f);

			if(timer>36)
			{
				Projectile.Kill();
			}
        }
	}
}

