using AftermathMod.Content.StatusFX.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class StargazerBlast : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/AmbiguousLaserBulletBig";

        int timer = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 position = (Projectile.position - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

            for (float i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Color drawColorNormal = new Color(0.36f, 0.52f, 1f, 0.39f);

                Color drawColor = new Color(0.36f, 0.52f, 1f, 0.29f) * MathHelper.Lerp(1, 0, i / 7);

                Vector2 DrawPosition = (Projectile.oldPos[(int)i] - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

                bool RandomFlip = Main.rand.NextBool();

                Main.EntitySpriteDraw(texture, DrawPosition, null, drawColor, Projectile.rotation, new Vector2(7 + Projectile.width * 0.5f, texture.Height * 0.5f), 1f, RandomFlip ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                Main.EntitySpriteDraw(texture, position, null, drawColorNormal, Projectile.rotation, new Vector2(7 + Projectile.width * 0.5f, texture.Height * 0.5f), 1f, RandomFlip ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }
            return false;
        }

        public override void SetDefaults()
		{
			
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
		}

        public override void AI()
		{
            timer++;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(180);

            Projectile.velocity *= 1.02f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<PainfulEnlightenment>(), 180);
        }
    }
}

