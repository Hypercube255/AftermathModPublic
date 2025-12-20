using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Items;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Body)]
	public class BronzeChainmail : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = 19800 * 5;
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.lifeRegen = 2;
		}
		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BronzeBar>(), 22);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}