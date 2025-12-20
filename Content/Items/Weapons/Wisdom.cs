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
	public class Wisdom : ModItem
	{

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Wisdom_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.damage = 33;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 25;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 6;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FrostRingProjectile>();
			Item.shootSpeed = 18;
            Item.value = 80000 * 5;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 309, 0f, 0f, 0, new Color(255, 40, 40), Scale: 1.2f);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<WisdomProjectile>(), Item.damage, 1);
            Projectile.NewProjectile(source, position, velocity * 0.75f, ModContent.ProjectileType<WisdomProjectile>(), Item.damage, 1);
            Projectile.NewProjectile(source, position, velocity * 0.5f, ModContent.ProjectileType<WisdomProjectile>(), Item.damage, 1);
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