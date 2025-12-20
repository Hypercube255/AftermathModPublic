using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
using Terraria.UI;


namespace AftermathMod.Content.NPCs;

[AutoloadBossHead]

public class SporeGuardian : ModNPC
{
    List<int> PossibleSpawns = new List<int> { NPCID.SporeBat, NPCID.SporeSkeleton };

    int frameCounter2;
    public Vector2 LineToPlayer;
    Vector2 ShootOffset;
    int TimesShot;
    List<int> WhichSideList = new List<int> { 1, -1 };
    int WhichSide = 1;
    Vector2 TargetPos;
    int OriginalDamage;

    float LifeRatio = 1;
    float LowerHPSpeed;
    float LowerHPSpeed2;
    float TicksPerSpawn;

    float DiffAdjust = 0.5f; //the game keeps multiplying my damage numbers in certain spots - this should make up for it

    Player Target;
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
        Main.npcFrameCount[Type] = 5;
    }

    public override void SetDefaults()
    {
        NPC.width = 125;
        NPC.height = 125;
        NPC.lifeMax = 7000;
        NPC.defense = 9;
        NPC.value = 70000;
        NPC.HitSound = SoundID.NPCHit7;
        NPC.DeathSound = SoundID.NPCDeath43;
        NPC.aiStyle = -1;
        NPC.damage = 30;
        OriginalDamage = NPC.damage;
        NPC.noGravity = true;
        NPC.boss = true;
        NPC.noTileCollide = true;
        NPC.knockBackResist = 0f;
        NPC.npcSlots = 5f;
        Music = MusicID.Boss3;

        if(Condition.DownedPlantera.IsMet())
        {
            NPC.lifeMax = 33000;
            NPC.damage = 60;
            OriginalDamage = 60;

            PossibleSpawns.Add(NPCID.MushiLadybug);
            PossibleSpawns.Add(NPCID.AnomuraFungus);
        }

        if (Main.zenithWorld)
        {
            PossibleSpawns.Add(NPCID.EyeofCthulhu);
        }
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/SporeGuardian", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f), NPC.frame, drawColor, NPC.rotation, new Vector2(texture.Width / 2, ((texture.Height - 10) / 10) - 30), new Vector2(1, 1), SpriteEffects.None, 0f);
        
        return false;
    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D textureGlow = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/SporeGuardian_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        spriteBatch.Draw(textureGlow, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f), NPC.frame, Color.White, NPC.rotation, new Vector2(textureGlow.Width / 2, ((textureGlow.Height - 10) / 10) - 30), new Vector2(1, 1), SpriteEffects.None, 0f);
    }
    public override void FindFrame(int frameHeight)
    {
        ++NPC.frameCounter;
        if (NPC.frameCounter >= 7)
        {
            NPC.frame.Y += (frameHeight+1);
            NPC.frameCounter = 0;
            frameCounter2++;
            if (frameCounter2 >= 5)
            {
                NPC.frame.Y = 0;
                frameCounter2 = 0;
            }
        }
    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)//Hp * 1.5; damage * 2; defense * 1.2
    {
        NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment * 0.75f);
        NPC.damage = (int)(NPC.damage * balance * bossAdjustment);
        OriginalDamage = (int)(OriginalDamage * balance * bossAdjustment);
        NPC.defense = (int)(NPC.defense * balance * bossAdjustment * 1.2f);
    }//help, the boss keeps doing like 5 damage less than it's supposed to and it only does damage in multiples of 4, wtf

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        LeadingConditionRule NormalMode = new LeadingConditionRule(new Conditions.NotExpert());
        LeadingConditionRule PlanteraDead = new LeadingConditionRule(new Conditions.DownedPlantera());

        NormalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ReinforcedMushroot>(), 1, 15, 20));
        NormalMode.OnSuccess(ItemDropRule.Common(ItemID.GlowingMushroom, 1, 30, 40));

        PlanteraDead.OnSuccess(ItemDropRule.Common(ItemID.ShroomiteBar, 1, 12, 18));
        NormalMode.OnSuccess(PlanteraDead);

        npcLoot.Add(NormalMode);

        npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TreasureBagSporeGuardian>()));
    }

    public override void BossLoot(ref string name, ref int potionType)
    {
        potionType = ItemID.HealingPotion;
    }
    
    public override void OnKill()
    {
        NPC.SetEventFlagCleared(ref BossDownedSystem.downedSporeGuardian, -1);

        for (int i = 0; i < 50; i++)
        {
            Vector2 DustVelocity = Main.rand.NextVector2Circular(6, 6);

            Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Spores>(), DustVelocity.X, DustVelocity.Y, Scale: 2.5f);
        }

        if (Main.netMode != NetmodeID.Server)
        {
            int Left = Mod.Find<ModGore>("SporeGuardian_Left").Type;
            int Mid = Mod.Find<ModGore>("SporeGuardian_Mid").Type;
            int Right = Mod.Find<ModGore>("SporeGuardian_Right").Type;

            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Left);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Mid);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Mid);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Right);
        }
    }

    public override void AI()
    {
        DrawOffsetY = 80;

        LifeRatio = Main.expertMode ? NPC.life / (float)NPC.lifeMax : 1;

        ShootOffset = (NPC.rotation + MathHelper.PiOver2).ToRotationVector2() * 90;

        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest();
        }

        Target = Main.player[NPC.target];

        if (Main.player[NPC.target].dead)
        {
            AttackState = 9;

            NPC.velocity.X *= 0.97f;
            NPC.velocity.Y += 0.08f;
            NPC.rotation = NPC.AngleTo(Target.Center) + MathHelper.PiOver2;

            NPC.EncourageDespawn(30);
        }

        LineToPlayer = NPC.DirectionTo(Target.Center);
        LineToPlayer.SafeNormalize(new(1, 1));

        switch (AttackState)
        {
            case 0:
                ShootSpores();
                break;

            case 1:
                SpawnDash();
                break;

            case 2:
                WavyAttack();
                break;
        }

        if(Condition.InGlowshroom.IsMet() && Condition.InBelowSurface.IsMet())
        {
            LowerHPSpeed = MathHelper.Lerp(0.75f, 1, LifeRatio);
            NPC.defense = 9;
        }
        else if(Condition.InGlowshroom.IsMet() || Condition.InBelowSurface.IsMet())
        {
            LowerHPSpeed = MathHelper.Lerp(0.5f, 0.75f, LifeRatio); //enrage
            NPC.defense = 12;
        }
        else
        {
            LowerHPSpeed = MathHelper.Lerp(0.25f, 0.5f, LifeRatio); //discombobulate.
            NPC.defense = 15;
        }
    }   

    public void ShootSpores()
    {
        TargetPos = Target.Center + new Vector2(500 * WhichSide, -400);

        float TargetAngle = NPC.AngleTo(TargetPos);
        NPC.velocity = NPC.velocity.ToRotation().AngleTowards(TargetAngle, 1).ToRotationVector2() * ((NPC.Distance(TargetPos) * NPC.Distance(TargetPos)/1500));

        if(NPC.velocity.Length() > 15)
        {
            NPC.velocity = NPC.velocity.SafeNormalize(new(1,1)) * 15f;
        }

        NPC.rotation = NPC.AngleTo(Target.Center) + MathHelper.ToRadians(90);
        AttackTimer++;

        if (AttackTimer >= 111 * LowerHPSpeed) //Shoot
        {
            Methods.SimpleRandomProjectileSpread(NPC.GetSource_FromAI(), 4, 0.7f, 0.1f, NPC.Center - ShootOffset, LineToPlayer * 10, ModContent.ProjectileType<SporeDart>(), (int)((float)OriginalDamage * 0.9f * DiffAdjust), 2);

            AttackTimer = 0;
            AttackTimer2 += Main.rand.Next(4,7);
        }

        if (AttackTimer2 >= 24) //end
        {
            AttackState = Main.rand.Next(1,3);
            AttackTimer = 0;
            AttackTimer2 = 0;
        }

        //AttackTimer2 Is just a complicated way to make it randomly shoot 4-6 times (edit: nvm it's not very good either)
    }
    
    public void SpawnDash()
    {
        if(AttackState2 == 0) //Reach starting position, telegraph the dash by playing a roar
        {
            TargetPos = Target.Center + new Vector2(500 * WhichSide, 0);

            float TargetAngle = NPC.AngleTo(TargetPos);
            NPC.velocity = NPC.velocity.ToRotation().AngleTowards(TargetAngle, 0.1f).ToRotationVector2() * ((NPC.Distance(TargetPos) * NPC.Distance(TargetPos) / 2500));

            if (NPC.velocity.Length() > 15)
            {
                NPC.velocity = NPC.velocity.SafeNormalize(new(1, 1)) * 15f;
            }

            NPC.rotation = NPC.AngleTo(Target.Center) + MathHelper.PiOver2;

            AttackTimer++;
            if (AttackTimer == 1)
            {
                SoundEngine.PlaySound(SoundID.Roar);
            }

            if (AttackTimer >= 100)
            {
                AttackState2 = 1;
                AttackTimer = 0;
            }
        }

        else if(AttackState2 == 1) //Launches itself towards player
        {
            LowerHPSpeed2 = MathHelper.Lerp(0.90f, 1, LifeRatio);

            NPC.velocity = NPC.DirectionTo(Target.Center) * (25 / LowerHPSpeed);
            AttackState2 = 2;
        }

        else if (AttackState2 == 2) //slowing down and spawning
        {
            AttackTimer++;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Spores>(), Scale: 3f);
            Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Spores>(), Scale: 3f);
            Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Spores>(), Scale: 3f);

            TicksPerSpawn = LifeRatio < 0.15f ? 22 : 30; // spawn +1 enemy when below 15%

            if ((AttackTimer + 15) % TicksPerSpawn == 0)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, PossibleSpawns[Main.rand.Next(PossibleSpawns.Count)]);
            }

            if(AttackTimer > 18)
            {
                NPC.velocity *= 0.96f;
            }

            if(AttackTimer >= 90)
            {
                AttackTimer = 0;
                AttackState2 = 3;
            }
        }
        else if (AttackState2 == 3)
        {
            WhichSide = WhichSideList[Main.rand.Next(2)];

            AttackState2 = 4;
        }
        else if (AttackState2 == 4) //gradually increase speed and return
        {
            float Speed = MathHelper.Lerp(0, 15, AttackTimer / 45);

            AttackTimer++;

            TargetPos = Target.Center + new Vector2(600 * WhichSide, -400);

            float TargetAngle = NPC.AngleTo(TargetPos);
            NPC.velocity = NPC.velocity.ToRotation().AngleTowards(TargetAngle, 0.1f).ToRotationVector2() * ((NPC.Distance(TargetPos) * NPC.Distance(TargetPos) / 1500));

            if (NPC.velocity.Length() > Speed)
            {
                NPC.velocity = NPC.velocity.SafeNormalize(new(1, 1)) * Speed;
            }

            NPC.rotation = NPC.AngleTo(Target.Center) + MathHelper.PiOver2;

            if (AttackTimer == 45)
            {
                AttackTimer = 0;
                AttackState = 0;
                AttackState2 = 0;
            }
        }
    }

    public void WavyAttack()
    {
        
        if(AttackState2 == 0)
        {
            AttackTimer++;

            TargetPos = Target.Center + new Vector2(500 * WhichSide, 0);

            float TargetAngle = NPC.AngleTo(TargetPos);
            NPC.velocity = NPC.velocity.ToRotation().AngleTowards(TargetAngle, 0.1f).ToRotationVector2() * ((NPC.Distance(TargetPos) * NPC.Distance(TargetPos) / 2500));

            if (NPC.velocity.Length() > 15)
            {
                NPC.velocity = NPC.velocity.SafeNormalize(new(1, 1)) * 15f;
            }

            NPC.rotation = NPC.AngleTo(Target.Center) + MathHelper.PiOver2;

            if (AttackTimer >= 165 * LowerHPSpeed)
            {
                AttackState2 = 1;
                AttackTimer = 0;
            }
        }

        else if(AttackState2 == 1)
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - ShootOffset, new(-4f * WhichSide, 0), ModContent.ProjectileType<WavySporeTop>(), (int)((float)OriginalDamage * 0.9 * DiffAdjust), 2);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - ShootOffset, new(-4f * WhichSide, 0), ModContent.ProjectileType<WavySporeBottom>(), (int)((float)OriginalDamage * 0.9 * DiffAdjust), 2);

            if (LifeRatio <= 0.4f)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - ShootOffset, new(-6 * WhichSide, 0), ModContent.ProjectileType<SporeDart>(), (int)((float)OriginalDamage * 0.9 * DiffAdjust), 2);
            }

            TimesShot++;

            AttackState2 = 0;

            if (TimesShot >= 3)
            {
                AttackState2 = 2;
                AttackTimer = 0;
                TimesShot = 0;
            }
        }
        else if (AttackState2 == 2)
        {
            WhichSide = WhichSideList[Main.rand.Next(2)];

            AttackState2 = 3;
        }
        else if (AttackState2 == 3) //gradually increase speed and return
        {
            float Speed = MathHelper.Lerp(0, 15, AttackTimer / 45);

            AttackTimer++;

            TargetPos = Target.Center + new Vector2(600 * WhichSide, -400);

            float TargetAngle = NPC.AngleTo(TargetPos);
            NPC.velocity = NPC.velocity.ToRotation().AngleTowards(TargetAngle, 0.1f).ToRotationVector2() * ((NPC.Distance(TargetPos) * NPC.Distance(TargetPos) / 1500));

            if (NPC.velocity.Length() > Speed)
            {
                NPC.velocity = NPC.velocity.SafeNormalize(new(1, 1)) * Speed;
            }

            NPC.rotation = NPC.AngleTo(Target.Center) + MathHelper.PiOver2;

            if (AttackTimer == 45)
            {
                AttackTimer = 0;
                AttackState = 0;
                AttackState2 = 0;
            }
        }
    }
}
//expert Changes: the lower the HP, the faster it shoots and dashes (up to 25% faster; Dash only up to only 10%)
//defensive special spawns in expert? DONE