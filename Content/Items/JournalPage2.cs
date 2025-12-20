using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class JournalPage2 : ModItem
	{
        public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.rare = ItemRarityID.Orange;
            Item.width = 40;
            Item.height = 40;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!BossDownedSystem.downedEvilRings)
            {
                tooltips.Clear();
            }
        }
    }
}