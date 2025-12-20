using AftermathMod.Content.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class MushroomNucleus : ModItem
	{

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/MushroomNucleus", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 1;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
            Item.width = 48;
            Item.height = 48;
        }
        public override bool CanUseItem(Player player)
        {
			return Condition.InBelowSurface.IsMet() && Condition.InGlowshroom.IsMet() && !NPC.AnyNPCs(ModContent.NPCType<SporeGuardian>());
        }
        public override bool? UseItem(Player player)
        {
			SoundEngine.PlaySound(SoundID.Roar);
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<SporeGuardian>());
			return true;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GlowingMushroom, 21);
            recipe.AddIngredient(ItemID.Fertilizer, 1);
			recipe.Register();
        }
	}	
}
