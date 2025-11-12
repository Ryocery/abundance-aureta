using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace abundance_aeruta;

[BepInPlugin(Guid, Name, Version)]
public class Abundance : BasePlugin {
    private const string Guid = "com.ryocery.abundance_aeruta";
    private const string Name = "Abundance";
    private const string Version = "1.1.0";
    
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
        Log.LogInfo($"Plugin {Guid} is loading...");

        EnableMoneyMultiplier = Config.Bind("Money Settings", "EnableMoneyMultiplier", true, "Enable money multiplier");
        MoneyMultiplier = Config.Bind("Money Settings", "MoneyMultiplier", 2.0f, "Money multiplier amount (e.g., 2.0 = double money)");
        EnableWoodMultiplier = Config.Bind("Wood Settings", "EnableWoodMultiplier", true, "Enable wood multiplier");
        WoodMultiplier = Config.Bind("Wood Settings", "WoodMultiplier", 2.0f, "Wood multiplier amount (e.g., 2.0 = double wood)");
        EnableMaterialMultiplier = Config.Bind("Material Settings", "EnableMaterialMultiplier", true, "Enable material multiplier for monster drops");
        MaterialMultiplier = Config.Bind("Material Settings", "MaterialMultiplier", 2.0f, "Material multiplier amount (e.g., 2.0 = double materials)");
        
        _harmony = new Harmony(Guid);
        _harmony.PatchAll();

        Log.LogInfo($"Money multiplier: {(EnableMoneyMultiplier.Value ? $"{MoneyMultiplier.Value}x" : "Disabled")}");
        Log.LogInfo($"Material multiplier: {(EnableMaterialMultiplier.Value ? $"{MaterialMultiplier.Value}x" : "Disabled")}");
        Log.LogInfo($"Wood multiplier: {(EnableWoodMultiplier.Value ? $"{WoodMultiplier.Value}x" : "Disabled")}");
    }
}

