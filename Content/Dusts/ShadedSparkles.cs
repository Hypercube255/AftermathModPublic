using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;

namespace AftermathMod.Content.Dusts
{
    public class ShadedSparkles : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            dust.noGravity = true;
            dust.noLight = false;

            dust.frame.Width = 10;
            dust.frame.Height = 10;
        }
       

        public override bool Update(Dust dust)
        {
            dust.scale *= 0.99f;

            return true;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}