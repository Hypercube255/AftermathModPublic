using System;
using AftermathMod.Content.StatusFX.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class StargazerSparkle : ModProjectile
	{
		float timer = 0;

		float DetectionRange = 3000f;

		float MaxLaunchSpeed
		{
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        float State
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

        int ParentDamage
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

		float Accel = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            for (float i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Color drawColor = new Color(255, 255, 255, 255) * MathHelper.Lerp(1, 0, i / 4) * Projectile.Opacity;

                Vector2 DrawPosition = (Projectile.oldPos[(int)i] - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
                Vector2 DrawPositionBase = (Projectile.position - Main.screenPosition) + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

                Main.EntitySpriteDraw(texture, DrawPosition, null, drawColor, Projectile.rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                Main.EntitySpriteDraw(texture, DrawPositionBase, null, Color.White * Projectile.Opacity, Projectile.rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
			return false;
        }

        public override void SetDefaults()
		{
			Projectile.height = 30;
			Projectile.width = 30;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.damage = 0;
		}

        public override void AI()
		{
            timer++;

			if(State == 0 && timer == 2)// == 2 so they dont spawn Dust even when destroyed by distance check
			{
                for (int i = 0; i < 25; i++)
                {
                    Vector2 DustVelocity = Main.rand.NextVector2Circular(2, 2);

                    DustVelocity = DustVelocity.RotatedByRandom(Math.PI);
                    Dust.NewDust(Projectile.Center, 0, 0, DustID.Clentaminator_Blue, DustVelocity.X, DustVelocity.Y, Scale: 1f);

                }
            }

			Projectile.rotation += Projectile.velocity.Length() / 100;

            Vector2 TargetPos = Vector2.Zero;
			Vector2 TargetVel = Vector2.Zero;

            if (State == 0)
			{
                TargetPos = FindTargetPosition(DetectionRange);

				if(Projectile.Center.Distance(TargetPos) < 400f && timer == 1)
				{
					Projectile.Kill();
				}

                Projectile.alpha -= 10;

                if (timer >= 30)
				{
					State = 1;
					timer = 0;
					Projectile.damage = ParentDamage;
				}
            }
			else if(State == 1)
			{
				if(timer == 1)
				{
                    TargetPos = FindTargetPosition(DetectionRange);
                    TargetVel = Projectile.Center.DirectionTo(TargetPos);

					Accel = 1 + MaxLaunchSpeed * 0.0016f;
				}

				if(Projectile.velocity.Length() < MaxLaunchSpeed)
				{
					Projectile.velocity += TargetVel * 2f;
                    Projectile.velocity *= Accel;
                }
			}
		}

		public Vector2 FindTargetPosition(float Range)
		{
			float RangeSquared = Range * Range;

			foreach (var Target2 in Main.player)
			{
				float DistanceSquared = Vector2.DistanceSquared(Projectile.Center, Target2.Center);

				if(RangeSquared > DistanceSquared)
				{
					return Target2.Center;
				}
			}

			return Vector2.Zero;
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<PainfulEnlightenment>(), 180);
        }
	}
}

