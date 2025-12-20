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
	public class RedStreetlight : ModItem
	{
        SoundStyle HitSound = new SoundStyle("AftermathMod/Content/Sounds/SoundFX/RedStreetlightBONG");
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/Weapons/RedStreetlight_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + 100 * 0.5f, Item.position.Y - Main.screenPosition.Y + 72 * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
            Item.damage = 55;
            Item.width = 100;
            Item.height = 100;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 16f;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.value = 129300 * 5;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.GetModPlayer<AftermathPlayer>().MutedRedStreetlight == false)
            {
                SoundEngine.PlaySound(in HitSound, target.Center);
            }
        }
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (player.GetModPlayer<AftermathPlayer>().MutedRedStreetlight == false)
            {
                SoundEngine.PlaySound(in HitSound, target.Center);
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 SwingOffset = player.itemLocation.DirectionTo(player.Center) * 22;

            player.itemLocation += SwingOffset;

            Lighting.AddLight(player.itemLocation, new Vector3(3, 3, 3));
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<AftermathPlayer>().CooldownRedStreetlight = 24;

            return null;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<AftermathPlayer>().CooldownRedStreetlight <= 0;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            if (player.GetModPlayer<AftermathPlayer>().MutedRedStreetlight == false)
            {
                Main.NewText("Muted Red Streetlights");
                player.GetModPlayer<AftermathPlayer>().MutedRedStreetlight = true;
            }
            else
            {
                Main.NewText("Unmuted Red Streetlights");
                player.GetModPlayer<AftermathPlayer>().MutedRedStreetlight = false;
            }
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center + new Vector2(-8, -50), new Vector3(3, 3, 3));
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<LightPole>());
            recipe.AddIngredient(ItemID.SoulofFright, 15);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}	
}