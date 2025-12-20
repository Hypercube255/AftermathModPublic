using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Items;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Head)]
	public class BronzeHelmet : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 22;
			Item.value = 12600 * 5;
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.1f;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			return body.type == ModContent.ItemType<BronzeChainmail>() && legs.type == ModContent.ItemType<BronzeGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
			player.maxMinions += 1;
            player.manaCost -= 0.1f;
			player.GetCritChance(DamageClass.Generic) += 8;
            player.setBonus = "Increases your max number of minions by 1\n" +
							  "8% increased critical strike chance\n" +
                              "10% reduced mana cost";
        }
        public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BronzeBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}