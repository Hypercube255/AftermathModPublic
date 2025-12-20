using AftermathMod.Content.Dusts;
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
	public class Sporay : ModItem
	{
        public override void SetStaticDefaults()
        {
			Item.staff[Type] = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Sporay_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.damage = 26;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 15;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Magic;
			Item.knockBack = 0;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item109;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.shootSpeed = 10;
            Item.mana = 9;
			Item.shoot = ModContent.ProjectileType<SporaySpores>();
            Item.value = 82000 * 5;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.3f) * 1.25f, ModContent.ProjectileType<SporayChunks>(), (int)(damage * 1.2f), 1f);

            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			Vector2 Offset = Vector2.Normalize(velocity) * 47f;

			if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
			{
                position += Offset;
            }
        }

        public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ReinforcedMushroot>(), 8);
            recipe.AddIngredient(ItemID.GlowingMushroom, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}	
}
