using AftermathMod.Content.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class TreasureBagEverlastingFlameI1S : ModItem
	{
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/TreasureBagEverlastingFlameI1S_glow").Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetStaticDefaults()
        {
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;

			Item.ResearchUnlockCount = 3;
        }
        public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
            Item.width = 32;
            Item.height = 32;
			Item.consumable = true;
			Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BurningEmbers>(), 1, 13, 17));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Voucher>()));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<JournalPage1>()));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<EverlastingFlameI1S>()));
        }
    }	
}
