using System;
using System.Runtime.InteropServices;
using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod
{
    public partial class Methods
    {
        public static void SimpleProjectileSpread(EntitySource_ItemUse_WithAmmo source, float ProjectileCount, float Angle, int type, Vector2 velocity, Vector2 position, float knockback, int damage, Player player, float ai0 = 0, float ai1 = 0, float ai2 = 0)
        {
            float AngleOverTwo = Angle / 2;

            for (int i = 0; i < ProjectileCount; i++)
            {
                Vector2 projectileDirection = velocity.RotatedBy(MathHelper.Lerp(-AngleOverTwo, AngleOverTwo, i/(ProjectileCount-1)));

                    Projectile.NewProjectile(source, position, projectileDirection, type, damage, knockback, player.whoAmI, ai0: ai0, ai1: ai1, ai2: ai2);
            }
        }

        public static void SimpleProjectileSpread(IEntitySource source, float ProjectileCount, float Angle, int type, Vector2 velocity, Vector2 position, float knockback, int damage, float ai0 = 0, float ai1 = 0, float ai2 = 0)
        {
            float AngleOverTwo = Angle / 2;

            for (int i = 0; i < ProjectileCount; i++)
            {
                Vector2 projectileDirection = velocity.RotatedBy(MathHelper.Lerp(-AngleOverTwo, AngleOverTwo, i / (ProjectileCount - 1)));

                Projectile.NewProjectile(source, position, projectileDirection, type, damage, knockback, ai0: ai0, ai1: ai1, ai2: ai2);
            }
        }

        public static void SimpleRandomProjectileSpread(EntitySource_ItemUse_WithAmmo source, float ProjectileCount, float Angle, float SpeedDifference, Vector2 position, Vector2 velocity, int type, int damage, float knockback, Player player, float ai0 = 0, float ai1 = 0, float ai2 = 0)
        {
            float AngleOverTwo = Angle / 2;

            for (int i = 0; i < ProjectileCount; i++)
            {
                Vector2 PreProjectileDirection = velocity.RotatedByRandom(Angle);
                PreProjectileDirection.RotatedBy(-AngleOverTwo);

                Vector2 projectileDirection = PreProjectileDirection * Main.rand.NextFloat(1 - SpeedDifference, 1 + SpeedDifference);



                Projectile.NewProjectile(source, position, projectileDirection, type, damage, knockback, player.whoAmI, ai0: ai0, ai1: ai1, ai2: ai2);
            }
        }

        public static void SimpleRandomProjectileSpread(IEntitySource source, float ProjectileCount, float Angle, float SpeedDifference, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float ai0 = 0, float ai1 = 0, float ai2 = 0)
        {
            float AngleOverTwo = Angle / 2;

            for (int i = 0; i < ProjectileCount; i++)
            {
                Vector2 PreProjectileDirection = velocity.RotatedByRandom(Angle);
                PreProjectileDirection.RotatedBy(-AngleOverTwo);

                Vector2 projectileDirection = PreProjectileDirection * Main.rand.NextFloat(1 - SpeedDifference, 1 + SpeedDifference);



                Projectile.NewProjectile(source, position, projectileDirection, type, damage, knockback, ai0: ai0, ai1: ai1, ai2: ai2);
            }
        }

        public static float GetCircleSpeed(float radius, float RpS)
        {
            float circumference = radius * MathHelper.TwoPi;

            float Speed60Hz = circumference * RpS / 60;

            return Speed60Hz;
        }

        public static float GetAngularVelocity(float RpS)
        {
            float w = MathHelper.TwoPi * RpS/60;
            return w;
        }

        public static Vector2 BossSmoothMoveTo(int npc, Vector2 TargetPos, float SpeedCap)
        {
            NPC Npc = Main.npc[npc];

            Vector2 position = Npc.Center;

            float TargetAngle = Npc.AngleTo(TargetPos);
            Vector2 velocity = Npc.velocity.ToRotation().AngleTowards(TargetAngle, 1).ToRotationVector2() * ((position.Distance(TargetPos) * position.Distance(TargetPos) / 1500));

            if (velocity.Length() > SpeedCap)
            {
                velocity = velocity.SafeNormalize(new(1, 1)) * SpeedCap;
            }

            return velocity;
        }
        /// <summary>
        /// Vanilla messes up hostile projectile damage - this should undo it
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public static int UnvanillaDamage(int damage)
        {
            float Damage = damage * 0.5f;

            if(Main.expertMode)
            {
                Damage *= 0.5f;
            }

            return (int)Damage;
        }
    }
}
