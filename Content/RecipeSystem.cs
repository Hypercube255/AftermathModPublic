using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using AftermathMod.Content.Items.Weapons;

namespace AftermathMod;

public class RecipeSystem : ModSystem
{
    public static RecipeGroup SilverBar;
    public static RecipeGroup GoldBar;
    public static RecipeGroup DemoniteBar;

    public override void AddRecipeGroups()
    {
        SilverBar = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SilverBar)}", ItemID.SilverBar, ItemID.TungstenBar);
        RecipeGroup.RegisterGroup(nameof(ItemID.SilverBar), SilverBar);

        GoldBar = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}", ItemID.GoldBar, ItemID.PlatinumBar);
        RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), GoldBar);

        DemoniteBar = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.DemoniteBar)}", ItemID.DemoniteBar, ItemID.CrimtaneBar);
        RecipeGroup.RegisterGroup(nameof(ItemID.DemoniteBar), DemoniteBar);
    }

    public override void PostAddRecipes()
    {
        for (int i = 0; i < Recipe.numRecipes; i++)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe.TryGetResult(ItemID.Zenith, out Item result))
            {
                recipe.AddIngredient(ModContent.ItemType<Tesseract>());
            }
        }
    }

    public override void Unload()
    {
        SilverBar = null;
        GoldBar = null;
        DemoniteBar = null;
    }
}