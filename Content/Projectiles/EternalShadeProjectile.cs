using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class EternalShadeProjectile : ModProjectile
	{
        private NPC TargetNPC;
		public override void SetDefaults()
		{
			
			Projectile.CloneDefaults (ProjectileID.Bullet);
			AIType = ProjectileID.Bullet;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
            Projectile.penetrate = 2;

            DrawOffsetX = -7;
            DrawOriginOffsetY = -4;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void AI()
		{
		Projectile.rotation += MathHelper.ToRadians(-90);
			if (Main.rand.NextFloat() < 0.6)
			{
				Dust.NewDustPerfect(Projectile.Center, 309, Projectile.velocity*0, 0, new Color(255,255,255), Scale: 1.2f);
			}

            float HomingRange = 444f;

            if (TargetNPC == null)
            {
                TargetNPC = FindTarget(HomingRange);
            }

            if (TargetNPC != null && !IsTargetable(TargetNPC))
            {
                TargetNPC = null;
            }

            if (TargetNPC == null)
            {
                return;
            }

            float LengthOrgnl = Projectile.velocity.Length();
            float TargetAngle = Projectile.AngleTo(TargetNPC.Center);

            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(TargetAngle, MathHelper.ToRadians(6)).ToRotationVector2() * LengthOrgnl;
            Projectile.rotation = Projectile.velocity.ToRotation();

        }
        public NPC FindTarget(float HomingRange)
        {
            NPC Target = null;

            float RangeSquared = HomingRange * HomingRange;

            foreach (var Target2 in Main.ActiveNPCs)
            {
                if (IsTargetable(Target2))
                {
                    float TargetDistanceSquared = Vector2.DistanceSquared(Target2.Center, Projectile.Center);
                    if (TargetDistanceSquared < RangeSquared)
                    {
                        RangeSquared = TargetDistanceSquared;
                        Target = Target2;
                        return Target;
                    }
                }
            }
            return null;
        }
        public bool IsTargetable(NPC Target2)
        {
            return Target2.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, Target2.position, Target2.width, Target2.height);
        }
    }
}

