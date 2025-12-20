using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class BurningEmbers : ModItem
	{
        int framecounter = 0;
        int framecounter2 = 0;

        public override void SetStaticDefaults()
        {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 12));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true; 
        }

        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = Item.CommonMaxStack;
            Item.width = 32;
            Item.height = 40;
            Item.value = 1200 * 5;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void PostUpdate()
        {
            framecounter++;
            if(framecounter == 6)
            {
                framecounter = 0;
                framecounter2++;
            }
            if(framecounter2 == 12)
            {
                framecounter2 = 0;
            }
        }
	}	
}
