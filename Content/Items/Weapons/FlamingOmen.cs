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
	public class FlamingOmen : ModItem
	{
        public override void SetStaticDefaults()
        {
			Item.staff[Type] = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/FlamingOmen_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.damage = 20;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 26;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item34;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.shootSpeed = 20;
			Item.useAmmo = AmmoID.Gel;
			Item.shoot = ModContent.ProjectileType<I1SFlamethrowerFriendly>();
            Item.value = 12000 * 5;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			Vector2 Offset = Vector2.Normalize(velocity) * 40f;

			if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
			{
                position += Offset;
            }
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BurningEmbers>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}	
}
