using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class AntimatterCore : ModItem
	{

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/AntimatterCore_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Red;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 92000 * 5;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(ItemID.FragmentSolar, 4);
			recipe.AddIngredient(ItemID.FragmentNebula, 4);
			recipe.AddIngredient(ItemID.FragmentStardust, 4);
			recipe.AddIngredient(ItemID.FragmentVortex, 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}	
}
