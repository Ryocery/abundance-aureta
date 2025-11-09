using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace abundance_aeruta;

public static class PluginInfo {
    public const string PluginGuid = "com.ryocery.abundance_aeruta";
    public const string PluginName = "Abundance";
    public const string PluginVersion = "1.0.0";
}

[BepInPlugin(PluginInfo.PluginGuid, PluginInfo.PluginName, PluginInfo.PluginVersion)]
public class Abundance : BasePlugin {
    internal new static ManualLogSource Log { get; private set; } = null!;
    private static Harmony _harmony = null!;
    
    internal static ConfigEntry<bool> EnableMoneyMultiplier { get; private set; } = null!;
    internal static ConfigEntry<float> MoneyMultiplier { get; private set; } = null!;
    internal static ConfigEntry<bool> EnableMaterialMultiplier { get; private set; } = null!;
    internal static ConfigEntry<float> MaterialMultiplier { get; private set; } = null!;
    
    public const uint MoneyItemID = 0;
    
    public override void Load() {
        Log = base.Log;
        Log.LogInfo($"Plugin {PluginInfo.PluginGuid} is loading...");
        
        EnableMoneyMultiplier = Config.Bind("Money Settings", "EnableMoneyMultiplier", true, "Enable money multiplier");
        MoneyMultiplier = Config.Bind("Money Settings", "MoneyMultiplier", 2.0f, "Money multiplier amount (e.g., 2.0 = double money)");
        EnableMaterialMultiplier = Config.Bind("Material Settings", "EnableMaterialMultiplier", true, "Enable material multiplier for monster drops");
        MaterialMultiplier = Config.Bind("Material Settings", "MaterialMultiplier", 2.0f, "Material multiplier amount (e.g., 2.0 = double materials)");
        
        _harmony = new Harmony(PluginInfo.PluginGuid);
        _harmony.PatchAll();
        
        Log.LogInfo($"Money multiplier: {(EnableMoneyMultiplier.Value ? $"{MoneyMultiplier.Value}x" : "Disabled")}");
        Log.LogInfo($"Material multiplier: {(EnableMaterialMultiplier.Value ? $"{MaterialMultiplier.Value}x" : "Disabled")}");
    }
}

[HarmonyPatch(typeof(Inventory), nameof(Inventory.AddItem))]
public class InventoryAddItemPatch {
    static void Prefix(ItemStack newItem, bool showNotice) {
        if (newItem?.Base == null) return;
        
        try {
            uint itemId = newItem.Base.Id;
            eItemType itemType = newItem.Base.ItemType;
            int originalAmount = newItem.Amount;

            if (itemId == Abundance.MoneyItemID && Abundance.EnableMoneyMultiplier.Value) {
                newItem.Amount = (int)(originalAmount * Abundance.MoneyMultiplier.Value);
                Abundance.Log.LogInfo($"Money multiplied: {originalAmount} -> {newItem.Amount}");
            }

            else if (itemType == eItemType.Material && Abundance.EnableMaterialMultiplier.Value) {
                newItem.Amount = (int)(originalAmount * Abundance.MaterialMultiplier.Value);
                Abundance.Log.LogInfo($"Material multiplied (ID={itemId}): {originalAmount} -> {newItem.Amount}");
            }
            
        } catch (Exception ex) {
            Abundance.Log.LogError($"Error in patch: {ex}");
        }
    }
}