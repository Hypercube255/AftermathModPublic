using AftermathMod.Content.Dusts;
using AftermathMod.Content.Projectiles;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class Tenebris : ModItem
	{
		int AdditionalDMG;

		int BaseDMG = 66;//change in projectile too
		float Scale;

		public override void SetDefaults()
		{
			Item.damage = BaseDMG;
			Item.width = 68;
			Item.height = 68;
			Item.useTime = 24;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 7f;
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TenebrisSwingProjectile>();
			Item.noMelee = true;
			Item.shootsEveryUse = true;
			Item.value = 144000 * 5;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			AdditionalDMG = damage - BaseDMG;

			Scale = player.GetAdjustedItemScale(Item);

			Projectile.NewProjectile(source, player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<TenebrisSwingProjectile>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, Scale, player.itemAnimationMax);


            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
			float StartScale = 0.25f;

			float DustSize = AdditionalDMG * 0.04f;

			if (DustSize > 5)
			{
				DustSize = 5;
			}

			float ScaleFinal = StartScale + DustSize;
			player.GetModPlayer<AftermathPlayer>().TenebrisDustSize = ScaleFinal; //sent to the player bc the projectile spawns them instead
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			int StartTime = 30;
			int ExtraTime = AdditionalDMG * 6;

			target.AddBuff(BuffID.ShadowFlame, StartTime + ExtraTime);

            if (player.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus + 0.15f < 2)
            {
                player.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus += 0.15f;
            }
			else
			{
				player.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus = 2;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            int StartTime = 30;
            int ExtraTime = AdditionalDMG * 6;

            target.AddBuff(BuffID.ShadowFlame, StartTime + ExtraTime);

            if (player.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus + 0.15f < 2)
            {
                player.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus += 0.15f;
            }
            else
            {
                player.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus = 2;
            }
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return player.GetModPlayer<AftermathPlayer>().TenebrisSpeedBonus;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ShadedBar>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}	
}