using AftermathMod.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class CharybdisTeeth : ModProjectile
	{
        public override string Texture => "AftermathMod/Assets/EmptySprite";

		float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		int OriginalDamage
		{
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

		Vector2 StartOffset = Vector2.UnitY * 80;
		Vector2 EndOffset = Vector2.UnitY * 13;

		Vector2 TopOffset;
		Vector2 BottomOffset;

		Vector2 HitboxA;
		Vector2 HitboxB;

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.height = 1;
			Projectile.width = 1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.light = 0f;
			Projectile.penetrate = -1;
			Projectile.Opacity = 0;

			Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Color drawColor = new Color(255, 0, 50) * Projectile.Opacity;
            Texture2D Top = AssetDatabase.CharTeethTop.Value;
            Main.EntitySpriteDraw(Top, Projectile.Center + TopOffset - Main.screenPosition, null, drawColor with { A = 0 }, Projectile.rotation, Top.Size() * 0.5f, 1f, SpriteEffects.None);

            Texture2D Bottom = AssetDatabase.CharTeethBottom.Value;
            Main.EntitySpriteDraw(Bottom, Projectile.Center + BottomOffset - Main.screenPosition, null, drawColor with { A = 0 }, Projectile.rotation, Bottom.Size() * 0.5f, 1f, SpriteEffects.None);

            return false;
        }

		public override void AI()
		{
			int BiteDuration = 12;

			if (Timer == 0)
			{
                Projectile.damage = 0;
				SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
            }

			Timer++;

			if (Timer <= BiteDuration)
			{
				Projectile.Opacity += 0.1f;

				TopOffset = -Vector2.Lerp(StartOffset, EndOffset, Timer / BiteDuration).RotatedBy(Projectile.rotation);
                BottomOffset = Vector2.Lerp(StartOffset, EndOffset, Timer / BiteDuration).RotatedBy(Projectile.rotation);

                if (Timer == BiteDuration)
				{
					SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
				}
            }
			else if (Timer <= BiteDuration + 3)
			{
				Projectile.damage = OriginalDamage;
			}
			else
			{
				Projectile.damage = 0;
				Projectile.Opacity -= 0.04f;
			}

			if (Projectile.Opacity <= 0 && Timer > BiteDuration)
			{
				Projectile.Kill();
			}
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			HitboxA = Projectile.position + (Vector2.UnitX * 70).RotatedBy(Projectile.rotation);
            HitboxB = Projectile.position - (Vector2.UnitX * 70).RotatedBy(Projectile.rotation);

            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), HitboxA, HitboxB, 55, ref point);
        }
	}
}

