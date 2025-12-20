using AftermathMod.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
    public class DrawHelper : ModProjectile
    {

        int indexTarget //passed in from spawn
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        float AttackDuration
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        float timer
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        float RingOpacity = 0;
        Player Target;

        public override string Texture => "AftermathMod/Assets/EmptySprite";

        public override void SetDefaults()
        {
            Projectile.height = 1;
            Projectile.width = 1;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Target = Main.player[indexTarget];

            Projectile.position = Target.position;

            timer++;
            if (timer <= 25)
            {
                RingOpacity += 0.04f;
            }

            if (AttackDuration - timer <= 25)
            {
                RingOpacity -= 0.04f;
            }

            if (timer >= AttackDuration)
            {
                Projectile.Kill();
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D ring = AssetDatabase.RingTelegraph1.Value;

            float RingSpin = timer * 0.01f;

            Color color = new Color(91, 132, 255, 100) * RingOpacity;

            Main.EntitySpriteDraw(ring, new Vector2(Target.Center.X - Main.screenPosition.X, Target.Center.Y - Main.screenPosition.Y), null, color with { A = 0 }, RingSpin, ring.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

            return false;
        }
    }
}

