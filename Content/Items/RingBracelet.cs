using AftermathMod.Content.Projectiles;
using AftermathMod.Content.StatusFX.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.Items
{
	public class RingBracelet : ModItem
	{

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/Items/RingBracelet_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height * 0.5f),
            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
			Item.width = 40;
			Item.height = 24;
			Item.value = 20000 * 5;
			Item.accessory = true;
			Item.expert = true;
		}

        public override void UpdateEquip(Player player)
        {

			player.GetCritChance<GenericDamageClass>() += 8;
			//spinning balls
			if (player.ownedProjectileCounts[ModContent.ProjectileType<EvilRingsRingFriendly>()] < 1)
			{
                Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center - new Vector2(200, 5), new Vector2(0, Methods.GetCircleSpeed(200, -1.2f)), ModContent.ProjectileType<EvilRingsRingFriendly>(), 22, 1);
                Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center + new Vector2(200, 5), new Vector2(0, Methods.GetCircleSpeed(200, 1.2f)), ModContent.ProjectileType<EvilRingsRingFriendly>(), 22, 1);
            }

			player.AddBuff(ModContent.BuffType<RingBraceletBuff>(), 1);
        }
    }
}