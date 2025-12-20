using AftermathMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using AftermathMod.Content.Items;
using System.Diagnostics.Metrics;
using AftermathMod.Content.NPCs;

namespace AftermathMod.Content.NPCs;
public class SupportDrone : ModNPC
{
    float timer = 0;
    int timer2 = 0;
    float DiffAdjust = Main.expertMode ? 0.25f : 0.5f;
    public override void SetDefaults()
    {
        NPC.width = 44;
        NPC.height = 44;
        NPC.lifeMax = Main.expertMode ? 150 : 200;
        NPC.defense = 8;
        NPC.value = 0;
        NPC.HitSound = SoundID.NPCHit4;
        NPC.DeathSound = SoundID.NPCDeath14;
        NPC.aiStyle = -1;
        NPC.damage = 20;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.knockBackResist = 0f;
    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D texture = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/SupportDrone_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        Main.EntitySpriteDraw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f), new Rectangle(0,0, texture.Width, texture.Height), Color.White, NPC.rotation, texture.Size() * 0.5f, 1, SpriteEffects.None);
    }
    public override bool PreKill()
    {
        for (int i = 0; i < 20; i++)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain);
        }

        if (Main.netMode != NetmodeID.Server)
        {
            int Front = Mod.Find<ModGore>("SupportDrone_Front").Type;
            int Back = Mod.Find<ModGore>("SupportDrone_Back").Type;

            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Front);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-4, 5), Main.rand.NextFloat(-4, 5)), Back);
        }
        return true;
    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
    {
        NPC.lifeMax = (int)(NPC.lifeMax * balance * 0.75f);
        NPC.damage = (int)(NPC.damage * balance * 0.75f);
        NPC.defense = (int)(NPC.defense * balance * 1.2f);
    }

    public override void AI()
    {
        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest();
        }

        Player player = Main.player[NPC.target];

        if (timer == 0)
        {
            timer = NPC.position.X < player.position.X ? MathHelper.Pi : 0;
        }

        timer += 0.01f;
        timer2++;


        Vector2 PlayerCenter = player.RotatedRelativePoint(player.MountedCenter);

        Vector2 SavePosition = NPC.Center - PlayerCenter;

        NPC.Center = PlayerCenter - SavePosition;

        NPC.Center = PlayerCenter - new Vector2((float)System.Math.Cos(timer), (float)System.Math.Sin(timer)) * 500;

        NPC.rotation = (float)timer - MathHelper.PiOver2;

        if(timer2 == 300)
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0,5), NPC.DirectionTo(Main.player[NPC.target].Center) * 4, ProjectileID.PinkLaser, (int)(NPC.damage * DiffAdjust), 2);
            timer2 = 0;
        }

        if(NPC.CountNPCS(ModContent.NPCType<EvilRings>())<1)
        {
            NPC.StrikeInstantKill();
        }
    }
}