using AftermathMod.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AftermathMod
{
    public class WingDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Wings);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.GetModPlayer<AftermathPlayer>().HasHyphiberWings;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            SpriteEffects spriteEffects;
            float Visible;

            Texture2D HyphiberWingsTexture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/HyphiberWings_WingsReal").Value;

            var Position = drawInfo.Center - Main.screenPosition;
            Position = new Vector2((int)Position.X, (int)Position.Y);

            Vector2 PositionOffset = Vector2.Zero;

            Rectangle Source = new Rectangle(0, 62 * drawInfo.drawPlayer.GetModPlayer<DrawingPlayer>().WingFrame, HyphiberWingsTexture.Width, HyphiberWingsTexture.Height / 5);
            Vector2 Origin = new Vector2(HyphiberWingsTexture.Width * 0.5f, (HyphiberWingsTexture.Height * 0.5f) / 5);

            if (drawInfo.drawPlayer.direction == - 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
                PositionOffset.X = 9;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
                PositionOffset.X = -9;
            }

            if (drawInfo.drawPlayer.GetModPlayer<DrawingPlayer>().DrawWings || drawInfo.drawPlayer.velocity.Y != 0)
            {
                Visible = 1f;
            }
            else
            {
                Visible = 0f;
            }

            drawInfo.DrawDataCache.Add(new DrawData(HyphiberWingsTexture, Position + PositionOffset, Source, Color.White * Visible, 0f, Origin, 1f, spriteEffects));
        }
    }
}
