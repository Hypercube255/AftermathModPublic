using System.Diagnostics.Metrics;
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
	public class Subversion : ModItem
	{
	int counter = 0;
    int counter2 = 0;
        public override void SetStaticDefaults()
		{
			Item.staff[Type] = true;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Subversion_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
			Item.damage = 123;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 6;
			Item.useAnimation = 18;
			Item.DamageType = DamageClass.Magic;
			Item.knockBack = 2;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SubversionProjectile>();
			Item.shootSpeed = 25;
			Item.noMelee = true;
			Item.mana = 6;
            Item.value = 114000 * 5;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
		counter++;
		counter2++;
		if (counter==4)
		{
			float projectileCount = 4; //projectile count
			float projRot = 100/(projectileCount-1); //sets angle, divides into segments

			for (int x = 0; x<projectileCount; x++)
			{
				Vector2 projectileDirection = velocity.RotatedBy(MathHelper.ToRadians(x*projRot-50));
				Projectile.NewProjectile(source, position, projectileDirection, ModContent.ProjectileType<SubversionProjectileHoming>(), damage, knockback, player.whoAmI);
                }
                counter = 0;
		}
        if (counter2 >= 15)
        {
            Projectile.NewProjectile(source, position, velocity * 0.4f, ModContent.ProjectileType<BeegVortex>(), damage, knockback, player.whoAmI);
				counter2 = 0;
        }
            return true;
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
//Custom projectile, big vortex on timer