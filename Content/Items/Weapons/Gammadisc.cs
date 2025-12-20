using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items.Weapons
{
	public class Gammadisc : ModItem
	{
        int framecounter;
        int framecounter2;

        int SoundTimer = 33;
        public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 2));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			Item.staff[Type] = true;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/Gammadisc_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 52 * framecounter2, texture.Width, texture.Height / 2), Color.White, rotation, new Vector2(texture.Width * 0.5f, (texture.Height - 4) / 4), 1f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
			Item.damage = 777;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 1;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 3;
			Item.rare = ItemRarityID.Red;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<HiddenGammadisc>();
			Item.autoReuse = true;
			Item.shootSpeed = 9;
			Item.noMelee = true;
            Item.value = 114000 * 5;
        }
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			Vector2 PositionReal = Vector2.Normalize(velocity) * 53f;
            position += PositionReal;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 PositionReal, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, PositionReal, velocity, ModContent.ProjectileType<HiddenGammadisc>(), Item.damage, 0);
            Projectile.NewProjectile(source, PositionReal -= Vector2.Normalize(velocity) * 26f, velocity, ModContent.ProjectileType<HiddenGammadisc>(), Item.damage, 0);
            Projectile.NewProjectile(source, PositionReal -= Vector2.Normalize(velocity) * 26f, velocity, ModContent.ProjectileType<HiddenGammadisc>(), Item.damage, 0);
            return false;
        }
        public override void PostUpdate()
        {
            framecounter++;
            if (framecounter == 4)
            {
                framecounter = 0;
                framecounter2++;
            }
            if (framecounter2 == 2)
            {
                framecounter2 = 0;
            }
        }
        public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Hyperobsidian>());
            recipe.AddIngredient(ModContent.ItemType<AntimatterCore>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool? UseItem(Player player)
        {
            SoundTimer++;

            if ((Main.mouseLeft && Main.mouseLeftRelease) || SoundTimer >= 33)
            {
                SoundEngine.PlaySound(SoundID.Item22);
                SoundTimer = 0;
            }

            return true;
        }
	}	
}