using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace AftermathMod.Content.Items
{

    [AutoloadEquip(EquipType.Wings)]
    public class HyphiberWings : ModItem
    {
        int FrameCounter;
        int lol = 0;

        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(70, 6f, 1f);
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.width = 32;
            Item.height = 24;
            Item.value = 20000 * 5;
            Item.accessory = true;
            Item.expert = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<AftermathPlayer>().HasHyphiberWings = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DrawingPlayer>().DrawWings = !hideVisual;
        }

        public override bool WingUpdate(Player player, bool inUse)
        {
            if (player.controlJump && player.velocity.Y != 0f)
            {
                if (player.wingTime > 0)//animate when flying
                {
                    FrameCounter++;

                    if (FrameCounter >= 6)
                    {
                        FrameCounter = 0;
                        lol++;

                        player.GetModPlayer<DrawingPlayer>().WingFrame = lol % 5;
                    }
                }
                else//gliding frame
                {
                    player.GetModPlayer<DrawingPlayer>().WingFrame = 2;
                }
            }
            else if (player.velocity.Y != 0f)//falling frame
            {
                player.GetModPlayer<DrawingPlayer>().WingFrame = 1;
            }
            else//standing frame
            {
                player.GetModPlayer<DrawingPlayer>().WingFrame = 0;
                lol = 0;
            }

            return true;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 6f;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.5f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 0.5f;
            maxAscentMultiplier = 1.5f;
            constantAscend = 0.1f;
        }//I have no idea what ANY of this means like wtf is this, WHAT DOES "maxCanAscendMultiplier" MEAN? And the parameter descriptions DO NOT make this easier AT ALL
    }
}