using abundance_aeruta;
using HarmonyLib;

[HarmonyPatch(typeof(Inventory), nameof(Inventory.AddItem))]
public class InventoryPatch {
    static void Prefix(ItemStack newItem, bool dontShowNotice) {
        if (newItem.Base == null) return;

        try {
            uint itemId = newItem.Base.Id;
            eItemType itemType = newItem.Base.ItemType;
            int originalAmount = newItem.Amount;

            switch (itemId) {
                case Abundance.MoneyItemID when Abundance.EnableMoneyMultiplier.Value:
                    newItem.Amount = (int)(originalAmount * Abundance.MoneyMultiplier.Value);
                    Abundance.Log.LogInfo($"Money multiplied: {originalAmount} -> {newItem.Amount}");
                    break;
                case Abundance.WoodItemID when Abundance.EnableWoodMultiplier.Value:
                    newItem.Amount = (int)(originalAmount * Abundance.WoodMultiplier.Value);
                    Abundance.Log.LogInfo($"Wood multiplied: {originalAmount} -> {newItem.Amount}");
                    break;
                default: {
                    if (itemType == eItemType.Material && Abundance.EnableMaterialMultiplier.Value) {
                        newItem.Amount = (int)(originalAmount * Abundance.MaterialMultiplier.Value);
                        Abundance.Log.LogInfo($"Material multiplied (ID={itemId}): {originalAmount} -> {newItem.Amount}");
                    }

                    break;
                }
            }
            
            Abundance.Log.LogInfo($"Adding item ID={itemId}, Type={itemType}, Amount={newItem.Amount}, Name={newItem.Base.Name.ToString()}");
        } catch (Exception ex) {
            Abundance.Log.LogError($"Error in patch: {ex}");
        }
    }
}