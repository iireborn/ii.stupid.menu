/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaNetworking;
using HarmonyLib;

namespace iiMenu.Patches.Menu
{
    [HarmonyPatch(typeof(CosmeticsController), nameof(CosmeticsController.PurchaseItem))]
    public class PurchasePatch
    {
        public static bool enabled;

        private static bool Prefix()
        {
            if (enabled)
            {
                CosmeticsController.CosmeticItem itemFromDict = CosmeticsController.instance.GetItemFromDict(CosmeticsController.instance.itemToBuy.itemName);
                if (itemFromDict.itemCategory == CosmeticsController.CosmeticCategory.Set)
                {
                    CosmeticsController.instance.UnlockItem(CosmeticsController.instance.itemToBuy.itemName);
                    foreach (string item in itemFromDict.bundledItems)
                        CosmeticsController.instance.UnlockItem(item);
                }
                else
                    CosmeticsController.instance.UnlockItem(CosmeticsController.instance.itemToBuy.itemName);

                CosmeticsController.instance.UpdateMyCosmetics();
                CosmeticsController.instance.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Success;

                CosmeticsController.instance.UpdateShoppingCart();
                CosmeticsController.instance.ProcessPurchaseItemState(null, CosmeticsController.instance.isLastHandTouchedLeft);

                return false;
            }

            return true;
        }
    }
}
