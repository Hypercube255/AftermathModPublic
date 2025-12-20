using System;
using AftermathMod.Content.NPCs;
using AftermathMod.Content.StatusFX.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace AftermathMod.Content
{
    public class AftermathPlayer : ModPlayer
    {
        public SoundStyle SecondChanceDodge = new SoundStyle("AftermathMod/Content/Sounds/SoundFX/SecondChanceDodge");

        //STATUS EFFECTS
        public bool RingBraceletBuff = false;

        public bool HitmanBuff = false;

        public bool PainfulEnlightenment = false;

        //EQUIPMENT
        public bool HasTalismanofPassion = false;

        public bool HasRelicofTruth = false;

        public bool HasSecondChance = false;

        public bool HasHyphiberWings = false;

        public bool ShadedSetBonus = false;

        public float ShadowRuneMultiplier = 1f; //1 to 3

        public float TenebrisSpeedBonus = 1f; //1 to 1.5

        public float TenebrisDustSize = 1;

        //public bool HasDraconicAquashield = false;    <--- this one is in DashPlayerDA

        //ITEMS
        public bool MutedLightPole = false;

        public int CooldownLightPole = 0;

        public bool MutedRedStreetlight = false;

        public int CooldownRedStreetlight = 0;

        public bool CharybdisVortexUp = false;

        public override void ResetEffects()
        {
            RingBraceletBuff = false;

            HitmanBuff = false;

            PainfulEnlightenment = false;

            HasTalismanofPassion = false;

            HasRelicofTruth = false;

            HasSecondChance = false;

            HasHyphiberWings = false;

            ShadedSetBonus = false;

            //HasDraconicAquashield = false;    <--- this one is in DashPlayerDA

            CharybdisVortexUp = false;
        }

        public override void PostUpdateRunSpeeds()
        {
            if (HasTalismanofPassion || HasRelicofTruth)
            {
                Player.runAcceleration *= 1.4f;
            }
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (HasSecondChance && Main.rand.NextBool(15, 100))
            {
                SoundEngine.PlaySound(SecondChanceDodge, Player.Center);

                int SegmentSize = 50;
                int HalfSegment = SegmentSize / 2;

                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 180 : 120);
                /*
                for (int i = 0; i < 160; i++)
                {
                    Dust dust = Dust.NewDustDirect(Player.Center + new Vector2(-HalfSegment, -3f * SegmentSize), SegmentSize, 6 * SegmentSize, DustID.Firework_Yellow, 0, 1, Scale: 2);
                }
                for (int i = 0; i < 32; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Player.Center + new Vector2(-2f * SegmentSize, -1.5f * SegmentSize), HalfSegment + SegmentSize, SegmentSize, DustID.Firework_Yellow, 0, 1, Scale: 2);
                }
                for (int i = 0; i < 32; i++)
                {
                    Dust dust3 = Dust.NewDustDirect(Player.Center + new Vector2(HalfSegment, -1.5f * SegmentSize), HalfSegment + SegmentSize, SegmentSize, DustID.Firework_Yellow, 0, 1, Scale: 2);
                }
                */

                for (int i = 0; i < 160; i++)
                {
                    Dust dust = Dust.NewDustDirect(Player.Center + new Vector2(-HalfSegment, -2.5f * SegmentSize), SegmentSize, 5 * SegmentSize, DustID.Firework_Yellow, 0, 1, Scale: 2);
                }
                for (int i = 0; i < 64; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Player.Center + new Vector2(-2.5f * SegmentSize, -0.5f * SegmentSize), 2 * SegmentSize, SegmentSize, DustID.Firework_Yellow, 0, 1, Scale: 2);
                }
                for (int i = 0; i < 64; i++)
                {
                    Dust dust3 = Dust.NewDustDirect(Player.Center + new Vector2(0.5f * SegmentSize, -0.5f * SegmentSize), 2 * SegmentSize, SegmentSize, DustID.Firework_Yellow, 0, 1, Scale: 2);
                }

                return true;
            }

            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (HitmanBuff)
            {
                if (StatusFX.Buffs.HitmanBuff.Bosses.Contains(target.type))
                {
                    modifiers.FinalDamage *= 1.1f;
                }
            }
        }

        public override void PreUpdate()
        {
            CooldownLightPole--;
            CooldownRedStreetlight--;

            if (ShadowRuneMultiplier > 1)
            {
                ShadowRuneMultiplier -= 0.01f;
            }

            if (TenebrisSpeedBonus > 1)
            {
                TenebrisSpeedBonus -= 0.005f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ShadowRuneMultiplier + 0.2f < 3)
            {
                ShadowRuneMultiplier += 0.2f;
            }
        }
    }
}

       


