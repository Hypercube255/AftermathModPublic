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
using AftermathMod.Content.Dusts;
using System;
using AftermathMod.Assets;

namespace AftermathMod.Content.NPCs;
public class ShadowRune : ModNPC
{
    int frameCounter = 0;
    int frameCounter2 = 0;
    int frame;

    bool RareOrNot = Main.rand.NextBool(50);
    int WhichSymbol = Main.rand.Next(10);

    int timer;

    bool Heal = true;

    public override void SetDefaults()
    {
        NPC.width = 36;
        NPC.height = 36;
        NPC.lifeMax = 222;
        NPC.defense = 0;
        NPC.value = 0;
        NPC.HitSound = SoundID.NPCHit11;
        NPC.DeathSound = SoundID.Item10;
        NPC.aiStyle = -1;
        NPC.damage = 0;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.knockBackResist = 0f;
        NPC.Opacity = 0;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D Base = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/ShadowRune").Value;
        Vector2 position = new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y - NPC.height * 0.5f + Base.Height * 0.5f);

        Main.EntitySpriteDraw(Base, position, new Rectangle(0, 54 * frame, Base.Width, Base.Height / 5), Color.White * NPC.Opacity, NPC.rotation, Base.Size() * 0.5f, 1, SpriteEffects.None);

        Texture2D Symbol = SelectSymbol(RareOrNot, WhichSymbol);
        Vector2 position2 = new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width * 0.5f, NPC.position.Y - Main.screenPosition.Y + NPC.height * 0.5f);
        float Sin = ((float)Math.Sin(timer / 5) + 1) / 2;//sin between 0 and 1
        float Opacity = float.Lerp(0.6f, 1f, Sin);

        Main.EntitySpriteDraw(Symbol, position2, new Rectangle(0, 0, Symbol.Width, Symbol.Height), Color.White * Opacity * NPC.Opacity, NPC.rotation, Symbol.Size() * 0.5f, 1, SpriteEffects.None);

        return false;
    }

    public override bool PreKill()
    {
        Player HealPlayer = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];

        for (int i = 0; i < 10; i++)
        {
            Vector2 DustSpeed = (Vector2.UnitY * Main.rand.Next(2, 10)).RotatedByRandom(MathHelper.TwoPi);
            Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ShadedSparkles>(), DustSpeed.X, DustSpeed.Y, Scale: 2.5f);

            DustSpeed = (Vector2.UnitY * Main.rand.Next(2, 10)).RotatedByRandom(MathHelper.TwoPi);
            Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ShadedSmoke>(), DustSpeed.X, DustSpeed.Y, Scale: 2.5f);
        }

        if (Heal)
        {
            for (int i = 0; i < 40; i++) //green line towards player
            {
                float Rando = Main.rand.NextFloat();
                Vector2 HealDustPos = Vector2.Lerp(NPC.Center, HealPlayer.Center, Rando);

                Dust dust = Dust.NewDustDirect(HealDustPos, 3, 3, DustID.GreenTorch, Scale: 3f);
                dust.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item42, NPC.Center);

            HealPlayer.Heal(Main.rand.Next(20, 25));
        }

        return true;
    }
    public override void AI()
    {
        frameCounter++;
        if(frameCounter >= 6)
        {
            frameCounter = 0;
            frameCounter2++;
            frame = frameCounter2 % 5;
        }

        timer++;

        if (timer > 480)
        {
            NPC.Opacity -= 0.05f;
        }
        else if (NPC.Opacity < 1)
        {
            NPC.Opacity += 0.05f;
        }

        if (NPC.Opacity <= 0)
        {
            Heal = false;
            NPC.StrikeInstantKill();
        }
    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
    {
        NPC.lifeMax = 222;
    }

    private Texture2D SelectSymbol(bool rare, int type)
    {
        if(!rare)
        {
            type %= 2;
            switch (type)
            {
                case 0:
                    return ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Runes/RuneShi").Value;

                case 1:
                    return ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Runes/RuneEye").Value;

                default:
                    return AssetDatabase.EmptySprite.Value;
            }
        }
        else
        {
            type %= 3;
            switch (type)
            {
                case 0:
                    return ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Runes/RuneRareCatface").Value;

                case 1:
                    return ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Runes/RuneRare67").Value;

                case 2:
                    return ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Runes/RuneRareWSG").Value;

                default:
                    return AssetDatabase.EmptySprite.Value;
            }
        }
    }
}