using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class TalismanofPassion : ModItem
	{
        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Green;
            Item.width = 34;
            Item.height = 36;
            Item.value = 25000 * 5;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.12f; //12%
            player.jumpSpeedBoost += 0.5f; //10%

            player.GetModPlayer<AftermathPlayer>().HasTalismanofPassion = true;
            // also increases acceleration by 50%
        }
	}
}