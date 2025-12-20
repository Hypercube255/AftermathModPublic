using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class MetallicFragments : ModItem
	{
        int framecounter = 0;
        int framecounter2 = 0;

        public override void SetStaticDefaults()
        {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 3));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true; 
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/MetallicFragments_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 42 * framecounter2, texture.Width, texture.Height / 3), Color.White, rotation, new Vector2(texture.Width * 0.5f, (texture.Height-6) / 6), 1f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = Item.CommonMaxStack;
            Item.width = 40;
            Item.height = 40;
            Item.value = 2000 * 5;
        }

        public override void PostUpdate()
        {
            framecounter++;
            if(framecounter == 7)
            {
                framecounter = 0;
                framecounter2++;
            }
            if(framecounter2 == 3)
            {
                framecounter2 = 0;
            }
        }
	}	
}
