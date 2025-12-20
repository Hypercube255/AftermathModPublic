using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Projectiles;
using rail;

namespace AftermathMod.Content.Items.Weapons
{
	public class TidalWrath : ModItem
	{
		Vector2 DustVelocity;

		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.width = 72;
			Item.height = 72;
			Item.useTime = 35;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 4.5f;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<TidalWave>();
			Item.shootSpeed = 10;
            Item.value = 12000*5;
        }

        public override bool? UseItem(Player player)
        {
			SoundEngine.PlaySound(SoundID.Item84, player.Center);

			for (int i = 0; i < 20; i++)
			{
				DustVelocity = Main.rand.NextVector2Unit() * 7;
				Dust dust = Dust.NewDustPerfect(player.Center, DustID.BlueCrystalShard, DustVelocity, Scale: 1.5f);
				dust.noGravity = true;
            }

            return true;
        }

        public override void HoldItem(Player player)
        {
			if (Collision.DrownCollision(player.position, player.width, player.height))
			{
				player.statDefense += 10;
				Item.useTime = 28;
				Item.useAnimation = Item.useTime;
			}
			else
			{
                Item.useTime = 35;
                Item.useAnimation = Item.useTime;
            }
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Hydroscales>(), 6); //6000
            recipe.AddIngredient(ItemID.LimeKelp); //2000
            recipe.AddIngredient(ItemID.Seashell, 8); //4000
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 10);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}	
}//make the projectile