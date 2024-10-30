using UnityEngine;

public class DemoInventorysys : MonoBehaviour
{
    public InventoryManager InventoryManager;
    public ItemSO[] ItemSOs;

    public void SpawnItem(int id)
    {

        bool result = InventoryManager.AddItem(ItemSOs[id]);
        if (result)
        {
            Debug.Log("item added");

        }
        else
        {
            Debug.Log("item not added");
        }
    }
}
