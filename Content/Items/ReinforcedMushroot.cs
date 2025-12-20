using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class ReinforcedMushroot : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.Orange;
            Item.width = 46;
            Item.height = 44;
            Item.value = 1000 * 5;
        }
	}
}