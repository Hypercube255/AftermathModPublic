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
	public class Mycolt : ModItem
	{
        Vector2 HoldOffset;

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Mycolt_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.damage = 30;
			Item.width = 52;
			Item.height = 34;
			Item.useTime = 25;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 2.5f;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item11;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.shootSpeed = 30;
			Item.useAmmo = AmmoID.Bullet;
			Item.shoot = ProjectileID.PurificationPowder;
            Item.value = 91500 * 5;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 Offset = Vector2.Normalize(velocity) * 40f;
            HoldOffset = Vector2.Normalize(velocity) * 12f;

            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }

            if (type == ProjectileID.Bullet)
			{
                type = ModContent.ProjectileType<GunSporeBullet>();
            }
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation -= HoldOffset;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ReinforcedMushroot>(), 9);
            recipe.AddIngredient(ItemID.GlowingMushroom, 15);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}	
}
