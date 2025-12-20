using System;
using System.Runtime.InteropServices;
using AftermathMod.Assets;
using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Animations;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod
{
    public partial class Methods
    {
        public enum LaserTypes
        {
            StargazerLaser
        }
        /// <summary>
        /// index: 0 = Basic line, 1 = Line with glow, 2 = Line with glow that isn't cut off at it's ends
        /// color: Don't forget about "with { A = 0 }"
        /// </summary>
        /// <param name="index"></param>
        public static void DrawTelegraph(int index, Vector2 startPosition, Vector2 endPosition, Color color, float width)
        {
            Texture2D texture = AssetDatabase.EmptySprite.Value;

            float OriginX = 0;

            switch (index)
            {
                case 0:
                    texture = AssetDatabase.Line.Value;
                    break;

                case 1:
                    texture = AssetDatabase.GlowLine.Value;
                    break;

                case 2:
                    texture = AssetDatabase.GlowLineWithEnd.Value;
                    OriginX = 112;
                    break;

                default:
                    texture = AssetDatabase.EmptySprite.Value;
                    break;
            }

            Vector2 Origin = new Vector2(OriginX, texture.Height * 0.5f);

            Vector2 Scale = Vector2.One;

            float rotation = startPosition.AngleTo(endPosition);

            Scale.X = startPosition.Distance(endPosition) / texture.Width;

            Scale.Y = width;

            //Origin.X *= Scale.X;

            Main.EntitySpriteDraw(texture, startPosition - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), color, rotation, Origin, Scale, SpriteEffects.None);
        }

        public static void DrawLaser(int index, Vector2 startPosition, Vector2 endPosition, float Width)
        {
            Texture2D texture = AssetDatabase.EmptySprite.Value;
            int SegmentX = 0;
            int SegmentY = 0;

            switch (index)
            {
                case 0:
                    texture = ModContent.Request<Texture2D>("AftermathMod/Content/Projectiles/StargazerBeam", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                    SegmentY = 30;

                    break;

                default:
                    texture = AssetDatabase.EmptySprite.Value;
                    break;
            }

            SegmentX = texture.Width;

            Vector2 Origin = new Vector2(SegmentX * 0.5f, SegmentY * 0.5f);

            int Head = (SegmentY + 2) * 2;
            int Body = SegmentY + 2;
            int Tail = 0;

            int SegmentAmount = (int)(Vector2.Distance(startPosition, endPosition) / SegmentY);
            Vector2 Step = startPosition.DirectionTo(endPosition) * SegmentY;
            float Rotation = startPosition.AngleTo(endPosition) + MathHelper.PiOver2;

            for(int i = 0; i < SegmentAmount; i++)
            {
                if(i == 0)
                {
                    Main.EntitySpriteDraw(texture, startPosition - Main.screenPosition + Step * i, new Rectangle(0, Head, SegmentX, SegmentY), Color.White, Rotation, Origin, new Vector2(Width, 1), SpriteEffects.None);
                }
                else if(i < SegmentAmount - 1)
                {
                    Main.EntitySpriteDraw(texture, startPosition - Main.screenPosition + Step * i, new Rectangle(0, Body, SegmentX, SegmentY), Color.White, Rotation, Origin, new Vector2(Width, 1), SpriteEffects.None);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, startPosition - Main.screenPosition + Step * i, new Rectangle(0, Tail, SegmentX, SegmentY), Color.White, Rotation, Origin, new Vector2(Width, 1), SpriteEffects.None);
                }
            }
        }

        public static bool OnScreen(Vector2 point)
        {
            Rectangle Screen = new((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

            if (Screen.Contains((int)point.X, (int)point.Y))
            {
                return true;
            }

            return false;
        }

        public static void AABBvLineGuide(Vector2 startRed, Vector2 endBlue, float GuideSize = 1)
        {
            Texture2D texture1 = AssetDatabase.PerlinNoise.Value;
            Main.EntitySpriteDraw(texture1, startRed - Main.screenPosition, null, Color.Red, 0, texture1.Size()* 0.5f, GuideSize / 100, SpriteEffects.None);
            Main.EntitySpriteDraw(texture1, endBlue - Main.screenPosition, null, Color.Blue, 0, texture1.Size()* 0.5f, GuideSize / 100, SpriteEffects.None);
        }
    }
}
