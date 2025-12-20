using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.UI;

namespace AftermathMod.Content.Items
{
	public class JournalPage1 : ModItem
	{
        public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.rare = ItemRarityID.Green;
            Item.width = 40;
            Item.height = 40;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!BossDownedSystem.downedEverlastingFlame)
            {
                tooltips.Clear();
            }
        }
	}
}