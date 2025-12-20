using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;

namespace AftermathMod.Content.Items.Weapons
{
	public class Annihilation : ModItem
	{
	int counter;

		public override void SetDefaults()
		{
			Item.damage = 143;
			Item.width = 100;
			Item.height = 100;
			Item.useTime = 12;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 8;
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType <AnnihilationProjectileBig> ();
			Item.shootSpeed = 70;
            Item.value = 114000 * 5;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Annihilation_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
			new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(2))
				{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 309, 0f, 0f, 0, new Color(255,0,0), Scale: 1.2f);
				}
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			counter++;
			float projectileCount = 1 + Main.rand.Next(0); //projectile count
			float projRot = 50/(projectileCount-0); //sets angle, divides into segments

			if (counter>=3)
			{
				for (int x = 0; x<projectileCount; x++)
				{
					Vector2 projectileDirection = velocity.RotatedBy(MathHelper.ToRadians(0));
					Projectile.NewProjectile(source, position, projectileDirection, type, damage, knockback, player.whoAmI);
					counter = 0;
				}
			}
			return false;
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