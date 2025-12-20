using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class Hyperobsidian : ModItem
	{

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Hyperobsidian_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = Item.CommonMaxStack;
            Item.width = 30;
            Item.height = 30;
            Item.value = 22000 * 5;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Obsidian, 5);
			recipe.AddIngredient(ItemID.HellstoneBar, 3);
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddTile(TileID.Hellforge);
			recipe.Register();
		}
	}	
}
