using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using AftermathMod.Content.Projectiles;

namespace AftermathMod.Content.Items.Weapons
{
	public class Charybdis : ModItem
	{
        Vector2 DustVelocity;

        int VortexSoundTimer = 0;

        int UseTimeTrue = 0;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

		public override void SetDefaults()
		{
			Item.damage = 75;
			Item.width = 84;
			Item.height = 84;
			Item.useTime = 30;
			Item.useAnimation = Item.useTime;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 5.5f;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = false;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 15;
            Item.value = 182500 * 5;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2) // RIGHT CLICK
			{
                VortexSoundTimer++;

                Item.useTime = 1;
                Item.useAnimation = Item.useTime;
                Item.useStyle = ItemUseStyleID.RaiseLamp;
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.UseSound = null;

                if ((Main.mouseRight && Main.mouseRightRelease) || VortexSoundTimer >= 17)
                {
                    SoundEngine.PlaySound(SoundID.Item66, player.Center);
                    VortexSoundTimer = 0;
                }

                if (player.ownedProjectileCounts[ModContent.ProjectileType<CharybdisVortex>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<CharybdisVortex>(), Item.damage, 1, Main.myPlayer);
                }

                return true;
			}

            else if (!Main.mouseRight)
            {
                SoundEngine.PlaySound(SoundID.Item21, player.Center);// LEFT CLICK

                Vector2 Dir = player.Center.DirectionTo(Main.MouseWorld);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center + Dir * Item.shootSpeed * 2, Dir * Item.shootSpeed, ModContent.ProjectileType<CharybdisWave>(), Item.damage, Item.knockBack, Main.myPlayer);

                Item.useStyle = ItemUseStyleID.Swing;
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.useTime = UseTimeTrue;
                Item.useAnimation = UseTimeTrue;
                Item.UseSound = SoundID.Item1;

                for (int i = 0; i < 30; i++)
                {
                    DustVelocity = Main.rand.NextVector2Unit() * 8;
                    Dust dust = Dust.NewDustPerfect(player.Center, DustID.BlueCrystalShard, DustVelocity, Scale: 1.5f);
                    dust.noGravity = true;
                }

                return true;
            }

            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center + new Vector2(Main.rand.NextFloat(-40, 40), Main.rand.NextFloat(-10, 10)), Vector2.Zero, ModContent.ProjectileType<CharybdisTeeth>(), Item.damage, 2, ai1:Item.damage);
        }

        public override void HoldItem(Player player)
        {
            if (Collision.DrownCollision(player.position, player.width, player.height))
            {
                player.statDefense += 15;
                UseTimeTrue = 24;
            }
            else
            {
                UseTimeTrue = 30;
            }

            if (player.GetModPlayer<AftermathPlayer>().CharybdisVortexUp == true)
            {
                player.lifeRegen += 10;
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 SwingOffset = player.itemLocation.DirectionTo(player.Center) * 10;

            player.itemLocation += SwingOffset;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<TidalWrath>());//12000
            recipe.AddIngredient(ModContent.ItemType<FrostRing>());//60500
            recipe.AddIngredient(ItemID.Ectoplasm, 6);//30000
            recipe.AddIngredient(ItemID.SoulofFright, 10);//80000
            recipe.AddIngredient(ItemID.Obsidian, 20);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}	
}//vortex + buff (slower but more defense), Freezing wave projectile, sword biting