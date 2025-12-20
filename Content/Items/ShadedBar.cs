using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class ShadedBar : ModItem
	{

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/ShadedBar_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.Pink;
            Item.width = 30;
            Item.height = 24;
            Item.value = 12000 * 5;
        }
		public override void AddRecipes()
		{
			CreateRecipe(5)
                .AddIngredient(ItemID.HallowedBar, 3)
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddIngredient(ItemID.SoulofNight, 6)
				.Register();
		}
	}
}