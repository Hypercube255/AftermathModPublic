using System.Net.Http;
using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using AftermathMod.Content.Items;

namespace AftermathMod.Content.NPCs;

[AutoloadBossHead]
public class EvilRings : ModNPC // MINION CANONS ATTACK AT 33% + "calling backup" message
{
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

    int RingsShot;
    float ProjRot = MathHelper.Pi;
    float TargetAngle;
    float Speed;
    Vector2 BossShootPosition;
    bool StartTimer = false;
    int frameCounter2;
    float ProjNum1 = Main.expertMode ? 10 : 8;
    int OriginalDamage;
    bool SpawnMinions = true;
    bool WhichAttack;

    float DiffAdjust = 0.5f; //the game keeps multiplying my damage numbers in certain spots - this should make up for it

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 4;
    }

    public override void SetDefaults()
    {
        NPC.width = 110;
        NPC.height = 110;
        NPC.lifeMax = 5000;
        NPC.defense = 10;
        NPC.value = 40000;
        NPC.HitSound = SoundID.NPCHit4;
        NPC.DeathSound = SoundID.NPCDeath56;
        NPC.aiStyle = -1;
        NPC.damage = 33;
        OriginalDamage = 28;
        NPC.noGravity = true;
        NPC.boss = true;
        NPC.noTileCollide = true;
        NPC.knockBackResist = 0f;
        NPC.npcSlots = 7f;
        Music = MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/Mysterious_Contraption");
    }

    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/EvilRings_glow",ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width*0.5f, NPC.position.Y - Main.screenPosition.Y+42f), NPC.frame, Color.White, NPC.rotation, new Vector2(70, 70), new Vector2(1, 1), SpriteEffects.None, 0f);
    }

    public override void FindFrame(int frameHeight)
    {
        ++NPC.frameCounter;
        if (NPC.frameCounter >= 15)
        {
            NPC.frame.Y += frameHeight;
            NPC.frameCounter = 0;
            frameCounter2++;
            if (frameCounter2 >= 4)
            {
                NPC.frame.Y = 0;
                frameCounter2 = 0;
            }
        }
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        LeadingConditionRule NormalMode = new LeadingConditionRule(new Conditions.NotExpert());

        NormalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MetallicFragments>(), minimumDropped: 6, maximumDropped: 7));
        NormalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<JournalPage2>()));

        npcLoot.Add(NormalMode);

        npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TreasureBagEvilRings>()));
    }
    public override void BossLoot(ref string name, ref int potionType)
    {
        potionType = ItemID.HealingPotion;
    }

    public override void OnKill()
    {
        NPC.SetEventFlagCleared(ref BossDownedSystem.downedEvilRings, -1);

        if (Main.netMode != NetmodeID.Server)
        {
            int Ring1 = Mod.Find<ModGore>("EvilRings_Ring1").Type;
            int Ring2 = Mod.Find<ModGore>("EvilRings_Ring2").Type;
            int Eye = Mod.Find<ModGore>("EvilRings_Eye").Type;

            for (int i = 0; i < 2; i++)//ring1 x3, ring2 x2, eye x1
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Ring1);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Ring2);
            }
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Ring1);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Eye);
        }

        for(int i = 0; i<50; i++)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain, Scale: 2f);
        }
    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
    {
        NPC.lifeMax = (int)(NPC.lifeMax * balance * 0.75f);
        NPC.damage = (int)(NPC.damage * balance * 0.75f);
        OriginalDamage = (int)(OriginalDamage * balance * 0.75f);
        NPC.defense = (int)(NPC.defense * balance * 1.2f);
    }

    public override void AI()
    {
        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest();
        }

        Player Target = Main.player[NPC.target];

        if (Main.player[NPC.target].dead || Main.dayTime)
        {
            AttackState = 9;

            NPC.velocity.X *= 0.97f;
            NPC.velocity.Y -= 0.07f;
            NPC.rotation += 0.01f * NPC.velocity.Y;

            NPC.EncourageDespawn(30);
        }

        else
        {
            NPC.rotation = NPC.velocity.X / 10;
        }

        if (AttackState == 0)
        {
            AttackTimer++;
            Speed = Main.expertMode ? 5f : 4f;

            ChaseAttack();

            if (AttackTimer > 820)
            {
                AttackState = 1;
                AttackTimer = 0;
            }
        }

        else if (AttackState == 1)
        {
            if (WhichAttack || NPC.life > NPC.lifeMax * 0.5)
            {
                Speed = 30f;
                AttackTimer++;

                if (AttackTimer == 1)
                {
                    BossShootPosition = new(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 250);
                }
                ShootRockets();
            }
            else
            {
                RingAttack();
            }
        }
        if(NPC.life < NPC.lifeMax/3 && SpawnMinions == true)
        {
            NPC.NewNPC(NPC.GetSource_FromAI(), (int)Target.Center.X - 500, 0, ModContent.NPCType<SupportDrone>());
            NPC.NewNPC(NPC.GetSource_FromAI(), (int)Target.Center.X + 500, 0, ModContent.NPCType<SupportDrone>());
            SpawnMinions = false;
            Main.NewText("FIREPOWER INSUFFICIENT; CALLING BACKUP", new Color(200, 25, 25));
        }
    }
    
    public virtual void ChaseAttack()
    {
        AttackTimer2++;
        if (AttackTimer2 > 200)
        {
            for (int L1 = 0; L1 < (NPC.life < NPC.lifeMax/3 ? ProjNum1+2 : ProjNum1); L1++)
            {
                Vector2 PreProjDir = new Vector2(0, 7);
                Vector2 FinalDirection = PreProjDir.RotatedBy(MathHelper.Lerp(-ProjRot, ProjRot, L1/ (NPC.life < NPC.lifeMax / 3 ? ProjNum1 + 2 : ProjNum1)));
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, FinalDirection, ProjectileID.DeathLaser, (int)((float)OriginalDamage * 0.8 * DiffAdjust), 2);

                AttackTimer2 = 0;
            }
        }
        TargetAngle = NPC.AngleTo(Main.player[NPC.target].Center);

        NPC.velocity = NPC.velocity.ToRotation().AngleTowards(TargetAngle, MathHelper.ToRadians(3)).ToRotationVector2() * Speed;

    }

    public virtual void ShootRockets()
    {
        if (StartTimer == true)
        {
            AttackTimer2++;
        }

        if (Vector2.Distance(BossShootPosition, NPC.Center) >16)//16 is just a number I guessed btw
        {
            NPC.damage = 0;

            float BossShootAngle = NPC.AngleTo(BossShootPosition);
            NPC.velocity = NPC.velocity.ToRotation().AngleTowards(BossShootAngle, MathHelper.TwoPi).ToRotationVector2() * Speed;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain);//loops in AI will crash game, so...
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain);
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain);
        }
        else
        {
            NPC.velocity = new(0, 0);
            NPC.rotation = 0f;
            StartTimer = true;

            if (AttackTimer2 > 30 && AttackTimer2 < 120 && AttackTimer2 % 15 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item61);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(-7,7),-11), ModContent.ProjectileType<EvilRingsRocket>(), (int)((float)OriginalDamage * 1.2 * DiffAdjust), 2);
            }
            if (AttackTimer2 == 60)
            {
                NPC.damage = Main.expertMode ? OriginalDamage * 2 : OriginalDamage;//expert doesn't double my thingies so this bs has to be here
            }
            if(AttackTimer2 >= 180)
            {
                AttackState = 0;
                AttackTimer = 0;
                AttackTimer2 = 0;
                WhichAttack = Main.rand.NextBool();
            }
        }
    }
    public virtual void RingAttack()
    {
        NPC.velocity *= 0.98f;
        AttackTimer2++;
        if (AttackTimer2 > 180)
        {
            SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            for (int L2 = 0; L2 < 4; L2++)
            {
                Vector2 PreProjDir2 = new Vector2(0, 6);
                Vector2 FinalDirection2 = PreProjDir2.RotatedBy(MathHelper.Lerp(-ProjRot, ProjRot, L2*0.25f ));
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, FinalDirection2, ModContent.ProjectileType<EvilRingsRing>(), (int)((float)OriginalDamage * 0.8 * DiffAdjust), 2);
            }
            AttackTimer2 = 0;
            RingsShot++;
        }
        if (RingsShot == 3)
        {
            RingsShot = 0;
            AttackState = 0;
            WhichAttack = Main.rand.NextBool();
        }
    }
}// expert changes: Speed: 4f -> 5f; Ring projectiles: 8 -> 10; Helt: 5000 -> 7500; Damage+