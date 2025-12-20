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
	public class LightPole : ModItem
	{
		SoundStyle HitSound = new SoundStyle("AftermathMod/Content/Sounds/SoundFX/LampPostBONG");
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/LightPole_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
			Item.damage = 16;
			Item.width = 88;
			Item.height = 88;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 12f;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = false;
            Item.value = 9300 * 5;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(player.GetModPlayer<AftermathPlayer>().MutedLightPole == false)
            {
                SoundEngine.PlaySound(in HitSound, target.Center);
            }
        }
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (player.GetModPlayer<AftermathPlayer>().MutedLightPole == false)
            {
                SoundEngine.PlaySound(in HitSound, target.Center);
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 SwingOffset = player.itemLocation.DirectionTo(player.Center) * 8;

            player.itemLocation += SwingOffset;

            Lighting.AddLight(player.itemLocation, new Vector3(2, 2, 2));
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<AftermathPlayer>().CooldownLightPole = 27;

            return null;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<AftermathPlayer>().CooldownLightPole <= 0;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center + new Vector2(28, -28), new Vector3(2, 2, 2));
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            if (player.GetModPlayer<AftermathPlayer>().MutedLightPole == false)
            {
                Main.NewText("Muted Light Poles");
                player.GetModPlayer<AftermathPlayer>().MutedLightPole = true;
            }
            else
            {
                Main.NewText("Unmuted Light Poles");
                player.GetModPlayer<AftermathPlayer>().MutedLightPole = false;
            }
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 20);
            recipe.AddRecipeGroup(RecipeSystem.SilverBar, 5);
            recipe.AddIngredient(ItemID.Glass, 8);
            recipe.AddIngredient(ItemID.Candle, 1);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}	
}