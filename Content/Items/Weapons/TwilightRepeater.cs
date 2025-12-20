using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Projectiles;

namespace AftermathMod.Content.Items.Weapons
{
	public class TwilightRepeater : ModItem
	{
		int BonusArrow;
		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.width = 54;
			Item.height = 24;
			Item.useTime = 20;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 1.5f;
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.shootSpeed = 10.5f;
			Item.useAmmo = AmmoID.Arrow;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.value = 120000 * 5;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			BonusArrow = (damage - Item.damage) / 5; //every 5 extra dmg = 1 extra arrow

			for (int i = 0; i < BonusArrow; i++)
			{
                velocity *= Main.rand.NextFloat(0.95f, 1.05f);
                velocity = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f));

                Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
            }
			
			return true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ShadedBar>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}	
}
