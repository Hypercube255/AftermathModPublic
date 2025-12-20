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
	public class Observer : ModItem
	{
        public override void SetStaticDefaults()
        {
			Item.staff[Type] = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Observer_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.damage = 44;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 33;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Magic;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ObserverProjectile>();
			Item.noMelee = true;
			Item.mana = 11;
            Item.value = 80000 * 5;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			velocity = new(10, 0);
			for(int i = 0; i<10; i++)
			{
                float WhereOnCircle = Main.rand.NextFloat(0,10);
                Vector2 SpawnPosition = Main.MouseWorld - new Vector2((float)System.Math.Cos(WhereOnCircle), (float)System.Math.Sin(WhereOnCircle)) * 200;
                Vector2 TowardsMouse = velocity.RotatedBy(SpawnPosition.AngleTo(Main.MouseWorld));

                Projectile.NewProjectile(source, SpawnPosition, TowardsMouse, ModContent.ProjectileType<ObserverProjectile>(), Item.damage, 0);

            }
            return false;
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<MetallicFragments>(4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }	
}