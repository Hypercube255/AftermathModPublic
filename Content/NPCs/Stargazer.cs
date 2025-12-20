using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using AftermathMod;
using AftermathMod.Assets;
using AftermathMod.Content.Dusts;
using AftermathMod.Content.Items;
using AftermathMod.Content.Items.Weapons;
using AftermathMod.Content.Projectiles;
using AftermathMod.Content.StatusFX.Buffs;
using AftermathMod.Content.StatusFX.Debuffs;
using Humanizer;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Platforms;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.RGB;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;


namespace AftermathMod.Content.NPCs;

[AutoloadBossHead]

public class Stargazer : ModNPC
{

    int OriginalDamage;

    float LifeRatio;

    Vector2 TargetPosition;

    public SoundStyle DeathSound = new SoundStyle("AftermathMod/Content/Sounds/SoundFX/StargazerDeath");
    public SoundStyle BeepSound = new SoundStyle("AftermathMod/Content/Sounds/SoundFX/StargazerBeepTelegraph");
    public SoundStyle DashSound = new SoundStyle("AftermathMod/Content/Sounds/SoundFX/StargazerDash");

    Player Target;

    double SineTimer = 0;

    float BlurIntensity;

    float GalaxyOpacity = 0;

    double GalaxyTimer = 0;

    float ColorDash;

    float LaserWidth = 0;

    float SmoothRotate = 0;

    int ShotCountCurrent = 0;

    bool Phase2 => LifeRatio <= 0.6;

    bool TransitionDone = false;

    float TransTimer = 0;

    float RotAmount = MathHelper.ToRadians(1);

    int WarningTimer = 0;

    Vector2 ProjectileVelocity2 = Vector2.Zero;

    int frameCounter2 = 0;

    Vector2 ShootOffset;

    Color StardustBlue = new Color(91, 132, 255, 100);

    Vector2 DashTarget = Vector2.Zero;
    Vector2 TelegraphDashTarget = Vector2.Zero;

    int CurState = (int)AttackStates.Shooting;

    //-----CONTROL VALUES-----

    //Damage
    float DamageLaser = Methods.UnvanillaDamage(92);//and blasts
    float DamageSparkles = Methods.UnvanillaDamage(84);
    float DamageBeam = Methods.UnvanillaDamage(120);
    float DamageDash = Methods.UnvanillaDamage(112);

    float DamageExpertMultiplier = Main.expertMode ? 1.5f : 1f;

    //Shooting
    int ProjectileCount = 4;
    int ShotCount = 4;
    int ProjectileSpeed = 30;
    int ShootDelay = Main.expertMode ? 70 : 90;
    int TelegraphDelay = 30; //tied to the projectile value, don't forget

    //ShootingWaving
    int SWShotDelay = Main.expertMode ? 15 : 17;
    float SWSineSpeed = Main.expertMode ? 0.07f : 0.065f;

    //ShootingRandowWalls
    int SRwProjectileCount = Main.expertMode ? 15 : 12;
    int SRwShotDelay = Main.expertMode ? 100 : 115;
    float SRwShotSpread = 4;

    //Dash
    int DPredictionQuotient = Main.expertMode ? 50 : 25;
    int DDashVelocity = 100;

    //StarSparkles
    int SSAttackDuration = 600;
    int SSShotDelay = Main.expertMode ? 18 : 22;
    float SSStarSpeed = Main.expertMode ? 25 : 22;
    int SSLaserCount = 4;

    //Laser
    float LDebuffDist = 2000;
    int LProjectileSpeed2 = 3;
    int LShotCount = 3;
    int LLaserDuration = 480;
    bool LDoBlasts = Main.expertMode ? true : false;
    float LLaserSpeed = Main.expertMode ? 1f : 1.4f;

    Texture2D TelegraphLine = ModContent.Request<Texture2D>("AftermathMod/Assets/GlowLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

    List<int> WhichSideListX = new List<int> { 1, -1 };
    List<int> WhichSideListY = new List<int> { 1, -1 };
    int WhichSideX = 1;
    int WhichSideY = 1;

    enum AttackStates
    { 
        Shooting,
        ShootingWaving,
        ShootingRandomWalls,
        Dash,
        StarSparkles,
        Laser,
        RandomBasicAttack,
        TransitionP2
    }

    List<int> AttacksP1 = new List<int>
        {
            (int)AttackStates.Shooting,
            (int)AttackStates.ShootingWaving,
            (int)AttackStates.ShootingRandomWalls,
            (int)AttackStates.StarSparkles
        };

    List<int> AttacksP2 = new List<int>
        {
            (int)AttackStates.StarSparkles,
            (int)AttackStates.Dash,
            (int)AttackStates.Laser,
            (int)AttackStates.RandomBasicAttack
        };

    List<int> BasicAttacks = new List<int>
        {
            (int)AttackStates.Shooting,
            (int)AttackStates.ShootingWaving,
            (int)AttackStates.ShootingRandomWalls,
        };

    int AttackState
    {
        get => (int)NPC.ai[0];
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


    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        drawColor = Color.Lerp(drawColor, StardustBlue, ColorDash);

        Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Stargazer", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f), null, drawColor, NPC.rotation, texture.Size() * 0.5f, new Vector2(1.4f, 1.4f), SpriteEffects.None, 0f);

        Color GalaxyCol = Color.Lerp(Color.White, StardustBlue, GalaxyOpacity);

        Texture2D textureGlowGalaxy = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Stargazer_glowGalaxy", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        spriteBatch.Draw(textureGlowGalaxy, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f), null, GalaxyCol, NPC.rotation, texture.Size() * 0.5f, new Vector2(1.4f, 1.4f), SpriteEffects.None, 0f);

        Texture2D textureGlowFire = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Stargazer_glowFire", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        spriteBatch.Draw(textureGlowFire, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f), NPC.frame, Color.White, NPC.rotation, new(textureGlowFire.Width * 0.5f, (textureGlowFire.Height * 0.5f) / 6 + 11), new Vector2(1.4f, 1.4f), SpriteEffects.None, 0f);

        if (AttackState == (int)AttackStates.Dash && AttackState2 == 2 && AttackTimer > 1)//dash telegraph
        {
            Methods.DrawTelegraph(2, NPC.Center + ShootOffset, DashTarget, StardustBlue with { A = 0 }, 1);
        }
        else if (AttackState == (int)AttackStates.Dash && AttackState2 == 3)//dash trail
        {
            for (float i = NPC.oldPos.Length - 1; i > 0; i--)
            {
                Color TrailColor = new Color(0.36f, 0.52f, 1f, 0.29f) * MathHelper.Lerp(1, 0, i / 10);

                Vector2 DrawPosition = (NPC.oldPos[(int)i] - Main.screenPosition) + new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);

                Main.EntitySpriteDraw(texture, DrawPosition, null, TrailColor with { A = 0 }, NPC.rotation, texture.Size() * 0.5f, 1.4f, SpriteEffects.None, 0f);
            }
        }
        else if (AttackState == (int)AttackStates.Laser && (AttackState2 == 2 || AttackState2 == 3))
        {
            Methods.DrawLaser((int)Methods.LaserTypes.StargazerLaser, NPC.Center + ShootOffset * 1.2f, NPC.Center + ShootOffset * 50, LaserWidth);
        }

        return false;
    }

    public override void FindFrame(int frameHeight)
    {
        ++NPC.frameCounter;
        if (NPC.frameCounter >= 6)
        {
            NPC.frame.Y += frameHeight + 22;//size difference between fire and normal sprite
            NPC.frameCounter = 0;
            frameCounter2++;

            if (frameCounter2 >= 6)
            {
                NPC.frame.Y = 0;
                frameCounter2 = 0;
            }
        }
    }

    public override void SetStaticDefaults()
    {
        NPCID.Sets.TrailCacheLength[NPC.type] = 10;
        NPCID.Sets.TrailingMode[NPC.type] = 1;
    }

    public override void SetDefaults()
    {
        NPC.width = 160;
        NPC.height = 160;
        NPC.lifeMax = 200000;
        NPC.defense = 60;
        NPC.value = 1000000;
        NPC.HitSound = SoundID.NPCHit42;
        NPC.DeathSound = DeathSound;
        NPC.aiStyle = -1;
        NPC.damage = 100;
        OriginalDamage = NPC.damage;
        NPC.noGravity = true;
        NPC.boss = true;
        NPC.noTileCollide = true;
        NPC.knockBackResist = 0f;
        NPC.npcSlots = 10f;
        Music = MusicID.Boss2;

        AttackState = (int)AttackStates.Shooting; //always start with this attack

        DamageLaser *= DamageExpertMultiplier;
        DamageSparkles *= DamageExpertMultiplier;
        DamageDash *= DamageExpertMultiplier;
        DamageBeam *= DamageExpertMultiplier;
    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)//Hp * 1.5; damage * 1.5f; defense * 1.2
    {
        NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment * 0.75f);
        NPC.damage = (int)(NPC.damage * balance * bossAdjustment * 0.75f);
        OriginalDamage = (int)(OriginalDamage * balance * bossAdjustment);
        NPC.defense = (int)(NPC.defense * balance * bossAdjustment * 1.2f);
    }//help, the boss keeps doing like 5 damage less than it's supposed to and it only does damage in multiples of 4, wtf

    public override void OnKill()
    {
        NPC.SetEventFlagCleared(ref BossDownedSystem.downedStargazer, -1);

        if (Main.netMode != NetmodeID.Server)
        {
            int Eye = Mod.Find<ModGore>("Stargazer_Eye").Type;
            int RingL = Mod.Find<ModGore>("Stargazer_RingL").Type;
            int RingR = Mod.Find<ModGore>("Stargazer_RingR").Type;
            int FragM1 = Mod.Find<ModGore>("Stargazer_FragM1").Type;
            int FragM2 = Mod.Find<ModGore>("Stargazer_FragM2").Type;
            int FragGold = Mod.Find<ModGore>("Stargazer_FragGold").Type;
            int FragBack = Mod.Find<ModGore>("Stargazer_FragBack").Type;

            Vector2 GoreVelocity = new Vector2(Main.rand.NextFloat(-6, 7), Main.rand.NextFloat(-6, 7));

            Gore.NewGore(NPC.GetSource_Death(), NPC.Center,GoreVelocity, Eye);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, GoreVelocity, RingL);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, GoreVelocity, RingR);

            for (int i = 0; i < 3; i++)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, GoreVelocity, FragM1);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, GoreVelocity, FragM2);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, GoreVelocity, FragGold);

                if (i < 3) //spawns 2 Frags
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, GoreVelocity, FragBack);
                }
            }
        }
        for (int i = 0; i < 100; i++)
        {
            Vector2 DustVelocity = Main.rand.NextVector2Circular(10, 10);

            Dust.NewDust(NPC.Center, 0, 0, DustID.Clentaminator_Blue, DustVelocity.X, DustVelocity.Y, Scale: 1.5f);
        }

        Filters.Scene.Deactivate("AftermathMod:ScreenTint");
        Filters.Scene.Deactivate("AftermathMod:BoxBlur");
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        LeadingConditionRule NormalMode = new LeadingConditionRule(new Conditions.NotExpert());

        NormalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Tesseract>()));

        npcLoot.Add(NormalMode);

        npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TreasureBagStargazer>()));
    }

    public override void BossLoot(ref string name, ref int potionType)
    {
        potionType = ItemID.SuperHealingPotion;
    }

    public override void AI()
    {
        LifeRatio = NPC.life / (float)NPC.lifeMax;

        ShootOffset = Vector2.UnitY.RotatedBy(NPC.rotation) * 75;

        GalaxyTimer += 0.075f;

        GalaxyOpacity = ((float)Math.Sin(GalaxyTimer) + 1) / 2;

        if(!Phase2 && NPC.CountNPCS(ModContent.NPCType<Stargazer>()) == 1)
        {
            Filters.Scene.Deactivate("AftermathMod:ScreenTint");
        }

        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest();
        }

        Target = Main.player[NPC.target];

        if (Main.player[NPC.target].dead)
        {
            AttackState = 9;

            NPC.velocity.X *= 0.97f;
            NPC.velocity.Y -= 0.08f;
            NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);

            NPC.EncourageDespawn(30);

            Filters.Scene.Deactivate("AftermathMod:ScreenTint");
            Filters.Scene.Deactivate("AftermathMod:BoxBlur");
        }
        else
        {
            NPC.DiscourageDespawn(900);
        }

            switch ((AttackStates)(int)AttackState)
            {
                case AttackStates.Shooting:
                    Shooting();
                    break;

                case AttackStates.ShootingWaving:
                    ShootingWaving();
                    break;

                case AttackStates.ShootingRandomWalls:
                    ShootingRandomWalls();
                    break;

                case AttackStates.Dash:
                    Dash();
                    break;

                case AttackStates.StarSparkles:
                    StarSparkles();
                    break;

                case AttackStates.Laser:
                    Laser();
                    break;

                case AttackStates.RandomBasicAttack:
                    AttackState = BasicAttacks[Main.rand.Next(BasicAttacks.Count)];
                    break;

                case AttackStates.TransitionP2:
                    TransitionP2();
                    break;
            }
    }
    public void Shooting()
    {
        Vector2 TargetPos = Target.Center + new Vector2(600 * WhichSideX, -450 * WhichSideY);

        NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, TargetPos, 25);

        if(Vector2.Distance(NPC.Center, TargetPos) >= 300)// keep moving if too far
        {
            NPC.damage = 0;

            NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);
            return;
        }
        else
        {
            NPC.damage = OriginalDamage;
        }

        if (AttackState2 == 0)
        {
            Vector2 ProjectileVelocity = NPC.Center.DirectionTo(Target.Center) * ProjectileSpeed;

            AttackTimer++;

            if (AttackTimer < TelegraphDelay && AttackTimer2 > 0)
            {
                NPC.velocity = Vector2.Zero;
            }
            else if (AttackTimer == TelegraphDelay && AttackTimer2 > 0)
            {
                SoundEngine.PlaySound(SoundID.Item33, NPC.position);
            }
            else
            {
                NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);
            }

            if (AttackTimer > ShootDelay)
            {
                Methods.SimpleProjectileSpread(NPC.GetSource_FromAI(), ProjectileCount + Main.rand.Next(2), MathHelper.ToRadians(60), ModContent.ProjectileType<StargazerLaserTelegraphed>(), ProjectileVelocity, NPC.Center + ShootOffset, 4, (int)DamageLaser, NPC.whoAmI);
                AttackTimer = 0;
                AttackTimer2++;
            }

            if (AttackTimer2 >= ShotCount)
            {
                AttackState2 = 1;
                AttackTimer = 0;
            }
        }
        if(AttackState2 == 1)
        {
            AttackTimer++;

            if (AttackTimer < TelegraphDelay)
            {
                NPC.velocity = Vector2.Zero;
            }
            else if(AttackTimer == TelegraphDelay)
            {
                SoundEngine.PlaySound(SoundID.Item33, NPC.position);
            }
            else
            {
                NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);
            }

            if (AttackTimer >= ShootDelay)//boss AI change is delayed because telegraphs are baked into projectile AI
            {
                ChooseAttack();
                AttackState2 = 0;
                AttackTimer = 0;
                AttackTimer2 = 0;
            }
        }
    }

    public void ShootingWaving()
    {
        if (AttackState2 == 0)
        {
            Vector2 TargetPos = Target.Center + new Vector2(750 * WhichSideX, 0);

            if (Vector2.Distance(NPC.Center, TargetPos) >= 300)// keep moving if too far
            {
                NPC.damage = 0;

                NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);

                NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, TargetPos, 20);
                return;
            }
            else
            {
                NPC.damage = OriginalDamage;
                AttackState2 = 1;
                AttackTimer = 0;
            }
        }
        else if(AttackState2 == 1)
        {
            AttackTimer++;

            SineTimer += SWSineSpeed;

            NPC.rotation = MathHelper.ToRadians(90 * WhichSideX);

            Vector2 ProjectileVelocity = new(-10 * WhichSideX, 0);

            Vector2 ProjectileVelocity2 = (NPC.Center + ShootOffset).DirectionTo(Target.Center) * 10;

            float PositionY = (float)Math.Sin(SineTimer) * 500;

            Vector2 TargetPos = Target.Center + new Vector2(750 * WhichSideX, PositionY);

            NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, TargetPos, 50);

            if (AttackTimer % SWShotDelay == 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + ShootOffset, ProjectileVelocity, ModContent.ProjectileType<StargazerBlast>(), (int)DamageLaser, 4);
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);

                AttackTimer2++;
            }

            if(AttackTimer2 % 5 == 0 && AttackTimer % SWShotDelay == 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + ShootOffset, ProjectileVelocity2, ModContent.ProjectileType<StargazerBlast>(), (int)DamageLaser, 4);
                SoundEngine.PlaySound(SoundID.Item8, NPC.position);
            }

            if (AttackTimer >= 360)
            {
                ChooseAttack();
                AttackState2 = 0;
                AttackTimer = 0;
                AttackTimer2 = 0;

                SineTimer = 0;
            }
        }  
    }

    public void ShootingRandomWalls()
    {
        Vector2 TargetPos = Target.Center + new Vector2(777 * WhichSideX, 0);

        NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);

        if (AttackState2 == 0)
        {
            if (Vector2.Distance(NPC.Center, TargetPos) >= 300)// keep moving if too far
            {
                NPC.damage = 0;

                NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, TargetPos, 25);
                return;
            }
            else
            {
                NPC.damage = OriginalDamage;
            }

            AttackTimer++;

            NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, TargetPos, 18);

            if (AttackTimer > SRwShotDelay)
            {
                for (int i = 0; i < SRwProjectileCount; i++)
                {
                    Vector2 ProjVelocity = new(-5 * WhichSideX, Main.rand.NextFloat(-SRwShotSpread, SRwShotSpread));
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + ShootOffset + ProjVelocity * 5, ProjVelocity, ModContent.ProjectileType<StargazerBlast>(), (int)DamageLaser, 4, ai0: 1);
                    SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                }
                AttackTimer = 0;
                AttackTimer2++;
            }

            if (AttackTimer2 >= 3)
            {
                AttackState2 = 1;
                AttackTimer = 0;
            }
        }
        else if (AttackState2 == 1)
        {
            AttackTimer++;

            if (AttackTimer > 60)
            {
                ChooseAttack();
                AttackState2 = 0;
                AttackTimer = 0;
                AttackTimer2 = 0;
            }
        }
    }

    public void Dash()//predictive, blue flash on boss, telegraph, blur shader
    {
        if(AttackTimer2 < 2)
        {
            AttackTimer++;

            if (AttackState2 == 0)// move closer if too far
            {
                if (Vector2.Distance(NPC.Center, Target.Center) >= 1000)
                {
                    NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, Target.Center, 30);
                    NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);
                }
                else
                {
                    AttackState2 = 1;
                    AttackTimer = 0;
                }
            }
            else if (AttackState2 == 1)// slow down and eventually stop
            {
                NPC.velocity *= 0.95f;
                if (AttackTimer >= 30)
                {
                    NPC.velocity = Vector2.Zero;
                    AttackState2 = 2;
                    AttackTimer = 0;
                }
            }
            else if (AttackState2 == 2)// choose target and play sounds
            {
                if (AttackTimer <= 90)
                {
                    DashTarget = NPC.Center + ((Target.Center + Target.velocity * DPredictionQuotient) - NPC.Center).SafeNormalize(Vector2.UnitX) * 4000;
                }

                NPC.rotation = NPC.AngleTo(DashTarget) - MathHelper.ToRadians(90);

                if (AttackTimer == 90)
                {
                    SoundEngine.PlaySound(BeepSound, Target.Center);
                }
                else if (AttackTimer < 120)
                {
                    ColorDash += 1f / 30f;
                }
                else if (AttackTimer == 120)
                {
                    SoundEngine.PlaySound(DashSound, NPC.Center);
                    AttackState2 = 3;
                    AttackTimer = 0;
                }
            }
            else if (AttackState2 == 3)
            {
                if (AttackTimer < 15)//lunge
                {
                    if (AttackTimer == 1)
                    {
                        NPC.damage = (int)DamageDash;

                        BlurIntensity = 0.5f;
                        Filters.Scene.Activate("AftermathMod:BoxBlur");
                    }

                    Filters.Scene["AftermathMod:BoxBlur"].GetShader().UseIntensity(BlurIntensity);

                    BlurIntensity -= 0.5f / 14f;

                    NPC.velocity = NPC.DirectionTo(DashTarget) * DDashVelocity;
                }
                else if (AttackTimer < 90)//slow down for a bit
                {
                    Filters.Scene.Deactivate("AftermathMod:BoxBlur");
                    ColorDash = 0;

                    NPC.velocity *= 0.95f;
                }
                else
                {
                    AttackState2 = 0;
                    AttackTimer = 0;
                    AttackTimer2++;

                    NPC.damage = OriginalDamage;
                }
            }
        }
        else
        {
            ChooseAttack();
            AttackState2 = 0;
            AttackTimer = 0;
            AttackTimer2 = 0;
        }
    }

    public void StarSparkles()
    {
        AttackTimer++;

        NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);

        if (AttackState2 == 0)
        {
            if (AttackTimer == 1)
            {
                SoundEngine.PlaySound(SoundID.Item105, NPC.position);

                for (int i = 0; i < 150; i++)
                {
                    Vector2 DustVelocity = Main.rand.NextVector2Circular(10, 10);

                    Dust.NewDust(NPC.Center, 0, 0, DustID.Clentaminator_Blue, DustVelocity.X, DustVelocity.Y, Scale: 1.5f);

                }

                foreach (var Player in Main.ActivePlayers)//spawn helper projectile for each player so the ring stops disappearing
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<RingTelegraphHelper>(), 0, 0, ai0: Player.whoAmI, ai1: SSAttackDuration);
                }
            }

            NPC.velocity *= 0.95f;

            if (AttackTimer >= 45)
            {
                NPC.velocity = Vector2.Zero;

                AttackTimer = 0;
                AttackTimer2 = 0;
                AttackState2 = 1;

                NPC.damage = 0;
            }
        }
        else if (AttackState2 == 1)
        {
            if(AttackTimer % SSShotDelay == 0)
            {
                Vector2 SparklePos = new Vector2(Main.rand.NextFloat(Main.screenPosition.X, Main.screenPosition.X + Main.screenWidth), Main.rand.NextFloat(Main.screenPosition.Y, Main.screenPosition.Y + Main.screenHeight));

                Projectile.NewProjectile(NPC.GetSource_FromAI(), SparklePos, Vector2.Zero, ModContent.ProjectileType<StargazerSparkle>(), 0, 3, ai0: SSStarSpeed, ai2: (int)DamageSparkles);
                SoundEngine.PlaySound(SoundID.Item9, SparklePos);
            }

            if (LifeRatio < 0.2f && AttackTimer % 180 == 0)
            {
                for (int i = 0; i < SSLaserCount; i++)
                {
                    float RandomX = Main.rand.NextFloat(5);//maxValue can be basically anything
                    float RandomY = Main.rand.NextFloat(5);

                    Vector2 Pos = Target.Center - new Vector2((float)System.Math.Cos(RandomX), (float)System.Math.Sin(RandomX)) * 1111;

                    Vector2 Vel = Pos.DirectionTo(Target.Center) * 30;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), Pos, Vel, ModContent.ProjectileType<StargazerLaserTelegraphed>(), (int)DamageLaser, 3, ai0: 123456);
                }
            }

            if(AttackTimer >= SSAttackDuration)
            {
                AttackTimer = 0;
                AttackTimer2 = 0;
                AttackState2 = 0;

                NPC.damage = OriginalDamage;
                ChooseAttack();
            }
        }
    }
    public void Laser()
    {
        AttackTimer++;

        if(NPC.Center.Distance(Main.LocalPlayer.Center) > LDebuffDist && AttackState2 >= 1)
        {
            WarningTimer++;
            Main.LocalPlayer.AddBuff(ModContent.BuffType<PainfulEnlightenment>(), 600); //too far = prank

            if(WarningTimer == 1)
            {
                Main.NewText("Come back...", new Color(210, 251, 255, 255));
            }
        }
        else
        {
            WarningTimer = 0;
        }

        if (AttackState2 == 0)
        {
            if (Vector2.Distance(NPC.Center, Target.Center) >= 1000)
            {
                NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, Target.Center, 30);
                NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);
                AttackTimer--;
            }
            else
            {
                NPC.rotation = NPC.rotation.AngleTowards(-MathHelper.Pi, 0.1f);
                NPC.velocity *= 0.95f;

                if (AttackTimer >= 60)
                {
                    AttackTimer = 0;
                    AttackState2 = 1;
                }
            }

        }
        else if (AttackState2 == 1)
        {
            NPC.velocity = Vector2.Zero;

            Dust.NewDust(NPC.Center + ShootOffset * Main.rand.NextFloat(1, 20), 1, 1, DustID.Clentaminator_Blue, Scale: 0.75f);// I can explain
            Dust.NewDust(NPC.Center + ShootOffset * Main.rand.NextFloat(1, 20), 1, 1, DustID.Clentaminator_Blue, Scale: 0.75f);
            Dust.NewDust(NPC.Center + ShootOffset * Main.rand.NextFloat(1, 20), 1, 1, DustID.Clentaminator_Blue, Scale: 0.75f);
            Dust.NewDust(NPC.Center + ShootOffset * Main.rand.NextFloat(1, 20), 1, 1, DustID.Clentaminator_Blue, Scale: 0.75f);
            Dust.NewDust(NPC.Center + ShootOffset * Main.rand.NextFloat(1, 20), 1, 1, DustID.Clentaminator_Blue, Scale: 0.75f);

            ColorDash += 1 / 90;

            if (AttackTimer >= 90)
            {
                AttackTimer = 0;
                AttackState2 = 2;
            }
        }
        else if (AttackState2 == 2)
        {
            if (AttackTimer == 1)
            {
                SoundEngine.PlaySound(SoundID.Zombie104, NPC.Center);
            }

            if (LaserWidth < 1)
            {
                LaserWidth += 0.1f;
                ColorDash -= 0.1f;
            }
            else
            {
                ColorDash = 0;
                LaserWidth = 1;
                AttackTimer = 0;
                AttackState2 = 3;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + ShootOffset, Vector2.Zero, ModContent.ProjectileType<StargazerBeam>(), (int)DamageBeam, 3, ai0: NPC.whoAmI, ai1: LLaserDuration);
            }

        }
        else if (AttackState2 == 3)
        {
            Vector2 LShootOffset2 = ProjectileVelocity2.SafeNormalize(Vector2.UnitY) * 50;

            AttackTimer2++;

            NPC.rotation += MathHelper.ToRadians(LLaserSpeed) * SmoothRotate * WhichSideX;

            if (LaserWidth > 0.5f)
            {
                Dust.NewDust(NPC.Center + ShootOffset * Main.rand.NextFloat(1, 50), 1, 1, DustID.Clentaminator_Blue, Scale: 0.75f);
                Dust.NewDust(NPC.Center + ShootOffset * Main.rand.NextFloat(1, 50), 1, 1, DustID.Clentaminator_Blue, Scale: 0.75f);
            }

            if (AttackTimer < LLaserDuration)
            {
                Vector2 DustCircle = (Vector2.UnitY * 1500).RotatedByRandom(MathHelper.TwoPi);

                if (AttackTimer2 == 90)
                {
                    ProjectileVelocity2 = (Vector2.UnitY * LProjectileSpeed2).RotatedByRandom(MathHelper.TwoPi);
                }
                if (AttackTimer2 > 90)
                {
                    if (AttackTimer2 % 15 == 0 && LDoBlasts)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + LShootOffset2, new Vector2(ProjectileVelocity2.X, ProjectileVelocity2.Y), ModContent.ProjectileType<StargazerBlast>(), (int)DamageLaser, 4);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-LShootOffset2.X, LShootOffset2.Y), new Vector2(-ProjectileVelocity2.X, ProjectileVelocity2.Y), ModContent.ProjectileType<StargazerBlast>(), (int)DamageLaser, 4);
                        SoundEngine.PlaySound(SoundID.Item8, NPC.position);

                        ProjectileVelocity2 = ProjectileVelocity2.RotatedBy(MathHelper.ToRadians(30));

                        ShotCountCurrent++;
                    }

                    if (ShotCountCurrent >= LShotCount)
                    {
                        AttackTimer2 = 0;
                        ShotCountCurrent = 0;
                    }
                }


                if (SmoothRotate < 1)
                {
                    SmoothRotate += 0.01f;
                }
                else
                {
                    SmoothRotate = 1;
                }
            }
            else
            {
                if (LaserWidth > 0)
                {
                    LaserWidth -= 0.03f;
                }
                else
                {
                    LaserWidth = 0;
                }

                if (SmoothRotate > 0)
                {
                    SmoothRotate -= 0.01f;
                }
                else
                {
                    SmoothRotate = 0;
                }

                if (SmoothRotate == 0 && LaserWidth == 0)
                {
                    AttackTimer = 0;
                    AttackTimer2 = 0;
                    AttackState2 = 0;
                    ChooseAttack();
                }
            }
        }
    }
    public void TransitionP2()
    {
        float TransDuration = 150;

        ChooseAttack();

        Vector2 DustVelocity = Main.rand.NextVector2Circular(10, 10);

        Dust.NewDust(NPC.Center, 0, 0, DustID.Clentaminator_Blue, DustVelocity.X, DustVelocity.Y, Scale: 1.5f);

        TransTimer++;

        NPC.velocity *= 0.98f;

        if (TransTimer == 1)
        {
            Filters.Scene.Activate("AftermathMod:ScreenTint");
        }

        if(TransTimer < TransDuration / 2)
        {
            RotAmount *= 1.04f;
            NPC.rotation += RotAmount;
        }
        else if (TransTimer == TransDuration / 2)
        {
            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
        }
        else
        {
            RotAmount *= 0.96f;
            NPC.rotation += RotAmount;
        }
            Filters.Scene["AftermathMod:ScreenTint"].GetShader().UseColor(0.36f, 0.52f, 1f).UseIntensity(TransTimer / TransDuration);//stardust blue with floats

        if (TransTimer >= TransDuration)
        {
            TransitionDone = true;
        }
    }

    public void ChooseAttack()//chooses attack type, side, and makes no attack happen multiple times in a row
    {
        WhichSideX = WhichSideListX[Main.rand.Next(WhichSideListX.Count)];
        WhichSideY = WhichSideListY[Main.rand.Next(WhichSideListY.Count)];

        do
        {
            if (Phase2 == false)
            {
                AttackState = AttacksP1[Main.rand.Next(AttacksP1.Count)];
            }
            else if (Phase2 == true && TransitionDone == false)
            {
                AttackState = (int)AttackStates.TransitionP2;
            }
            else
            {
                AttackState = AttacksP2[Main.rand.Next(AttacksP2.Count)];
            }
        } while (CurState == AttackState && AttackState != (int)AttackStates.TransitionP2);

        CurState = AttackState;
    }

}//fight progress, real damage/hp/etc. values