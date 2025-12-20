using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using AftermathMod.Content.NPCs;
using AftermathMod.Content.Items;
using System.Collections.Generic;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace AftermathMod;

public class CrossMod : ModSystem
{
    public override void PostSetupContent()
    {
        //Bosses

        if (ModLoader.TryGetMod("BossChecklist", out Mod BossChecklist))
        {
            if (BossChecklist.Version < new Version(1, 6))
            {
                return;
            }


            int SpawnItemEverlastingFlameI1S = ModContent.ItemType<TrackingDevice>();
            LocalizedText SpawnInfoEverlastingFlameI1S = Language.GetText("Mods.AftermathMod.BossChecklistLocalization.SpawnInfo.EverlastingFlameI1S");

            var CustomPortraitEverlastingFlameI1S = (SpriteBatch spriteBatch, Rectangle rectangle, Color color) =>
            {
                Texture2D Portrait = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Portraits/EverlastingFlameI1S_Portrait").Value;
                Vector2 Position = new Vector2(rectangle.X + (rectangle.Width * 0.5f - Portrait.Width * 0.5f), rectangle.Y + (rectangle.Height * 0.5f - Portrait.Height * 0.5f));
                Vector2 Offset = new Vector2(-0.1f * Portrait.Width, -0.1f * Portrait.Height);

                spriteBatch.Draw(Portrait, Position + Offset, null, color, 0, Vector2.Zero, scale: 1.2f, SpriteEffects.None, 0);
            };

            BossChecklist.Call("LogBoss", Mod, nameof(EverlastingFlameI1S), 2.5f, () => BossDownedSystem.downedEverlastingFlame, ModContent.NPCType<EverlastingFlameI1S>(),
                new Dictionary<string, object>
                {
                    ["spawnItems"] = SpawnItemEverlastingFlameI1S,
                    ["spawnInfo"] = SpawnInfoEverlastingFlameI1S,
                    ["customPortrait"] = CustomPortraitEverlastingFlameI1S
                }
            );

            int SpawnItemEvilRings = ModContent.ItemType<EnhancedTrackingDevice>();
            LocalizedText SpawnInfoEvilRings = Language.GetText("Mods.AftermathMod.BossChecklistLocalization.SpawnInfo.EvilRings");

            BossChecklist.Call("LogBoss", Mod, nameof(EvilRings), 4.5f, () => BossDownedSystem.downedEvilRings, ModContent.NPCType<EvilRings>(),
                new Dictionary<string, object>
                {
                    ["spawnItems"] = SpawnItemEvilRings,
                    ["spawnInfo"] = SpawnInfoEvilRings

                }
            );

            int SpawnItemSporeGuardian = ModContent.ItemType<MushroomNucleus>();
            LocalizedText SpawnInfoSporeGuardian = Language.GetText("Mods.AftermathMod.BossChecklistLocalization.SpawnInfo.SporeGuardian");

            BossChecklist.Call("LogBoss", Mod, nameof(SporeGuardian), 6.5f, () => BossDownedSystem.downedSporeGuardian, ModContent.NPCType<SporeGuardian>(),
                new Dictionary<string, object>
                {
                    ["spawnItems"] = SpawnItemSporeGuardian,
                    ["spawnInfo"] = SpawnInfoSporeGuardian
                }
            );

            int SpawnItemStargazer = ModContent.ItemType<Kaleidotelescope>();
            LocalizedText SpawnInfoStargazer = Language.GetText("Mods.AftermathMod.BossChecklistLocalization.SpawnInfo.Stargazer");

            var CustomPortraitStargazer = (SpriteBatch spriteBatch, Rectangle rectangle, Color color) =>
            {
                Texture2D Portrait = ModContent.Request<Texture2D>("AftermathMod/Content/NPCs/Portraits/Stargazer_Portrait").Value;
                Vector2 Position = new Vector2(rectangle.X + (rectangle.Width * 0.5f - Portrait.Width * 0.5f), rectangle.Y + (rectangle.Height * 0.5f - Portrait.Height * 0.5f));
                Vector2 Offset = new Vector2(-0.2f * Portrait.Width, -0.2f * Portrait.Height);

                spriteBatch.Draw(Portrait, Position + Offset, null, color, 0, Vector2.Zero, scale: 1.4f, SpriteEffects.None, 0);
            };

            BossChecklist.Call("LogBoss", Mod, nameof(Stargazer), 19f, () => BossDownedSystem.downedStargazer, ModContent.NPCType<Stargazer>(),
                new Dictionary<string, object>
                {
                    ["spawnItems"] = SpawnItemStargazer,
                    ["spawnInfo"] = SpawnInfoStargazer,
                    ["customPortrait"] = CustomPortraitStargazer
                }
            );

            //MINIBOSSES

            LocalizedText SpawnInfoSeaDragon = Language.GetText("Mods.AftermathMod.BossChecklistLocalization.SpawnInfo.SeaDragon");
            string HeadSeaDragon = "AftermathMod/Content/NPCs/SeaDragon_HeadBC";
            BossChecklist.Call("LogMiniBoss", Mod, nameof(SeaDragon), 7.8f, () => BossDownedSystem.downedSeaDragon, ModContent.NPCType<SeaDragon>(),
                new Dictionary<string, object>
                {
                    ["spawnInfo"] = SpawnInfoSeaDragon,
                    ["overrideHeadTextures"] = HeadSeaDragon
                }
            );
        }
    }
}