using System;
using System.Collections.Generic;
using System.Net.Http;
using AftermathMod.Content.Dusts;
using AftermathMod.Content.Items;
using AftermathMod.Content.Projectiles;
using Humanizer;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.RGB;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;


namespace AftermathMod.Content.NPCs;
public class SeaDragon : ModNPC
{
    int frameCounter2;

    bool FlipYourselfNOW = false;

    int FrameStep = 8;

    int ShootSpeed = 80;

    float FlameVelocity = Main.expertMode ? 14 : 18;

    Player Target = null;

    bool NormalAnim = true;

    Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/SeaDragon", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

    float LifeRatio = 1;

    float ExpertSpeedMultiplier = Main.expertMode ? 1.5f : 1;

    int StuckTimer = 0;

    int VortexTimer = 0;

    int VortexSummonTime = 150;

    Vector2 ProjPos = Vector2.One;

    int FireballDamage = Main.expertMode ? 90 : 70;
    int SmallFireballDamage = Main.expertMode ? 80 : 65;
    int VortexDamage = Main.expertMode ? 110 : 80;

    float AttackState
    {
        get => NPC.ai[0];
        set => NPC.ai[0] = value;
    }
    float AttackState2
    {
        get => NPC.ai[1];
        set => NPC.ai[1] = value;
    }
    float AttackTimer
    {
        get => NPC.ai[2];
        set => NPC.ai[2] = value;
    }
    float AttackTimer2
    {
        get => NPC.ai[3];
        set => NPC.ai[3] = value;
    }
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 8;
    }

    public override void SetDefaults()
    {
        NPC.width = 80;
        NPC.height = 115;
        NPC.lifeMax = 3500;
        NPC.defense = 8;
        NPC.value = 20000;
        NPC.HitSound = SoundID.NPCHit6;
        NPC.DeathSound = SoundID.NPCDeath5;
        NPC.aiStyle = -1;
        NPC.damage = 60;
        NPC.knockBackResist = 0.1f;
        NPC.npcSlots = 3f;
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f - 14), NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 16), new Vector2(1, 1), FlipYourselfNOW ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        
        return false;
    }

    public override void FindFrame(int frameHeight)
    {
        if (NormalAnim)
        {
            ++NPC.frameCounter;
            if (NPC.frameCounter >= FrameStep)
            {
                NPC.frame.Y += (frameHeight);
                NPC.frameCounter = 0;
                frameCounter2++;
                if (frameCounter2 >= 6)
                {
                    NPC.frame.Y = 0;
                    frameCounter2 = 0;
                }
            }
        }
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Hydroscales>(), 1, 8, 10));
    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)//Hp * 2; damage * 1.5; defense * 1.2
    {
        NPC.damage = (int)(NPC.damage * balance * 0.75f);
        NPC.defense = (int)(NPC.defense * balance * 1.2f);
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return SpawnCondition.Ocean.Chance * 0.075f;
    }
    public override void OnKill()
    {
        NPC.SetEventFlagCleared(ref BossDownedSystem.downedSeaDragon, -1);

        for (int i = 0; i < 50; i++)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BlueCrystalShard, Scale: 2f);
        }

        if (Main.netMode != NetmodeID.Server)
        {
            int Horn = Mod.Find<ModGore>("SeaDragon_Horn").Type;
            int Head = Mod.Find<ModGore>("SeaDragon_Head").Type;
            int Body = Mod.Find<ModGore>("SeaDragon_Body").Type;
            int Tail = Mod.Find<ModGore>("SeaDragon_Tail").Type;

            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Horn);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Head);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Body);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Tail);
        }
    }
    public override void AI()
    {
        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest();
        }

        Target = Main.player[NPC.target];

        ProjPos = NPC.Center + new Vector2(45 * NPC.direction, -20);

        LifeRatio = NPC.life / (float)NPC.lifeMax;

        float LifeRatioApplied = MathHelper.Lerp(3f, 1, LifeRatio);

        float MaxRunSpeed = 1 * LifeRatioApplied * ExpertSpeedMultiplier;

        if (Target.Center.X <= NPC.Center.X)
        {
            NPC.direction = -1;
        }
        else
        {
            NPC.direction = 1;
        }

        if (NPC.Bottom.Y > Target.Bottom.Y + 64 && RiseCollision() || StuckTimer > 30) //rise if colliding with tiles and player is higher
        {
            NPC.noTileCollide = true;
            NPC.velocity.Y = -4;
        }
        else if (NPC.Bottom.Y < Target.Bottom.Y - 64) //fall if player is a lot lower
        {
            NPC.noTileCollide = true;
        }
        else
        {
            NPC.noTileCollide = false;
        }

        VortexTimer++;

        if (AttackState == 0)
        {
            if (NPC.direction == -1)
            {
                FlipYourselfNOW = false;
            }
            else
            {
                FlipYourselfNOW = true;
            }

            if (NPC.velocity.Length() < 0.1f)
            {
                StuckTimer++;
            }
            else
            {
                StuckTimer = 0;
            }

            if (Vector2.Distance(NPC.Center, Target.Center) < 2000)
            {
                if (Math.Abs(NPC.velocity.X) <= MaxRunSpeed)
                {
                    NPC.velocity.X += (float)NPC.direction * 0.06f * MaxRunSpeed * ExpertSpeedMultiplier;
                }
                else
                {
                    NPC.velocity.X = (MaxRunSpeed * 0.85f) * NPC.direction;
                }
            }

            if (Vector2.Distance(NPC.Center, Target.Center) < 1000)
            {
                Flames();
            }

            if (VortexTimer >= 1080)
            {
                AttackState = 1;
                AttackTimer = 0;
                AttackTimer2 = 0;
                VortexTimer = 0;
            }
        }
        else if (AttackState == 1)
        {
            FireVortex();
        }
    }

    public void Flames()
    {
        AttackTimer2++;

        if (AttackTimer2 >= ShootSpeed)
        {
            NormalAnim = false;

            NPC.frame.Y = (texture.Height / 8) * 6;

            if (AttackTimer2 >= ShootSpeed + FrameStep)
            {
                NPC.frame.Y = (texture.Height / 8) * 7;

                Vector2 vel = ProjPos.DirectionTo(Target.Center) * FlameVelocity;

                if(AttackTimer2 == ShootSpeed + FrameStep)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjPos, vel, ModContent.ProjectileType<SGBigFireball>(), Methods.UnvanillaDamage(FireballDamage), 3);

                    if (Main.expertMode)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjPos, vel.RotatedBy(MathHelper.ToRadians(25)) * 0.8f, ModContent.ProjectileType<SGSmallFireball>(), Methods.UnvanillaDamage(SmallFireballDamage), 2);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjPos, vel.RotatedBy(MathHelper.ToRadians(-25)) * 0.8f, ModContent.ProjectileType<SGSmallFireball>(), Methods.UnvanillaDamage(SmallFireballDamage), 2);
                    }
                }
            }

            if (AttackTimer2 >= ShootSpeed + FrameStep * 2)
            {
                NPC.frame.Y = 0;
                AttackTimer2 = 0;
                NormalAnim = true;
            }
        }
    }

    public void FireVortex()
    {
        AttackTimer++;

        if (AttackState2 == 0)
        {
            NPC.velocity.X *= 0.95f;

            if (NPC.velocity.X < 0.1f)
            {
                NPC.velocity.X = 0;
                AttackTimer = 0;
                AttackState2 = 1;
            }
        }
        else if (AttackState2 == 1)
        {
            if (NPC.velocity.Y == 0 || AttackTimer > 300)
            {
                AttackState2 = 2;
                AttackTimer = 0;
            }
        }
        else if (AttackState2 == 2)
        {
            NormalAnim = false;

            if (AttackTimer < 10)
            {
                NPC.frame.Y = (texture.Height / 8) * 6;
            }
            else if (AttackTimer == 10)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), ProjPos, Vector2.UnitY * 3, ModContent.ProjectileType<SGFireVortex>(), Methods.UnvanillaDamage(VortexDamage), -2, ai1: VortexSummonTime, ai2: Target.whoAmI);
            }
            else if (AttackTimer < 10 + VortexSummonTime)
            {
                NPC.frame.Y = (texture.Height / 8) * 7;

                if (AttackTimer % 5 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item20, ProjPos);
                }
            }
            else
            {
                AttackState = 0;
                AttackState2 = 0;
                AttackTimer = 0;
                AttackTimer2 = 0;

                NPC.frame.Y = 0;
                NormalAnim = true;
            }
        }
    }

    public bool RiseCollision()
    {

        int MinTileX = (int)(NPC.Left.X / 16) - 1;
        int MaxTileX = (int)(NPC.Right.X / 16) + 1;

        int MinTileY = (int)(NPC.Top.Y / 16) - 1;
        int MaxTileY = (int)(NPC.Bottom.Y / 16) + 1;

        if (MinTileX < 0)
        {
            MinTileX = 0;
        }
        if (MinTileY < 0)
        {
            MinTileY = 0;
        }
        if (MaxTileX > Main.maxTilesX)
        {
            MinTileX = Main.maxTilesX;
        }
        if (MaxTileY > Main.maxTilesY)
        {
            MinTileY = Main.maxTilesY;
        }

        for (int i = MinTileX; i < MaxTileX; ++i)
        {
            for (int j = MinTileY; j < MaxTileY; ++j)
            {
                Tile tile = Main.tile[i, j];

                if (tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] || tile.LiquidAmount > 128)
                {
                    Vector2 tileInWorld = new Point16(i, j).ToWorldCoordinates(0, 0);

                    if (NPC.Left.X < tileInWorld.X + 24 && NPC.Right.X > tileInWorld.X - 8 && NPC.Top.Y < tileInWorld.Y + 16 && NPC.Bottom.Y > tileInWorld.Y)
                    {
                        return true;
                    }
                } 
            }
        }

        return false;
    }
}