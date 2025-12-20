using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
	public class SGFireVortex : ModProjectile
	{
        float FinalOpacity = 1f;
        float LifeTime = 720;

        float VelocityOriginal;
        int DamageOriginal;

        Vector2 FireballVelocity = new Vector2(0, 7);

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White * Projectile.Opacity, Projectile.rotation, texture.Size() * 0.5f, 1f * (Projectile.Opacity / FinalOpacity), SpriteEffects.None, 0f);

            return false;
        }

        int timer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        float IntroLength //passed in from dragon ai
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        int TargetIndex //passed in from dragon ai
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }
        public override void SetDefaults()
		{
			Projectile.height = 230;
			Projectile.width = 230;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.Opacity = 0;
            Projectile.aiStyle = -1;
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = new Color(255, 255, 255, 75);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 360);
        }

        public override void AI()
		{
            Player Target = Main.player[TargetIndex];

			Projectile.rotation += 0.15f;

			// makes it not do barrel rolls
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(25);
            DrawOriginOffsetY = -(25);

            timer++;

            if (timer < IntroLength)
            {
                if (timer == 1)
                {
                    VelocityOriginal = Projectile.velocity.Length();
                    DamageOriginal = Projectile.damage;
                }

                Projectile.damage = 0;

                Projectile.velocity = Vector2.Zero;

                Projectile.Opacity += (FinalOpacity / IntroLength);
            }
            else if (timer == IntroLength)
            {
                Projectile.damage = DamageOriginal;

                Projectile.Opacity = FinalOpacity;

                Projectile.velocity = Vector2.UnitY.RotatedBy(Projectile.AngleTo(Target.Center)) * VelocityOriginal;
            }
            else
            {
                float Angle = Projectile.AngleTo(Target.Center);
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Angle, 1).ToRotationVector2() * VelocityOriginal;

                if (timer % 150 == 0)
                {
                    FireballVelocity = FireballVelocity.RotatedBy(MathHelper.ToRadians(30));

                    Methods.SimpleProjectileSpread(Projectile.GetSource_FromThis(), 6, MathHelper.ToRadians(300), ModContent.ProjectileType<SGSmallFireball>(), FireballVelocity, Projectile.Center, 2, DamageOriginal);
                }

                if (timer > LifeTime)
                {
                    Projectile.damage = 0;

                    Projectile.Opacity -= 0.03f;

                    if (Projectile.Opacity <= 0)
                    {
                        Projectile.Kill();
                    }
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Projectile.WithinRange(targetHitbox.ClosestPointInRect(Projectile.Center), 140); // circular hitboxes <3 <3 <3
        }
    }
}

