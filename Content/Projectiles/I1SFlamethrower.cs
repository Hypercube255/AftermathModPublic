using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class I1SFlamethrower : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/VanillaFlamethrower";

        public override void SetStaticDefaults()
        {
			Main.projFrames[Type] = 7;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float OriginalSize = 1.2f;

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Rectangle ProjFrame = new Rectangle(0, Projectile.frame * 98, texture.Width, 98);
            Vector2 Origin = new Vector2(ProjFrame.Width * 0.5f, ProjFrame.Height * 0.5f);

            Vector2 DrawPositionMain = (Projectile.position - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

            for (float i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                float TrailSize = MathHelper.Lerp(OriginalSize, 0.6f, i / ProjectileID.Sets.TrailCacheLength[Projectile.type]);

                Color TrailColor = new Color(229, 64, 105) * MathHelper.Lerp(1f, 0, i / ProjectileID.Sets.TrailCacheLength[Projectile.type]);

                Vector2 DrawPosition = (Projectile.oldPos[(int)i] - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

                Main.EntitySpriteDraw(texture, DrawPosition, ProjFrame, TrailColor with { A = 0 }, Projectile.rotation, Origin, TrailSize, SpriteEffects.None);
            }

            Main.EntitySpriteDraw(texture, DrawPositionMain, ProjFrame, new Color(255, 135, 131) with { A = 0 }, Projectile.rotation, Origin, OriginalSize, SpriteEffects.None);

            return false;
        }

        public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.hostile = true;
			Projectile.light = 0f;
			Projectile.penetrate = -1;
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void AI()
		{
			if ((Projectile.frame + 1) >= Main.projFrames[Type])
			{
				Projectile.Kill();
			}

            if (++Projectile.frameCounter >= 3) // Animation speed dictates max time
            {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }

            Projectile.rotation += 0.075f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Projectile.WithinRange(targetHitbox.ClosestPointInRect(Projectile.Center), 45); // circular hitboxes <3 <3 <3
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}

