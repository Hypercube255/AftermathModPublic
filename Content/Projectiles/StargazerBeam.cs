using AftermathMod.Assets;
using AftermathMod.Content.StatusFX.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Projectiles
{
    public class StargazerBeam : ModProjectile
    {

        int indexParent //passed in whoAmI from boss instance
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

        NPC Parent = null;

        Vector2 ShootOffset;

        public override string Texture => "AftermathMod/Assets/EmptySprite";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4800;// 300 blocks offscreen
        }

        public override void SetDefaults()
		{
			Projectile.height = 30;
			Projectile.width = 30;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.hostile = true;
            Projectile.damage = 1;
		}
		public override void AI()
		{
            AttackDuration--;

            Parent = Main.npc[indexParent];

            ShootOffset = Vector2.UnitY.RotatedBy(Parent.rotation) * 75;

            Projectile.position = Parent.Center + ShootOffset;

            if (AttackDuration <= 0)
            {
                Projectile.Kill();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float Point = 0f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.position, Projectile.position + ShootOffset * 50, 40, ref Point);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<PainfulEnlightenment>(), 300);
        }
    }
}

