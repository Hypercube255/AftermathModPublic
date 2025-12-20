using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Items;
using Microsoft.Xna.Framework;
using AftermathMod.Content.NPCs;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Head)]
	public class ShadedHood : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = 96000 * 5;
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 2;
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
		}
		public override void UpdateEquip(Player player)
		{
            player.GetCritChance(DamageClass.Generic) += 30;
			player.maxMinions += 2;

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                if (Main.rand.NextFloat() > 1 - 0.001 * player.GetModPlayer<AftermathPlayer>().ShadowRuneMultiplier)
                {
                    Vector2 position = (Vector2.UnitY * Main.rand.Next(1000)).RotatedByRandom(MathHelper.TwoPi);
                    NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)(player.position.X + position.X), (int)(player.position.Y + position.Y), ModContent.NPCType<ShadowRune>());
                }
            }
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			return body.type == ModContent.ItemType<ShadedBreastplate>() && legs.type == ModContent.ItemType<ShadedBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Spawns shadow runes around you; they appear more often when you attack enemies\n" +
                              "Destroying the runes heals the closest player";
        }
        public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadedBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}