using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] InventorySlots;
    public GameObject inventoryItemPrefab;
    public void AddItem(ItemSO item)
    {

        foreach (InventorySlot slot in InventorySlots)
        {
            Debug.Log("5");
            InventoryItem itemInSlot =slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                Debug.Log("1");
                SpawnNewItem(item,slot);
                return;
            }
        }
    }
    private void SpawnNewItem(ItemSO item, InventorySlot slot)
    {

        GameObject newItem =Instantiate(inventoryItemPrefab,slot.transform);
        InventoryItem inventoryItem=newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
