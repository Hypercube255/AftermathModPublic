using AftermathMod.Content.Items;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Legs)]
	public class MushrootBoots : ModItem
	{
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Armor/MushrootBoots_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.buyPrice(gold: 1) * 5;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
		}
		public override void UpdateEquip(Player player)
		{
            player.moveSpeed += 0.18f; //=20%
		}
		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ReinforcedMushroot>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}