using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace abundance_aeruta;

public static class Plugin {
    public const string Guid = "com.ryocery.abundance_aeruta";
    public const string Name = "Abundance";
    public const string Version = "1.1.0";
}

[BepInPlugin(Plugin.Guid, Plugin.Name, Plugin.Version)]
public class Abundance : BasePlugin {
    internal new static ManualLogSource Log { get; private set; } = null!;
    private static Harmony _harmony = null!;

    internal static ConfigEntry<bool> EnableMoneyMultiplier { get; private set; } = null!;
    internal static ConfigEntry<float> MoneyMultiplier { get; private set; } = null!;
    internal static ConfigEntry<bool> EnableMaterialMultiplier { get; private set; } = null!;
    internal static ConfigEntry<float> MaterialMultiplier { get; private set; } = null!;
    internal static ConfigEntry<bool> EnableWoodMultiplier { get; private set; } = null!;
    internal static ConfigEntry<float> WoodMultiplier { get; private set; } = null!;

    public const uint MoneyItemID = 0;
    public const uint WoodItemID = 3;

    public override void Load() {
        Log = base.Log;
        Log.LogInfo($"Plugin {Plugin.Guid} is loading...");

        EnableMoneyMultiplier = Config.Bind("Money Settings", "EnableMoneyMultiplier", true, "Enable money multiplier");
        MoneyMultiplier = Config.Bind("Money Settings", "MoneyMultiplier", 2.0f, "Money multiplier amount (e.g., 2.0 = double money)");
        EnableMaterialMultiplier = Config.Bind("Material Settings", "EnableMaterialMultiplier", true, "Enable material multiplier for monster drops");
        MaterialMultiplier = Config.Bind("Material Settings", "MaterialMultiplier", 2.0f, "Material multiplier amount (e.g., 2.0 = double materials)");
        EnableWoodMultiplier = Config.Bind("Wood Settings", "EnableWoodMultiplier", true, "Enable wood multiplier");
        WoodMultiplier = Config.Bind("Wood Settings", "WoodMultiplier", 2.0f, "Wood multiplier amount (e.g., 2.0 = double wood)");

        _harmony = new Harmony(Plugin.Guid);
        _harmony.PatchAll();

        Log.LogInfo($"Money multiplier: {(EnableMoneyMultiplier.Value ? $"{MoneyMultiplier.Value}x" : "Disabled")}");
        Log.LogInfo($"Material multiplier: {(EnableMaterialMultiplier.Value ? $"{MaterialMultiplier.Value}x" : "Disabled")}");
        Log.LogInfo($"Wood multiplier: {(EnableWoodMultiplier.Value ? $"{WoodMultiplier.Value}x" : "Disabled")}");
    }
}

