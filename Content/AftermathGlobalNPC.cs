using System;
using AftermathMod.Content.Items;
using Microsoft.Build.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content
{
    public class AftermathGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if(npc.type == NPCID.Skeleton)
            {
                npcLoot.Add(ItemDropRule.OneFromOptions(Main.expertMode ? 15 : 20, ModContent.ItemType<TalismanofPassion>(), ModContent.ItemType<TalismanofPerseverance>(), ModContent.ItemType<TalismanofPrecision>()));
            }

            if (npc.type == NPCID.UndeadMiner || npc.type == NPCID.DoctorBones)
            {
                npcLoot.Add(ItemDropRule.OneFromOptions(Main.expertMode ? 8 : 10, ModContent.ItemType<TalismanofPassion>(), ModContent.ItemType<TalismanofPerseverance>(), ModContent.ItemType<TalismanofPrecision>()));
            }
        }

        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.SkeletonMerchant) // wanted to make it random but had to resort to the moons
            {
                shop.Add(ModContent.ItemType<TalismanofPassion>(), Condition.MoonPhaseThirdQuarter);

                shop.Add(ModContent.ItemType<TalismanofPerseverance>(), Condition.MoonPhaseWaxingCrescent);

                shop.Add(ModContent.ItemType<TalismanofPrecision>(), Condition.MoonPhaseWaxingGibbous);
            }
        }
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if (Main.rand.NextBool(5))
            {
                shop[nextSlot] = ModContent.ItemType<TalismanofPassion>();
                nextSlot++;
            }

            if (Main.rand.NextBool(5))
            {
                shop[nextSlot] = ModContent.ItemType<TalismanofPerseverance>();
                nextSlot++;
            }

            if (Main.rand.NextBool(5))
            {
                shop[nextSlot] = ModContent.ItemType<TalismanofPrecision>();
                nextSlot++;
            }
        }

        public override void OnKill(NPC npc)
        {
            if(npc.type == NPCID.Plantera && Condition.NotDownedPlantera.IsMet())
            {
                Main.NewText("The Spore Guardian grows stronger", new Microsoft.Xna.Framework.Color(150, 230, 255));
            }
        }
    }
}

       


