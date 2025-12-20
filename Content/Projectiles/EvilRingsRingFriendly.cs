using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AftermathMod.Content.Items;
using Microsoft.Xna.Framework.Graphics;

namespace AftermathMod.Content.Projectiles
{
	public class EvilRingsRingFriendly : ModProjectile
	{
		double timer = 0.011;

        public override void SetDefaults()
		{
            Projectile.height = 33;
			Projectile.width = 33;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (timer == 0.011)
			{
                timer = Projectile.position.X < player.position.X ? MathHelper.Pi : 0;
            }

			timer += 0.07;

			if(timer > 31412)
			{
				timer = 0;
			}

            Vector2 PlayerCenter = player.RotatedRelativePoint(player.MountedCenter);

                Vector2 SavePosition = Projectile.Center - PlayerCenter;

                Projectile.Center = PlayerCenter - SavePosition;

			Projectile.Center = PlayerCenter - new Vector2((float)System.Math.Cos(timer), (float)System.Math.Sin(timer)) * 200;

			Projectile.rotation = (float)timer;

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain);

			if (player.GetModPlayer<AftermathPlayer>().RingBraceletBuff == false)
			{
				Projectile.Kill();
			}
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}

