using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace AftermathMod;

public class BossDownedSystem : ModSystem
{
    public static bool downedEverlastingFlame = false;
    public static bool downedEvilRings = false;
    public static bool downedSporeGuardian = false;
    public static bool downedStargazer = false;

    public static bool downedSeaDragon = false;

    public override void ClearWorld()
    {
        downedEverlastingFlame = false;
        downedEvilRings = false;
        downedSporeGuardian = false;
        downedStargazer = false;

        downedSeaDragon = false;
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag["downedEverlastingFlame"] = downedEverlastingFlame;
        tag["downedEvilRings"] = downedEvilRings;
        tag["downedSporeGuardian"] = downedSporeGuardian;
        tag["downedStargazer"] = downedStargazer;

        tag["downedSeaDragon"] = downedSeaDragon;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        downedEverlastingFlame = tag.GetBool("downedEverlastingFlame");
        downedEvilRings = tag.GetBool("downedEvilRings");
        downedSporeGuardian = tag.GetBool("downedSporeGuardian");
        downedStargazer = tag.GetBool("downedStargazer");

        downedSeaDragon = tag.GetBool("downedSeaDragon");
    }
}