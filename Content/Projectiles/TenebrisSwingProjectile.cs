using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AftermathMod.Content.Dusts;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;

namespace AftermathMod.Content.Projectiles
{
	public class TenebrisSwingProjectile : ModProjectile
	{//982id
        public override string Texture => "Terraria/Images/Projectile_982";

        int Direction
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
		float Opacity
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        float AdjustedScale
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        float Lifetime
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        float Timer
        {
			get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public override void SetStaticDefaults()
        {
			Main.projFrames[Type] = 4;
        }

		float SparkleSize;
		Player owner;

        public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 5;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;

			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			Projectile.ownerHitCheck = true;
			Projectile.ownerHitCheckDistance = 400;

			Projectile.usesOwnerMeleeHitCD = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;

			Projectile.noEnchantmentVisuals = true;
		}

		public override void AI()
		{
			Timer++;
			if (Timer > Lifetime)
			{
				Projectile.Kill();
			}

			owner = Main.player[Projectile.owner];
			float LifetimeRatio = Timer / Lifetime;

			Projectile.Center = owner.RotatedRelativePoint(owner.MountedCenter);

			float StartRotation = Direction == 1 ? 0 : -MathHelper.Pi;

			Projectile.rotation = StartRotation + float.Lerp(0, MathHelper.Pi * Direction * 1.05f, LifetimeRatio);

			if (LifetimeRatio < 0.5f)
			{
				Opacity += 2 / Lifetime;
			}
			else
			{
                Opacity -= 2 / Lifetime;
            }

			SparkleSize = 1 + ((float)Math.Sin(Timer / 2) + 1) / 4; //1 - 1.5

			Projectile.scale = float.Lerp(1.2f, 1.8f, LifetimeRatio);
            Projectile.scale *= AdjustedScale;

			//my dust
			Vector2 SwingLineDust = owner.Center + Projectile.rotation.ToRotationVector2() * -Main.rand.NextFloat(81 * Projectile.scale + 10);
			Vector2 DustVelocity = (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * 5 * -Direction;

            Dust.NewDust(SwingLineDust, 0, 0, ModContent.DustType<ShadedSparkles>(), DustVelocity.X, DustVelocity.Y, Scale: owner.GetModPlayer<AftermathPlayer>().TenebrisDustSize);

			//flask dust
			Vector2 FlaskBoxPos = owner.Center + Projectile.rotation.ToRotationVector2() * -40 * Projectile.scale;

			Rectangle rectangle = Utils.CenteredRectangle(FlaskBoxPos, new Vector2(75 * Projectile.scale, 75 * Projectile.scale));

			Projectile.EmitEnchantmentVisualsAt(rectangle.TopLeft(), rectangle.Width, rectangle.Height);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			float Length = 81 * Projectile.scale;

            return targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, Length, Projectile.rotation - MathHelper.Pi, MathHelper.ToRadians(75));
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Rectangle source = texture.Frame(1, 4, 0, 0);
			Rectangle sourceLine = texture.Frame(1, 4, 0, 3);
            Vector2 origin = source.Size() * 0.5f;
			SpriteEffects spriteEffects = Direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically; 

			Color color1 = Color.White;
			Color color2 = Color.LightPink;
            Color color3 = Color.Purple;
            Color color4 = Color.Violet;

			//Energy swing
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, source, color3 with { A = 100 } * Opacity, Projectile.rotation - MathHelper.Pi, origin, Projectile.scale, spriteEffects);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, source, color2 with { A = 100 } * Opacity, Projectile.rotation - MathHelper.Pi + 0.2f * Direction, origin, Projectile.scale, spriteEffects);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, source, color4 with { A = 100 } * Opacity, Projectile.rotation - MathHelper.Pi - 0.2f * Direction, origin, Projectile.scale, spriteEffects);
			//Lines
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceLine, color1 with { A = 0 } * Opacity, Projectile.rotation - MathHelper.Pi, origin, Projectile.scale, spriteEffects);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceLine, color1 with { A = 0 } * Opacity * 0.75f, Projectile.rotation - MathHelper.Pi, origin, Projectile.scale * 0.75f, spriteEffects);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceLine, color1 with { A = 0 } * Opacity * 0.5f, Projectile.rotation - MathHelper.Pi, origin, Projectile.scale * 0.5f, spriteEffects);

            //Sparkle
            Texture2D textureSparkle = TextureAssets.Extra[ExtrasID.SharpTears].Value;
			Vector2 VerticalScale = new Vector2(0.5f, 2);
            Vector2 HorizontalScale = new Vector2(0.5f, 1.5f);
			Vector2 origin2 = textureSparkle.Size() * 0.5f;
			Vector2 Position2 = (Projectile.rotation + 0.4f * Direction).ToRotationVector2() * 81 * Projectile.scale;

            Main.EntitySpriteDraw(textureSparkle, owner.Center - Position2 - Main.screenPosition, null, color1 with { A = 100 } * Opacity, 0, origin2, VerticalScale * SparkleSize, SpriteEffects.None);
            Main.EntitySpriteDraw(textureSparkle, owner.Center - Position2 - Main.screenPosition, null, color1 with { A = 100 } * Opacity, MathHelper.PiOver2, origin2, HorizontalScale * SparkleSize, SpriteEffects.None);

            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int AdditionalDMG = Projectile.damage - 66; //tied to the item's base damage

            int StartTime = 30;
            int ExtraTime = AdditionalDMG * 6;

            target.AddBuff(BuffID.ShadowFlame, StartTime + ExtraTime);

            if (target.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus + 0.15f < 2)
            {
                target.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus += 0.15f;
            }
            else
            {
                target.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus = 2;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int AdditionalDMG = Projectile.damage - 66; //tied to the item's' base damage

            int StartTime = 30;
            int ExtraTime = AdditionalDMG * 6;

            target.AddBuff(BuffID.ShadowFlame, StartTime + ExtraTime);

            if (owner.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus + 0.15f < 2)
            {
                owner.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus += 0.15f;
            }
            else
            {
                owner.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus = 2;
            }
        }
    }
}

