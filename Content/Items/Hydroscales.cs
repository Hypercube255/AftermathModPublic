using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class Hydroscales : ModItem
	{

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = Item.CommonMaxStack;
            Item.width = 40;
            Item.height = 40;
            Item.value = 1000 * 5;
        }
	}	
}
