using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class Voucher : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.Expert;
            Item.width = 58;
            Item.height = 46;
            Item.value = 50000 * 5;
			Item.expert = true;
        }
	}
}