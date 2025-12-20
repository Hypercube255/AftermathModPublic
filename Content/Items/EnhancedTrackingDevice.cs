using AftermathMod.Content.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class EnhancedTrackingDevice : ModItem
	{

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/TrackingDevice_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 1;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
            Item.width = 40;
            Item.height = 46;
        }
        public override bool CanUseItem(Player player)
        {
			return !Main.IsItDay() && !NPC.AnyNPCs(ModContent.NPCType<EvilRings>());
        }
        public override bool? UseItem(Player player)
        {
			SoundEngine.PlaySound(SoundID.Roar);
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<EvilRings>());
			return true;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TrackingDevice>());
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddRecipeGroup(RecipeSystem.SilverBar, 10);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
        }
	}	
}
