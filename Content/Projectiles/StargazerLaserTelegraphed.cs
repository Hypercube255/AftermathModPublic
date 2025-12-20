using AftermathMod.Content.StatusFX.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class StargazerLaserTelegraphed : ModProjectile
	{
        public override string Texture => "AftermathMod/Content/Projectiles/StargazerLaser";

        int indexParentNPC //passed in whoAmI from boss instance, input "123456" to make it independent
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

		float timer
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
        float state
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        const int TelegraphFadeOut = 5;
        int TelegraphFadeOutTimer = 5;

        int TelegraphDuration = 30; //tied to the boss value, don't forget

        int DamageOriginal = 0;
		Vector2 VelocityOriginal = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4800;// 300 blocks offscreen
        }

        public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

        public override void AI()
		{
            //---TELEGRAPH---

            timer++;

            NPC Parent = null;

            if(indexParentNPC != 123456)
            {
                Parent = Main.npc[indexParentNPC];
            }
            else
            {
                Parent = Main.npc[1];
            }

			switch(state)
			{
				case 0://telegraph prep

                    DamageOriginal = Projectile.damage;
                    VelocityOriginal = Projectile.velocity;

                    Projectile.velocity = Vector2.Zero;
                    Projectile.damage = 0;

					state = 1;
					timer = 0;

                    Projectile.velocity = Parent.velocity;

                    if (indexParentNPC == 123456)
                    {
                        Projectile.velocity = Vector2.Zero;
                    }

                    break;

				case 1://telegraphin'

                    Projectile.velocity = Parent.velocity;

                    if (indexParentNPC == 123456)
                    {
                        Projectile.velocity = Vector2.Zero;
                    }

                    if (timer >= TelegraphDuration)
					{
						state = 2;
						timer = 0;
					}
					break;

				case 2://actual AI

                    Projectile.damage = DamageOriginal;
                    Projectile.velocity = VelocityOriginal;

                    DrawOriginOffsetX = -106;

                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(180);

                    if (timer > 5)
                    {
                        Projectile.alpha -= 20;
                    }
                    break;
            }
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 1f) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float opacity = MathHelper.Lerp(0, 1, TelegraphFadeOutTimer / (float)TelegraphFadeOut);

            Color drawColor = new Color(91, 132, 255, 100) * opacity;

            if (state != 2)
            {
                Methods.DrawTelegraph(2, Projectile.Center, Projectile.Center + VelocityOriginal * 150, drawColor with { A = 0 }, 0.75f);

                if (timer >= TelegraphDuration - TelegraphFadeOut)
                {
                    TelegraphFadeOutTimer--;
                }
            }

            return state == 2;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<PainfulEnlightenment>(), 180);
        }
    }
}

