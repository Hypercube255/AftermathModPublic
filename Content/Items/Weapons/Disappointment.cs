using System.Reflection;
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
	public class Disappointment : ModItem
	{
        public override void SetStaticDefaults()
        {
			Item.staff[Type] = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			Vector2 ShootOffset = Vector2.Normalize(velocity) * 120;

			if (Collision.CanHit(position, 0, 0, position + ShootOffset, 0, 0))
			{
				position += ShootOffset;
			}
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Disappointment_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.damage = 2000;
			Item.width = 100;
			Item.height = 100;
			Item.useTime = 75;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Magic;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<DisappointmentProjectile>();
			Item.shootSpeed = 9;
			Item.noMelee = true;
			Item.mana = 25;
            Item.value = 114000 * 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<Hyperobsidian>();
            recipe.AddIngredient<AntimatterCore>();
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }	
}