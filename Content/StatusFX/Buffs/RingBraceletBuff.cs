using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AftermathMod.Content.StatusFX.Buffs
{
    public class RingBraceletBuff : ModBuff
    {
        public override LocalizedText DisplayName => base.DisplayName.WithFormatArgs("Plasma Balls");
        public override LocalizedText Description => base.Description.WithFormatArgs("The plasma balls Will protect you");
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<AftermathPlayer>().RingBraceletBuff = true;
        }
    }
}