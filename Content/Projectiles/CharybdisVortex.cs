using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AftermathMod.Content.Projectiles
{
	public class CharybdisVortex : ModProjectile
	{
        public override void SetDefaults()
		{
			Projectile.height = 250;
			Projectile.width = 250;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 75;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.Opacity = 0.01f;
		}


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D vortex = ModContent.Request<Texture2D>("AftermathMod/Content/Projectiles/CharybdisVortex", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(vortex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity * 0.75f, Projectile.rotation, vortex.Size() * 0.5f, 1f, SpriteEffects.None);

            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D sword = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Charybdis", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(sword, Projectile.Center - Main.screenPosition, null, Color.Black * Projectile.Opacity * 0.75f, Projectile.rotation * 1.5f, sword.Size() * 0.5f, 1.5f, SpriteEffects.None);
        }

        public override void AI()
		{
            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<AftermathPlayer>().CharybdisVortexUp = true;

            player.AddBuff(BuffID.Slow, 2);

            // makes it not do barrel rolls
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(25);
            DrawOriginOffsetY = -(25);

            Projectile.rotation += 0.08f;
			Projectile.Center = player.Center;


            if (Main.mouseRight)
            {
                Projectile.Opacity += 0.07f;
            }
            else
			{
				Projectile.Opacity -= 0.2f;
			}

			if (Projectile.Opacity <= 0)
			{
                Projectile.Kill();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Projectile.WithinRange(targetHitbox.ClosestPointInRect(Projectile.Center), 150); // circular hitboxes <3 <3 <3
        }

        public override void OnKill(int timeLeft)
        {
            Main.player[Projectile.owner].GetModPlayer<AftermathPlayer>().CharybdisVortexUp = false;
        }
	}
}

