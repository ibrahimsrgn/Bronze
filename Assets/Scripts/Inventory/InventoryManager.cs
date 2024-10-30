using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 4;
    public InventorySlot[] InventorySlots;
    public GameObject inventoryItemPrefab;

    public int selectedSlot=-1;
    private void Start()
    {
        ChangeSelectedSlot(0);
    }
    private void Update()
    {
        if(Input.inputString!=null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber&&number>0&&number<9)
            {
                ChangeSelectedSlot(number - 1);

            }
        }
    }
    private void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot>=0)
        {
            InventorySlots[selectedSlot].DeSelected();
        }
        InventorySlots[newValue].Selected();
        selectedSlot = newValue;
    }
    public bool AddItem(ItemSO item)
    {
        //Searching for same stackable item
        foreach (InventorySlot slot in InventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot !=null && itemInSlot.item==item&& itemInSlot.count< maxStackedItems&&itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        //Searching for empty slot
        foreach (InventorySlot slot in InventorySlots)
        {
            InventoryItem itemInSlot =slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item,slot);
                return true;
            }
        }
        return false;
    }
    private void SpawnNewItem(ItemSO item, InventorySlot slot)
    {

        GameObject newItem =Instantiate(inventoryItemPrefab,slot.transform);
        InventoryItem inventoryItem=newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
