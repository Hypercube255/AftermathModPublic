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
	public class Snowblast : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 25;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Magic;
			Item.knockBack = 2;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HiddenExplosionSnow>();
			Item.noMelee = true;
			Item.mana = 7;
			Item.value = 500 * 5;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

			for (int i = 0; i < 123; i++)
			{
				Vector2 DustVelocity = Main.rand.NextVector2Unit()*14;
				Dust dust = Dust.NewDustDirect(position, 1, 1, DustID.Ice, DustVelocity.X, DustVelocity.Y, Scale: 1.7f);
				dust.noGravity = true;
			}

			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 25);
            recipe.AddIngredient(ItemID.SnowBlock, 25);
            recipe.AddIngredient(ItemID.Shiverthorn, 5);

			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}	
}
//3 tiers - 1. short range ring (from dusts), 2. this. 3. something krazy