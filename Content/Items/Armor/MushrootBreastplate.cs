using AftermathMod.Content.Items;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Body)]
	public class MushrootBreastplate : ModItem
	{
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Armor/MushrootBreastplate_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 28;
			Item.value = Item.buyPrice(silver: 120) * 5;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 8;
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = false;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = false;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 8;
			player.lifeRegen = 2;
		}
		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ReinforcedMushroot>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}