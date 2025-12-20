using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Items;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Body)]
	public class ShadedBreastplate : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.value = 196000 * 5;
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 10;
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
		}
		public override void UpdateEquip(Player player)
		{
            player.GetDamage(DamageClass.Generic) += 0.3f;
        }
		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadedBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}