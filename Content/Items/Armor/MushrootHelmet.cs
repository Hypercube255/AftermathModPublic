using AftermathMod.Content.Items;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AftermathMod.Content.Items.Armor
{ 
	[AutoloadEquip(EquipType.Head)]
	public class MushrootHelmet : ModItem
	{
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Armor/MushrootHelmet_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 20;
			Item.value = Item.buyPrice(silver: 80) * 5;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 6;
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.15f;
			player.statManaMax2 += 90;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			return body.type == ModContent.ItemType<MushrootBreastplate>() && legs.type == ModContent.ItemType<MushrootBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
			player.manaRegenBonus += 5;
            player.setBonus = "Increased mana regeneration";
        }
        public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ReinforcedMushroot>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}