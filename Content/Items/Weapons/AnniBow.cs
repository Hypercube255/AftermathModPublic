using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class AnniBow : ModItem
	{

		public override void SetDefaults()
		{
			Item.damage = 440;
			Item.width = 44;
			Item.height = 72;
			Item.useTime = 33;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 5;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<AnniBowProjectile>();
			Item.noMelee = true;
			Item.shootSpeed = 40;
            Item.value = 114000 * 5;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/AnniBow_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AntimatterCore>());
			recipe.AddIngredient(ModContent.ItemType<Hyperobsidian>());
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}	
}
