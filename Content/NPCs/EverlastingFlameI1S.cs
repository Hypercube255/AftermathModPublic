using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using AftermathMod;
using AftermathMod.Content.Items;
using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AftermathMod.Content.NPCs;

[AutoloadBossHead]
public class EverlastingFlameI1S : ModNPC
{
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

    enum AttackStates
    {
        ShootingFireballsUpwards,
        FlameRing,
        Dashing,
    }
    List<int> AttacksP1 = new List<int>
    {
        (int)AttackStates.ShootingFireballsUpwards,
        (int)AttackStates.FlameRing,
    };
    List<int> AttacksP2 = new List<int>
    {
        (int)AttackStates.ShootingFireballsUpwards,
        (int)AttackStates.FlameRing,
        (int)AttackStates.Dashing,
    };

    int CurState = (int)AttackStates.ShootingFireballsUpwards;
    //alternates between shooting a volley of small fireballs and shooting fireballs that rain down smaller ones
    //moves towards player - dust telegraph - shoots flamethrower projectiles in 8 directions (guaranteed hit if close) + a few small fireballs
    //closes shell - (stops moving - rotates towards player - dashes while spawning dusts) * 3 to 5 depending on health

    bool AutoAnim = true;

    int framecounterMain;

    int frameCounterRhombi;
    int frameRhombi;

    int frameCounterFire;
    int frameFire;

    int frameShell;

    Player Target;

    float LifeRatio;
    float LifeRatioPauses; //shorter pauses between attacks as the fight progresses
    float LifeRatioDashes;
    float LifeRatioOther; //fireballs, flamethrower

    int DamageOriginal;
    int SmallFireballDMG = Methods.UnvanillaDamage(22);
    int BigFireballDMG = Methods.UnvanillaDamage(26);
    int FlamethrowerDMG = Methods.UnvanillaDamage(32);

    float DamageExpertMultiplier = Main.expertMode ? 1.5f : 1f;

    Vector2 DashTarget = Vector2.One;
    
    //SFU
    int ProjectileCount = Main.expertMode ? 12 : 8;
    float Angle = MathHelper.ToRadians(60);
    float SpeedDifference = 0.1f;
    int ProjectileCount2 = 2;
    float Angle2 = MathHelper.ToRadians(125);
    int ShotCount;
    //FR
    int FRProjectileCount = Main.expertMode ? 14 : 10;
    float RingSizeMP = Main.expertMode ? 1.25f : 1;
    //D
    int frameStep = 6;
    int DashNumber = 3;
    int DashSpeedBase = Main.expertMode ? 40 : 32;

    float AddDashRatio = 0.667f;


    public override void SetDefaults()
    {
        NPC.width = 123;
        NPC.height = 123;
        NPC.lifeMax = 3000;
        NPC.defense = 10;
        NPC.value = 30000;
        NPC.HitSound = SoundID.NPCHit4;
        NPC.DeathSound = SoundID.NPCDeath56;
        NPC.aiStyle = -1;
        NPC.damage = 24;
        NPC.noGravity = true;
        NPC.boss = true;
        NPC.noTileCollide = true;
        NPC.knockBackResist = 0f;
        NPC.npcSlots = 7f;
        Music = MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/Mysterious_Contraption");

        DamageOriginal = NPC.damage;
        AttackState = (int)AttackStates.ShootingFireballsUpwards;
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D body = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/EverlastingFlameI1S").Value;
        Vector2 bodyPosition = new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f);

        spriteBatch.Draw(body, bodyPosition, null, drawColor, NPC.rotation, body.Size() * 0.5f, 1.2f, SpriteEffects.None, 0);


        Texture2D rhombi = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/I1Srhombi").Value;
        Rectangle rhombiSource = new Rectangle(0, frameRhombi * body.Height, body.Width, body.Height);

        spriteBatch.Draw(rhombi, bodyPosition, rhombiSource, drawColor, NPC.rotation, body.Size() * 0.5f, 1.2f, SpriteEffects.None, 0);


        Texture2D fire = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/I1Sfire").Value;
        Rectangle fireSource = new Rectangle(0, frameFire * body.Height, body.Width, body.Height);

        spriteBatch.Draw(fire, bodyPosition, fireSource, Color.White, NPC.rotation, body.Size() * 0.5f, 1.2f, SpriteEffects.None, 0);

        //this one is done manually
        Texture2D shell = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/I1Sshell").Value;
        Rectangle shellSource = new Rectangle(0, frameShell * body.Height, body.Width, body.Height);

        spriteBatch.Draw(shell, bodyPosition, shellSource, drawColor, NPC.rotation, body.Size() * 0.5f, 1.2f, SpriteEffects.None, 0);

        return false;
    }
    
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        LeadingConditionRule NormalMode = new LeadingConditionRule(new Conditions.NotExpert());

        NormalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BurningEmbers>(), minimumDropped: 10, maximumDropped: 13));
        NormalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<JournalPage1>()));

        npcLoot.Add(NormalMode);

        npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TreasureBagEverlastingFlameI1S>()));
    }
    public override void BossLoot(ref string name, ref int potionType)
    {
        potionType = ItemID.LesserHealingPotion;
    }

    public override void OnKill()
    {
        NPC.SetEventFlagCleared(ref BossDownedSystem.downedEverlastingFlame, -1);

        for(int i = 0; i<50; i++)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain, Scale: 2f);
        }

        if (Main.netMode != NetmodeID.Server)
        {
            int Body = Mod.Find<ModGore>("EverlastingFlameI1S_Body").Type;
            int Bowl = Mod.Find<ModGore>("EverlastingFlameI1S_Bowl").Type;
            int Rhombus = Mod.Find<ModGore>("EverlastingFlameI1S_Rhombus").Type;

            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Body);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Bowl);

            for(int i = 0; i < 4; i++)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Rhombus);
            }
        }
    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
    {
        NPC.lifeMax = (int)(NPC.lifeMax * balance * 0.75f);
        NPC.damage = (int)(NPC.damage * balance * 0.75f);
        NPC.defense = (int)(NPC.defense * balance * 1.2f);
    }

    public override void AI()
    {
        //frame counters
        if (AutoAnim)
        {
            framecounterMain++;

            if (framecounterMain % 8 == 0) //loops through the first 8 frames; advances every 8 ticks
            {
                frameCounterRhombi++;
            }
            frameRhombi = frameCounterRhombi % 8;

            if (framecounterMain % 6 == 0) //loops through the first 5 frames; advances every 6 ticks
            {
                frameCounterFire++;
            }
            frameFire = frameCounterFire % 5;
        }

        LifeRatio = NPC.life / (float)NPC.lifeMax; // 1 = full -> 0 = none
        LifeRatioPauses = float.Lerp(0.5f, 1, LifeRatio); 
        LifeRatioDashes = float.Lerp(0.67f, 1, LifeRatio / AddDashRatio);
        LifeRatioOther = float.Lerp(0.8f, 1, LifeRatio);

        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest();
        }

        Target = Main.player[NPC.target];

        if (Main.player[NPC.target].dead || Main.dayTime)
        {
            AttackState = 9;

            NPC.velocity.X *= 0.97f;
            NPC.velocity.Y -= 0.07f;
            NPC.rotation = NPC.rotation.AngleTowards(0f, 0.05f);

            NPC.EncourageDespawn(30);
        }

        else if (!(AttackState == (int)AttackStates.Dashing && AttackState2 == 2))
        {
            NPC.rotation = NPC.velocity.X / 20;
        }

        switch ((AttackStates)(int)AttackState)
        {
            case AttackStates.ShootingFireballsUpwards:
                ShootingFireballsUpwards();
                break;

            case AttackStates.FlameRing:
                FlameRing();
                break;

            case AttackStates.Dashing:
                Dashing();
                break;
        }
    }

    public void ShootingFireballsUpwards()
    {
        AttackTimer++;    
        Vector2 MoveHere;

        if (AttackState2 == 0)
        { 
            MoveHere = new Vector2(Target.position.X + (float)Math.Sin(AttackTimer / 100) * 500, Target.position.Y - 300 + (float)Math.Sin(AttackTimer / 50) * 75); // figure 8 movement loop
            NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, MoveHere, 12);

            if (AttackTimer == 1)
            {
                ShotCount = Main.rand.Next(5, 8);
            }

            if (AttackTimer % 120 == 0)
            {
                if (AttackTimer2 == 0 && Main.rand.NextBool())//start with random 
                {
                    AttackTimer2++;
                }

                if (AttackTimer2 % 2 == 0)
                {
                    Methods.SimpleRandomProjectileSpread(NPC.GetSource_FromAI(), ProjectileCount, Angle, SpeedDifference, NPC.Center, Vector2.UnitY * -8, ModContent.ProjectileType<I1SSmallFireball>(), (int)(SmallFireballDMG * DamageExpertMultiplier), 1, ai0: 1);
                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                }
                else
                {
                    Methods.SimpleProjectileSpread(NPC.GetSource_FromAI(), ProjectileCount2, Angle2, ModContent.ProjectileType<I1SBigFireball>(), Vector2.UnitY * -8, NPC.Center, 2.5f, (int)(BigFireballDMG * DamageExpertMultiplier), ai0: 1, ai2: SmallFireballDMG * DamageExpertMultiplier);
                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                }

                AttackTimer2++;
            }

            if (AttackTimer2 >= ShotCount)
            {
                AttackTimer = 0;
                AttackTimer2 = 0;
                AttackState2 = 1;
            }
        }
        else if (AttackState2 == 1)//wait for a bit after the attack
        {
            if (AttackTimer >= 30 * LifeRatioPauses)
            {
                AttackTimer = 0;
                AttackTimer2 = 0;
                AttackState = ChooseAttack();
                AttackState2 = 0;
            }
        }
    }

    public void FlameRing()
    {
        AttackTimer++;

        if (AttackState2 == 0) //move to player
        {
            NPC.damage = 0;

            if (NPC.Center.Distance(Target.Center) > 300)
            {
                NPC.velocity = Methods.BossSmoothMoveTo(NPC.whoAmI, Target.Center, 16);
            }
            else
            {
                AttackState2 = 1;
                AttackTimer = 0;
            }
        }
        else if (AttackState2 == 1) //slow down and eventually stop
        {
            NPC.velocity *= 0.94f;

            if (AttackTimer >= 30)
            {
                NPC.velocity = Vector2.Zero;
                AttackState2 = 2;
                AttackTimer = 0;
            }
        }
        else if (AttackState2 == 2)//dust telegraph and FLAMES
        {
            if (AttackTimer <= (100 * LifeRatioOther))
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 DustCircle = (Vector2.UnitY * 310 * RingSizeMP).RotatedByRandom(MathHelper.TwoPi);
                    Dust.NewDust(NPC.Center + DustCircle, 1, 1, DustID.LifeDrain, SpeedX: 0, SpeedY: 0, Scale: 1.5f);
                }
            }
            else
            {
                NPC.damage = DamageOriginal;

                if (AttackTimer % 8 == 0)
                {
                    Methods.SimpleProjectileSpread(NPC.GetSource_FromAI(), 8, MathHelper.ToRadians(315), ModContent.ProjectileType<I1SFlamethrower>(), (Vector2.UnitY * 15 * RingSizeMP).RotatedByRandom(MathHelper.TwoPi), NPC.Center, 1, (int)(FlamethrowerDMG * DamageExpertMultiplier));
                    SoundEngine.PlaySound(SoundID.Item34, NPC.Center);
                    AttackTimer2++;

                    if(AttackTimer2 >= 6)
                    {
                        Methods.SimpleRandomProjectileSpread(NPC.GetSource_FromAI(), FRProjectileCount, MathHelper.TwoPi, 0, NPC.Center, Vector2.UnitY * -6, ModContent.ProjectileType<I1SSmallFireball>(), (int)(SmallFireballDMG * DamageExpertMultiplier), 1);
                        SoundEngine.PlaySound(SoundID.Item20, NPC.Center);

                        AttackTimer = 0;
                        AttackTimer2 = 0;
                        AttackState2 = 3;
                    }
                }
            }
        }
        else if (AttackState2 == 3)//wait for a bit after the attack
        {
            if (AttackTimer >= 30 * LifeRatioPauses)
            {
                AttackTimer = 0;
                AttackTimer2 = 0;
                AttackState = ChooseAttack();
                AttackState2 = 0;
            }
        }
    }

    public void Dashing()
    {
        AttackTimer++;
        int DashSpeed = DashSpeedBase + (int)(NPC.Distance(Target.Center) / 75);

        DashNumber = (int)float.Lerp(6, 3, LifeRatio / AddDashRatio); // 3 -> 5

        if (AttackState2 == 0)// slow down
        {
            NPC.velocity *= 0.96f;

            if (AttackTimer >= 30 * LifeRatioDashes)
            {
                NPC.velocity = Vector2.Zero;

                AttackTimer = 0;
                AttackState2 = 1;
            }
        }
        else if (AttackState2 == 1)// close shell and wait ~0.5s
        {
            if (AutoAnim)
            {
                AutoAnim = false;

                frameFire = 4;
                frameRhombi = 6;
                frameShell = 4;
            }

            if (AttackTimer % frameStep == 0 && AttackTimer <= frameStep * 4)//all of them have 4 frames
            {
                frameFire++;
                frameRhombi++;
                frameShell++;
            }

            if (AttackTimer > 60 * LifeRatioDashes)
            {
                AttackTimer = 0;
                AttackState2 = 2;
            }
        }
        else if (AttackState2 == 2)
        {
            if (AttackTimer < 75 * LifeRatioDashes) // select and look at target
            {
                NPC.velocity = Vector2.Zero;
                NPC.rotation = NPC.AngleTo(Target.Center) - MathHelper.ToRadians(90);
                DashTarget = Target.Center;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 DustCircle = (Vector2.UnitY * 75).RotatedByRandom(MathHelper.TwoPi);
                    Dust.NewDust(NPC.Center + DustCircle, 1, 1, DustID.LifeDrain, SpeedX: 0, SpeedY: 0, Scale: 1f);
                }
            }
            else if (AttackTimer == (int)(90 * LifeRatioDashes)) // lunge
            {
                NPC.velocity = NPC.DirectionTo(DashTarget) * DashSpeed;
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
            }
            else if (AttackTimer > (int)(90 * LifeRatioDashes) && AttackTimer <= 135 * LifeRatioDashes)// slow down while releasing dusts
            {
                NPC.velocity *= 0.95f;

                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain);
                }
            }
            else if (AttackTimer > 135 * LifeRatioDashes)
            {
                AttackTimer = 0;
                AttackTimer2++;
            }

            if (AttackTimer2 >= DashNumber)
            {
                AttackState2 = 3;
            }
        }
        else if (AttackState2 == 3)
        {
            if (AttackTimer % frameStep == 0 && AttackTimer <= frameStep * 4)//all of them have 4 frames
            {
                frameFire++;
                frameRhombi--;
                frameShell++;
            }

            if (AttackTimer >= frameStep * 4)
            {
                AttackTimer = 0;
                AttackState2 = 10;
            }
        }
        else if (AttackState2 == 10)
        {
            AutoAnim = true;
            AttackTimer = 0;
            AttackTimer2 = 0;
            AttackState = ChooseAttack();
            AttackState2 = 0;
        }
    }
    public int ChooseAttack(int TheChosenOne = 0)
    {
        do
        {
            if (LifeRatio > AddDashRatio)
            {
                TheChosenOne = AttacksP1[Main.rand.Next(AttacksP1.Count)];
            }
            else
            {
                TheChosenOne = AttacksP2[Main.rand.Next(AttacksP2.Count)];
            }
        } while (CurState == TheChosenOne);

        CurState = TheChosenOne;
        return TheChosenOne;
    }
}//