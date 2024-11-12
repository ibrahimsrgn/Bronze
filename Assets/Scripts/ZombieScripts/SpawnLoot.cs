using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    [SerializeField] private GameObject lootBox;
    public GameObject SpawnLootBox()
    {
        GameObject loot= Instantiate(lootBox,InventoryManager.instance.LootParent);
        loot.gameObject.SetActive(false);
        return loot;
    }
}
