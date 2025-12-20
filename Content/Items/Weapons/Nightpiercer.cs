using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class Nightpiercer : ModItem
	{
		int BaseDMG = 60;

		public override void SetDefaults()
		{
			Item.damage = BaseDMG;
			Item.DamageType = DamageClass.Melee;
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 13;
			Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 5;
			Item.value = 108000 * 5;
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<NightpiercerProjectile>();
			Item.shootSpeed = 2.4f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int AdditionalDMG = damage - BaseDMG;

			float ExtraPierce = AdditionalDMG / 5;
			float ExtraVelocity = 1 + (AdditionalDMG * 0.05f);
            if (player.itemAnimation == player.itemAnimationMax) //1 projectile per click
			{
                Projectile.NewProjectile(source, position, (velocity * 4) * ExtraVelocity, ModContent.ProjectileType<NightpiercerBall>(), damage, 2f, ai0: ExtraPierce);
            }
			return true;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ShadedBar>(), 9);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}