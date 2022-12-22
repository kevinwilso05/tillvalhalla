

// ValheimPlus.InventoryAssistant
using System.Collections.Generic;
using System.Linq;
using TillValhalla.Configurations;
using TillValhalla.Configurations.Sections;
using UnityEngine;

internal static class InventoryAssistant
{
    public static List<Container> GetNearbyChests(GameObject target, float range, bool checkWard = true)
    {
        string[] layerNames = new string[1] { "piece" };
        if (CraftingStationConfiguration.craftFromCarts.Value || CraftingStationConfiguration.craftFromShips.Value)
        {
            layerNames = new string[3] { "piece", "item", "vehicle" };
        }
        Collider[] source = Physics.OverlapSphere(target.transform.position, range, LayerMask.GetMask(layerNames));
        IOrderedEnumerable<Collider> orderedEnumerable = source.OrderBy((Collider x) => Vector3.Distance(x.gameObject.transform.position, target.transform.position));
        List<Container> list = new List<Container>();
        foreach (Collider item in orderedEnumerable)
        {
            try
            {
                Container componentInParent = item.GetComponentInParent<Container>();
                bool flag = componentInParent.CheckAccess(Player.m_localPlayer.GetPlayerID());
                if (checkWard)
                {
                    flag = flag && PrivateArea.CheckAccess(item.gameObject.transform.position, 0f, flash: false, wardCheck: true);
                }
                Piece componentInParent2 = componentInParent.GetComponentInParent<Piece>();
                bool flag2 = componentInParent.GetComponentInParent<Vagon>() != null;
                bool flag3 = componentInParent.GetComponentInParent<Ship>() != null;
                if (componentInParent2 != null && flag && componentInParent.GetInventory() != null && (!flag2 || CraftingStationConfiguration.craftFromCarts.Value) && (!flag3 || CraftingStationConfiguration.craftFromShips.Value) && (componentInParent2.IsPlacedByPlayer() || (flag3 && CraftingStationConfiguration.craftFromShips.Value)))
                {
                    list.Add(componentInParent);
                }
            }
            catch
            {
            }
        }
        return list;
    }

    public static List<Container> GetNearbyChestsWithItem(GameObject target, float range, ItemDrop.ItemData itemInfo, bool checkWard = true)
    {
        List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);
        List<Container> list = new List<Container>();
        foreach (Container item in nearbyChests)
        {
            if (ChestContainsItem(item, itemInfo))
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static bool ChestContainsItem(Container chest, ItemDrop.ItemData needle)
    {
        List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
        foreach (ItemDrop.ItemData item in allItems)
        {
            if (item.m_shared.m_name == needle.m_shared.m_name)
            {
                return true;
            }
        }
        return false;
    }

    public static bool ChestContainsItem(Container chest, string needle)
    {
        List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
        foreach (ItemDrop.ItemData item in allItems)
        {
            if (item.m_shared.m_name == needle)
            {
                return true;
            }
        }
        return false;
    }

    public static List<ItemDrop.ItemData> GetNearbyChestItems(GameObject target, float range = 10f, bool checkWard = true)
    {
        List<ItemDrop.ItemData> list = new List<ItemDrop.ItemData>();
        List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);
        foreach (Container item in nearbyChests)
        {
            List<ItemDrop.ItemData> allItems = item.GetInventory().GetAllItems();
            foreach (ItemDrop.ItemData item2 in allItems)
            {
                list.Add(item2);
            }
        }
        return list;
    }

    public static List<ItemDrop.ItemData> GetNearbyChestItemsByContainerList(List<Container> nearbyChests)
    {
        List<ItemDrop.ItemData> list = new List<ItemDrop.ItemData>();
        foreach (Container nearbyChest in nearbyChests)
        {
            List<ItemDrop.ItemData> allItems = nearbyChest.GetInventory().GetAllItems();
            foreach (ItemDrop.ItemData item in allItems)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static int GetItemAmountInItemList(List<ItemDrop.ItemData> itemList, ItemDrop.ItemData needle)
    {
        int num = 0;
        foreach (ItemDrop.ItemData item in itemList)
        {
            if (item.m_shared.m_name == needle.m_shared.m_name)
            {
                num += item.m_stack;
            }
        }
        return num;
    }

    public static int GetItemAmountInItemList(List<ItemDrop.ItemData> itemList, string needle)
    {
        int num = 0;
        foreach (ItemDrop.ItemData item in itemList)
        {
            if (item.m_shared.m_name == needle)
            {
                num += item.m_stack;
            }
        }
        return num;
    }

    public static int RemoveItemInAmountFromAllNearbyChests(GameObject target, float range, ItemDrop.ItemData needle, int amount, bool checkWard = true)
    {
        List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);
        List<ItemDrop.ItemData> nearbyChestItemsByContainerList = GetNearbyChestItemsByContainerList(nearbyChests);
        int itemAmountInItemList = GetItemAmountInItemList(nearbyChestItemsByContainerList, needle);
        if (amount == 0)
        {
            return 0;
        }
        int num = 0;
        foreach (Container item in nearbyChests)
        {
            if (num != amount)
            {
                int num2 = RemoveItemFromChest(item, needle, amount);
                num += num2;
                amount -= num2;
            }
        }
        return num;
    }

    public static int RemoveItemInAmountFromAllNearbyChests(GameObject target, float range, string needle, int amount, bool checkWard = true)
    {
        List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);
        List<ItemDrop.ItemData> nearbyChestItemsByContainerList = GetNearbyChestItemsByContainerList(nearbyChests);
        int itemAmountInItemList = GetItemAmountInItemList(nearbyChestItemsByContainerList, needle);
        if (amount == 0)
        {
            return 0;
        }
        int num = 0;
        foreach (Container item in nearbyChests)
        {
            if (num != amount)
            {
                int num2 = RemoveItemFromChest(item, needle, amount);
                num += num2;
                amount -= num2;
            }
        }
        return num;
    }

    public static int RemoveItemFromChest(Container chest, ItemDrop.ItemData needle, int amount = 1)
    {
        if (!ChestContainsItem(chest, needle))
        {
            return 0;
        }
        int num = 0;
        List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
        foreach (ItemDrop.ItemData item in allItems)
        {
            if (item.m_shared.m_name == needle.m_shared.m_name)
            {
                int num2 = Mathf.Min(item.m_stack, amount);
                item.m_stack -= num2;
                amount -= num2;
                num += num2;
                if (amount <= 0)
                {
                    break;
                }
            }
        }
        if (num == 0)
        {
            return 0;
        }
        allItems.RemoveAll((ItemDrop.ItemData x) => x.m_stack <= 0);
        chest.m_inventory.m_inventory = allItems;
        ConveyContainerToNetwork(chest);
        return num;
    }

    public static int RemoveItemFromChest(Container chest, string needle, int amount = 1)
    {
        if (!ChestContainsItem(chest, needle))
        {
            return 0;
        }
        int num = 0;
        List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
        foreach (ItemDrop.ItemData item in allItems)
        {
            if (item.m_shared.m_name == needle)
            {
                int num2 = Mathf.Min(item.m_stack, amount);
                item.m_stack -= num2;
                amount -= num2;
                num += num2;
                if (amount <= 0)
                {
                    break;
                }
            }
        }
        if (num == 0)
        {
            return 0;
        }
        allItems.RemoveAll((ItemDrop.ItemData x) => x.m_stack <= 0);
        chest.m_inventory.m_inventory = allItems;
        ConveyContainerToNetwork(chest);
        return num;
    }

    public static void ConveyContainerToNetwork(Container c)
    {
        c.Save();
        c.GetInventory().Changed();
    }
}
