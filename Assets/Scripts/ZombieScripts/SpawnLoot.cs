using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    [SerializeField] private GameObject lootBox;
    public GameObject SpawnLootBox()
    {
        GameObject loot= Instantiate(lootBox,InventoryManager.instance.LootParent);
        UIManager.instance.UIListManager(loot);
        return loot;
    }
}
