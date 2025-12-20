using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class PocketLightning : ModItem
	{

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/PocketLightning_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.damage = 10;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 4;
			Item.useAnimation = 20;
			Item.DamageType = DamageClass.Magic;
			Item.knockBack = 2;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item9;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PLProjectile>();
			Item.shootSpeed = 25;
			Item.noMelee = true;
			Item.mana = 2;
            Item.value = 4 * 5;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, Main.MouseWorld - new Vector2(Main.rand.NextFloat(-150, 150), 555), new Vector2(0, 20).RotatedByRandom(0.4f), Item.shoot, Item.damage, Item.knockBack);
			return false;
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddIngredient(ItemID.Cloud, 15);
            recipe.AddIngredient(ItemID.RainCloud, 10);
			recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }	
}