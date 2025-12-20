using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AftermathMod.Content.Dusts;

namespace AftermathMod.Content.Projectiles
{
	public class NightpiercerProjectile : ModProjectile
	{
		public int timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public const int TotalTime = 16;
		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(22);
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.ownerHitCheck = true;
			Projectile.extraUpdates = 1;
			Projectile.hide = true;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (timer <= 5)
			{
				Projectile.alpha = 255;
			}
			else
			{
				Projectile.alpha = 0;
				if (Main.rand.NextFloat() < 0.75f)
				{
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ShadedSparkles>(), Projectile.velocity.X, Projectile.velocity.Y, Scale: 1f);
                }
            }


            timer += 1;
			if(timer >= TotalTime)
			{
				Projectile.Kill();
				return;
			}
			else
            {
				player.heldProj = Projectile.whoAmI;
			}

			Vector2 PlayerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
			Projectile.Center = PlayerCenter + Projectile.velocity * (timer-1);

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;


			int HalfSprite = 17;
			int HalfHitboxX = Projectile.width / 2;
            int HalfHitboxY = Projectile.height / 2;

			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSprite - HalfHitboxX);
            DrawOriginOffsetY = -(HalfSprite - HalfHitboxY);
        }

        public override bool ShouldUpdatePosition()
        {
			return false;
        }

        public override void CutTiles()
        {
			DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;

			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * 7.5f, 10 * Projectile.scale, DelegateMethods.CutTiles);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * 7.5f, 10, ref collisionPoint);
        }

    }
}

