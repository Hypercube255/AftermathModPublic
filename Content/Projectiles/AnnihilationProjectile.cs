using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class AnnihilationProjectile : ModProjectile
	{
        private NPC TargetNPC;
        public override void SetDefaults()
		{
			
			Projectile.CloneDefaults (ProjectileID.Bullet);
			AIType = ProjectileID.Bullet;
			Projectile.height = 25;
			Projectile.width = 25;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.penetrate = 7;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 4;

            DrawOffsetX = -35;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new(255, 255, 255, 255);
        }

        public override void AI()
		{
			if (Main.rand.NextFloat() < 0.6)
			{

                Dust.NewDustPerfect(Projectile.Center, 309, Projectile.velocity * 0, 0, new Color(255, 0, 0), Scale: 1.2f);
            }

            float HomingRange = 600f;

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

            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(TargetAngle, MathHelper.ToRadians(5)).ToRotationVector2() * LengthOrgnl;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

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
            return Target2.CanBeChasedBy();
        }
    }
}

