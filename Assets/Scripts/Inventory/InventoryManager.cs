using System.Net;
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetSelectedItem();
        }
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].DeSelected();
           
        }
        inventorySlots[newValue].Selected();


        selectedSlot = newValue;
    }
    public bool AddItem(ItemSO item,out GameObject gameObject)
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
                gameObject=SpawnNewItem(item, slot);
                
                return true;
            }
        }
        gameObject=null;
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
                    Destroy(itemInSlot.gameObject);
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
}
