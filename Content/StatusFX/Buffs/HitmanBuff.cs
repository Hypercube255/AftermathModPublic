using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AftermathMod.Content.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AftermathMod.Content.StatusFX.Buffs
{
    public class HitmanBuff : ModBuff
    {
        public static List<int> Bosses = new List<int>() 
        {
            NPCID.KingSlime,

            NPCID.EyeofCthulhu,
            NPCID.ServantofCthulhu,

            NPCID.EaterofWorldsHead,
            NPCID.EaterofWorldsBody,
            NPCID.EaterofWorldsTail,

            NPCID.BrainofCthulhu,
            NPCID.Creeper,

            NPCID.QueenBee,

            NPCID.Deerclops,

            NPCID.Skeleton,
            NPCID.SkeletronHand,

            NPCID.WallofFlesh,
            NPCID.WallofFleshEye,
            NPCID.TheHungry,
            NPCID.TheHungryII,
            NPCID.LeechHead,
            NPCID.LeechBody,
            NPCID.LeechTail,

            NPCID.QueenSlimeBoss,
            NPCID.QueenSlimeMinionBlue,
            NPCID.QueenSlimeMinionPink,
            NPCID.QueenSlimeMinionPurple,

            NPCID.Retinazer,
            NPCID.Spazmatism,

            NPCID.TheDestroyer,
            NPCID.TheDestroyerBody,
            NPCID.TheDestroyerTail,
            NPCID.Probe,

            NPCID.SkeletronPrime,
            NPCID.PrimeCannon,
            NPCID.PrimeSaw,
            NPCID.PrimeVice,
            NPCID.PrimeLaser,

            NPCID.Plantera,
            NPCID.PlanterasHook,
            NPCID.PlanterasTentacle,
            NPCID.Spore,

            NPCID.Golem,
            NPCID.GolemFistLeft,
            NPCID.GolemFistRight,
            NPCID.GolemHead,
            NPCID.GolemHeadFree,

            NPCID.HallowBoss,

            NPCID.DukeFishron,
            NPCID.Sharkron,
            NPCID.Sharkron2,

            NPCID.CultistBoss,
            NPCID.AncientCultistSquidhead,
            NPCID.AncientDoom,
            NPCID.AncientLight,
            NPCID.CultistDragonHead,
            NPCID.CultistDragonBody1,
            NPCID.CultistDragonBody2,
            NPCID.CultistDragonBody3,
            NPCID.CultistDragonBody4,
            NPCID.CultistDragonTail,

            NPCID.MoonLordCore,
            NPCID.MoonLordHand,
            NPCID.MoonLordHead,
            NPCID.MoonLordLeechBlob,

            /*-----*/

            ModContent.NPCType<EverlastingFlameI1S>(),

            ModContent.NPCType<EvilRings>(),
            ModContent.NPCType<SupportDrone>(),

            ModContent.NPCType<SporeGuardian>(),

            ModContent.NPCType<Stargazer>(),
        };

        public override LocalizedText DisplayName => base.DisplayName.WithFormatArgs("Hitman");
        public override LocalizedText Description => base.Description.WithFormatArgs("Damage to bosses and their servants is increased");
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AftermathPlayer>().HitmanBuff = true;
        } 
    }
}