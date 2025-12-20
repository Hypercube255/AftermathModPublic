using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AftermathMod.Content.Projectiles;

namespace AftermathMod.Content.Items
{
    [AutoloadEquip(EquipType.Shield)]
    public class DraconicAquashield : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightRed;
            Item.width = 54;
            Item.height = 48;
            Item.value = 34000 * 5;
            Item.accessory = true;
            Item.defense = 4;
            Item.damage = 60;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 6;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DashPlayerDA>().HasDraconicAquashield = true;
            player.GetModPlayer<DashPlayerDA>().ShieldItem = Item;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe()
                .AddIngredient(ModContent.ItemType<Hydroscales>(), 12)
                .AddIngredient(ModContent.ItemType<Hyperobsidian>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class DashPlayerDA : ModPlayer
    {
        public bool HasDraconicAquashield = false;

        public const int Left = 3;
        public const int Right = 2;

        public const int DashTime = 30;
        public const int DashCooldown = 55;

        public  int DashTimeCurrent = 0;
        public  int DashCooldownCurrent = 0;

        public int DashDirection = -1;

        public int DashVelocity = 12;

        public int Damage = 60;

        public Item ShieldItem;

        public override void ResetEffects()
        {
            HasDraconicAquashield = false;

            ShieldItem = null;

            if(Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[Left] < 15)
            {
                DashDirection = Left;
            }
            else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[Right] < 15)
            {
                DashDirection = Right;
            }
            else
            {
                DashDirection = -1;
            }
        }

        public override void PreUpdateMovement()
        {
            if (CanDash() && DashDirection != -1 && DashCooldownCurrent == 0)
            {
                if (DashDirection == Left && Player.velocity.X > -DashVelocity)
                {
                    Player.velocity = new(-DashVelocity, Player.velocity.Y);

                    DashTimeCurrent = DashTime;
                    DashCooldownCurrent = DashCooldown;

                    Player.GiveImmuneTimeForCollisionAttack(DashTime);
                }
                else if (DashDirection == Right && Player.velocity.X < DashVelocity)
                {
                    Player.velocity = new(DashVelocity, Player.velocity.Y);

                    DashTimeCurrent = DashTime;
                    DashCooldownCurrent = DashCooldown;

                    Player.GiveImmuneTimeForCollisionAttack(DashTime);
                }
            }

            if(DashTimeCurrent > 0)
            {
                --DashTimeCurrent;

                Player.eocDash = DashTimeCurrent;
                Player.armorEffectDrawShadowEOCShield = true;

                Player.CollideWithNPCs(new((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Damage, 2, 5, 0);

                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.BlueTorch, Scale: 3.5f);
                    dust.noGravity = true;
                }

                Dust dust2 = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Clentaminator_Cyan);
                dust2.noGravity = true;

                if(DashTimeCurrent % 4 == 0)
                {
                    Vector2 Velocity = new Vector2(0, -16).RotatedByRandom(-0.5f);
                    Projectile.NewProjectile(Player.GetSource_Accessory(ShieldItem), Player.Center, Velocity, ModContent.ProjectileType<DAWaterDroplet>(), Damage / 3, 1);
                }
            }

            if (DashCooldownCurrent > 0)
            {
                --DashCooldownCurrent;
            }
        }

        private bool CanDash() => HasDraconicAquashield && Player.dashType == DashID.None && !Player.setSolar && !Player.mount.Active;
    }
}