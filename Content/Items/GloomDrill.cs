using System;
using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class GloomDrill : ModItem
	{
        public override void SetStaticDefaults()
        {
			ItemID.Sets.IsDrill[Type] = true;
        }
		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.width = 50;
			Item.height = 24;
			Item.useTime = 3;
			Item.useAnimation = 13;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3f;
			Item.value = 144000 * 5;
			Item.rare = ItemRarityID.LightPurple;
			Item.autoReuse = true;
			Item.tileBoost = -2;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<GloomDrillProjectile>();
			Item.shootSpeed = 38f;
			Item.channel = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item23;

			Item.pick = 200;
			Item.attackSpeedOnlyAffectsWeaponAnimation = true;
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