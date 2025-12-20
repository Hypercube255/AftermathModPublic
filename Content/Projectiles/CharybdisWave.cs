using AftermathMod.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class CharybdisWave : ModProjectile
	{
        Vector2 start;
        Vector2 end;

        public override void SetDefaults()
		{
			Projectile.height = 60;
			Projectile.width = 60;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.penetrate = 12;
			Projectile.Opacity = 0;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Color drawColor = new Color(0, 200, 255) * Projectile.Opacity;
            Texture2D texture1 = AssetDatabase.Trail1Big.Value;
            Main.EntitySpriteDraw(texture1, Projectile.Center - Main.screenPosition, null, drawColor with { A = 0 }, Projectile.rotation, new Vector2(texture1.Width, texture1.Height * 0.5f), 1f, SpriteEffects.None);

            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Projectiles/CharybdisWave", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None);

            return false;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceGolem, Scale: 1f);
            dust.noGravity = true;

            Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water_Snow, Scale: 1.5f);
            dust2.noGravity = true;

            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.ai[0]++;

            if (Projectile.Opacity < 1 && Projectile.ai[0] < 20)
            {
                Projectile.Opacity += 0.2f;
            }

            if (Projectile.ai[0] > 20)
            {
                Projectile.Opacity -= 0.2f;

                if (Projectile.Opacity <= 0)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Chilled, 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Chilled, 180);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) // hitbox = line that goes vertically through the front
        {
            start = Projectile.Center + new Vector2(30, 0).RotatedBy(Projectile.rotation + MathHelper.ToRadians(80));
            end = Projectile.Center + new Vector2(30, 0).RotatedBy(Projectile.rotation - MathHelper.ToRadians(80)); ;

            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 3, ref point);
        }
    }
}

