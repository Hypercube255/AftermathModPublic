using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class SGBigFireball : ModProjectile
	{
		float timer = 0;

        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 6;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (float i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Projectiles/SGBigFireball").Value;

                Color TrailColor = new Color(255, 167, 71) * MathHelper.Lerp(0.5f, 0, i / 5);

                Vector2 DrawPosition = (Projectile.oldPos[(int)i] - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
                Vector2 DrawPositionMain = (Projectile.position - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

                Rectangle ProjFrame = new Rectangle(0, Projectile.frame * 72, texture.Width, 70);
                Vector2 Origin = new Vector2(ProjFrame.Width * 0.5f, ProjFrame.Height * 0.5f + 18);

                Main.EntitySpriteDraw(texture, DrawPosition, ProjFrame, TrailColor with { A = 0 }, Projectile.rotation, Origin, 1, SpriteEffects.None, 0f);

                Main.EntitySpriteDraw(texture, DrawPositionMain, ProjFrame, Color.White, Projectile.rotation, Origin, 1, SpriteEffects.None, 0f);
            }

            return false;
        }

        public override void SetDefaults()
		{	
			Projectile.height = 26;
			Projectile.width = 26;
			Projectile.hostile = true;
			Projectile.penetrate = 5;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 240);
        }

        public override void AI()
		{
			DrawOriginOffsetY = 0;

            if (++Projectile.frameCounter >= 7)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

			timer++;
		}
	}
}

