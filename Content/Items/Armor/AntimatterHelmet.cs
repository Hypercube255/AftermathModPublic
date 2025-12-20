using AftermathMod.Content.Items;
using AftermathMod.Content.StatusFX.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Head)]
	public class AntimatterHelmet : ModItem
	{
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Armor/AntimatterHelmet_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = 114000 * 5;
			Item.rare = ItemRarityID.Red;
			Item.defense = 22;
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 0.19f;
			player.manaCost -= 0.08f;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			return body.type == ModContent.ItemType<AntimatterShell>() && legs.type == ModContent.ItemType<AntimatterLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
			player.AddBuff(ModContent.BuffType<HitmanBuff>(), 2);
			player.setBonus = "1.1x Damage dealt to bosses and their servants";
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Hyperobsidian>());
            recipe.AddIngredient(ModContent.ItemType<AntimatterCore>());
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
//endgame bars in recipe