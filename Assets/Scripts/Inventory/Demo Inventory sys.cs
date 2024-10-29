using UnityEngine;

public class DemoInventorysys : MonoBehaviour
{
    public InventoryManager InventoryManager;
    public ItemSO[] ItemSOs;

    public void SpawnItem(int id)
    {

        InventoryManager.AddItem(ItemSOs[id]);
    }
}
