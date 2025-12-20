using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;

namespace AftermathMod.Content.Dusts
{
    public class Spores : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            dust.noGravity = true;
            dust.noLight = false;
        }
        
        public override bool Update(Dust dust)
        {

            dust.scale *= 0.99f;
            Vector3 shine = new Vector3(76, 121, 255) * 0.003f;

            Lighting.AddLight(dust.position, shine);

            return true;
        }
    }
}