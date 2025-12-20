using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AftermathMod.Content.StatusFX.Debuffs
{
    public class PainfulEnlightenment : ModBuff
    {
        int timer = 0;
        public override LocalizedText DisplayName => base.DisplayName.WithFormatArgs("Painful Enlightenment");
        public override LocalizedText Description => base.Description.WithFormatArgs("The dazzling radiance is overwhelming you");
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            timer++;

            player.GetModPlayer<AftermathPlayer>().PainfulEnlightenment = true;

            if (timer % 15 == 0)
            {
                player.statLife -= 3;
                CombatText.NewText(player.Hitbox, CombatText.DamagedFriendly, 3);
            }

            Dust.NewDust(player.position, player.width, player.height, DustID.BlueTorch, Scale: 1f);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            timer++;

            if (timer % 5 == 0)
            {
                npc.life -= 3;
                CombatText.NewText(npc.Hitbox, CombatText.DamagedFriendly, 3);
            }

            Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueTorch, Scale: 1f);
        }
    }
}