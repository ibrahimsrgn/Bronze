using System.Net;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public Transform LootParent;

    public int selectedSlot = -1;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetSelectedItem();
        }
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (newValue != selectedSlot)
        {
            inventorySlots[newValue].Selected();
            if (selectedSlot >= 0)
            {
                inventorySlots[selectedSlot].DeSelected();
            }
            selectedSlot = newValue;
        }

    }
    public bool AddItem(ItemSO item, out GameObject gameObject)
    {
        //Searching for same stackable item
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < itemInSlot.item.maxStackableCount && itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                gameObject = null;
                return true;
            }
        }
        //Searching for empty slot
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                gameObject = SpawnNewItem(item, slot);
                // gameObject = item.objPrefab;

                return true;
            }
        }
        gameObject = null;
        return false;
    }
    public GameObject SpawnNewItem(ItemSO item, InventorySlot slot)
    {

        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
        return newItem;
    }
    public ItemSO GetSelectedItem()
    {
        if (selectedSlot == -1) return null;
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            ItemSO item = itemInSlot.item;
            if (item.usable == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.prefab);
                    Destroy(itemInSlot.gameObject);
                    DeSelectAllSlots();
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }
    public int GetSelectedItemCount()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        return itemInSlot.count;
    }
    public ItemSO DropSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            ItemSO item = itemInSlot.item;
            itemInSlot.count--;
            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject);
                InventoryManager.instance.DeSelectAllSlots();
            }
            else
            {
                itemInSlot.RefreshCount();
            }
            return item;
        }
        return null;
    }
    public void DeSelectAllSlots()
    {
        inventorySlots[selectedSlot].DeSelectWithOutSettingInActive();
        selectedSlot = -1;
    }

}

