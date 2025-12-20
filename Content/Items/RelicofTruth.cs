using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class RelicofTruth : ModItem
	{
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 12));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Pink;
            Item.width = 36;
            Item.height = 50;
            Item.value = 80000 * 5;
            Item.accessory = true;
            Item.defense = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //passion.
            player.moveSpeed += 0.12f; //12%
            player.jumpSpeedBoost += 0.5f; //10%

            player.GetModPlayer<AftermathPlayer>().HasRelicofTruth = true;

            //perseverance.
            player.statLifeMax2 = 11 * (player.statLifeMax2 / 10); //10% increased health
            //(and the defense)

            //precision.
            player.GetCritChance(DamageClass.Generic) += 6;
            player.GetArmorPenetration(DamageClass.Generic) += 3;
        }

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddTile(TileID.TinkerersWorkbench)
				.AddIngredient(ModContent.ItemType<TalismanofPassion>())
                .AddIngredient(ModContent.ItemType<TalismanofPerseverance>())
                .AddIngredient(ModContent.ItemType<TalismanofPrecision>())
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddIngredient(ItemID.SoulofMight, 3)
                .AddIngredient(ItemID.SoulofSight, 3)
                .Register();
		}
	}
}