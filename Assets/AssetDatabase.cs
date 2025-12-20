using System;
using System.Runtime.InteropServices;
using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace AftermathMod.Assets
{
    public class AssetDatabase : ModSystem
    {
        //TEXTURES
        public static Asset<Texture2D> EmptySprite {  get; private set; }

        public static Asset<Texture2D> Line { get; private set; }

        public static Asset<Texture2D> GlowLine { get; private set; }

        public static Asset<Texture2D> GlowLineWithEnd { get; private set; }

        public static Asset<Texture2D> RingTelegraph1 { get; private set; }

        public static Asset<Texture2D> LaserBullet { get; private set; }

        public static Asset<Texture2D> LaserBulletBig { get; private set; }

        public static Asset<Texture2D> PerlinNoise { get; private set; }

        public static Asset<Texture2D> Trail1Small { get; private set; }

        public static Asset<Texture2D> Trail1Big { get; private set; }

        public static Asset<Texture2D> CharTeethTop { get; private set; }

        public static Asset<Texture2D> CharTeethBottom { get; private set; }

        public static Asset<Texture2D> VanillaFlamethrower { get; private set; }

        public static Asset<Texture2D> TesseractRay { get; private set; }

        //SHADERS
        public static Asset<Effect> BoxBlur { get; private set; }

        public static Asset<Effect> ScreenTint { get; private set; }

        public static Asset<Effect> LaserShader { get; private set; }

        public override void Load()
        {
            //TEXTURES
            EmptySprite = ModContent.Request<Texture2D>("AftermathMod/Assets/EmptySprite", AssetRequestMode.ImmediateLoad);

            Line = ModContent.Request<Texture2D>("AftermathMod/Assets/Line", AssetRequestMode.ImmediateLoad);

            GlowLine = ModContent.Request<Texture2D>("AftermathMod/Assets/GlowLine", AssetRequestMode.ImmediateLoad);

            GlowLineWithEnd = ModContent.Request<Texture2D>("AftermathMod/Assets/GlowLineTest", AssetRequestMode.AsyncLoad);

            RingTelegraph1 = ModContent.Request<Texture2D>("AftermathMod/Assets/RingTelegraph1", AssetRequestMode.ImmediateLoad);

            LaserBullet = ModContent.Request<Texture2D>("AftermathMod/Assets/AmbiguousLaserBullet", AssetRequestMode.ImmediateLoad);

            LaserBulletBig = ModContent.Request<Texture2D>("AftermathMod/Assets/AmbiguousLaserBulletBig", AssetRequestMode.ImmediateLoad);

            PerlinNoise = ModContent.Request<Texture2D>("AftermathMod/Assets/Perlin", AssetRequestMode.ImmediateLoad);

            Trail1Small = ModContent.Request<Texture2D>("AftermathMod/Assets/Trail1Small", AssetRequestMode.ImmediateLoad);

            Trail1Big = ModContent.Request<Texture2D>("AftermathMod/Assets/Trail1Big", AssetRequestMode.ImmediateLoad);

            CharTeethTop = ModContent.Request<Texture2D>("AftermathMod/Assets/CharTeethTop", AssetRequestMode.ImmediateLoad);

            CharTeethBottom = ModContent.Request<Texture2D>("AftermathMod/Assets/CharTeethBottom", AssetRequestMode.ImmediateLoad);

            VanillaFlamethrower = ModContent.Request<Texture2D>("AftermathMod/Assets/VanillaFlamethrower", AssetRequestMode.ImmediateLoad);

            TesseractRay = ModContent.Request<Texture2D>("AftermathMod/Assets/TesseractRay", AssetRequestMode.ImmediateLoad);

            //SHADERS
            if (Main.netMode != NetmodeID.Server)
            {
                BoxBlur = ModContent.Request<Effect>("AftermathMod/Assets/Shaders/BoxBlur", AssetRequestMode.ImmediateLoad);
                Filters.Scene["AftermathMod:BoxBlur"] = new Filter(new ScreenShaderData(BoxBlur, "BlurPass"), EffectPriority.Medium);

                ScreenTint = ModContent.Request<Effect>("AftermathMod/Assets/Shaders/ScreenTint", AssetRequestMode.ImmediateLoad);
                Filters.Scene["AftermathMod:ScreenTint"] = new Filter(new ScreenShaderData(ScreenTint, "ScreenTintPass"), EffectPriority.Medium);

                LaserShader = ModContent.Request<Effect>("AftermathMod/Assets/Shaders/LaserShader", AssetRequestMode.ImmediateLoad);
                Filters.Scene["AftermathMod:LaserShader"] = new Filter(new ScreenShaderData(ScreenTint, "LaserPass"), EffectPriority.Medium);
            }
        }

        public override void Unload()
        {
            //TEXTURES
            EmptySprite = null;

            Line = null;

            GlowLine = null;

            GlowLineWithEnd = null;

            RingTelegraph1 = null;

            LaserBullet = null;

            LaserBulletBig = null;

            PerlinNoise = null;

            Trail1Small = null;

            Trail1Big = null;

            CharTeethTop = null;

            CharTeethBottom = null;

            VanillaFlamethrower = null;

            TesseractRay = null;

            //SHADERS
            BoxBlur = null;

            ScreenTint = null;

            LaserShader = null;
        }
    }
}
