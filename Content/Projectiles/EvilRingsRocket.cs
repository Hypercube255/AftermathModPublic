using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AftermathMod.Content.NPCs;
using Terraria.DataStructures;

namespace AftermathMod.Content.Projectiles
{
	public class EvilRingsRocket : ModProjectile
	{
		Vector2 Target;
		Player TargetCheck = null;
		int TargetIndex;


		float AITimer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
        public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.height = 14;
			Projectile.width = 14;
			Projectile.hostile = true;
		}

		public override void AI()
		{
			AITimer++;

			if(Main.rand.NextBool(3))
			{
                Dust.NewDustPerfect(Projectile.Center, DustID.OrangeStainedGlass);
            }

				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active == true && !Main.player[i].dead)
					{
						Target = Main.player[i].Center;

						TargetCheck = Main.player[i];

						TargetIndex = i;
					}
				}

			if (!Main.player[TargetIndex].dead && AITimer < 200)
			{
                float LengthOrgnl = Projectile.velocity.Length();
                float TargetAngle = Projectile.AngleTo(Target);

                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(TargetAngle, MathHelper.ToRadians(1.3f)).ToRotationVector2() * LengthOrgnl;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (AITimer > 600)
            {
                Projectile.Kill();
            }
        }
    }
}

