using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Projectiles
{
	public class GloomDrillProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(23);
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.aiStyle = ProjAIStyleID.Drill;
			Projectile.ownerHitCheck = true;
			Projectile.hide = true;
			Projectile.tileCollide = false;
		}
    }
}

