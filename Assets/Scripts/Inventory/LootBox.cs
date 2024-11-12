using NUnit.Framework.Internal;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    private InventorySlot[] inventorySlot;
    private void Start()
    {
        PrepareChilds();
    }
    private void PrepareChilds()
    {
        inventorySlot = new InventorySlot[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            inventorySlot[i] = transform.GetChild(i).GetComponent<InventorySlot>();
        }
        CalculateItemAmount();
    }
    private void CalculateItemAmount()
    {
        int itemCount = Random.Range(0, 5);
        for (int i = 0; i < itemCount; i++)
        {
            AddRandomItem(inventorySlot[i]);
        }
    }
    private void AddRandomItem(InventorySlot inventorySlot)
    {
        //Toplam item say�s�na g�re olacak
        //Her itemin kendine g�re �ans de�eri olmas� laz�m
        int randomitem=Random.Range(0, 2);
        InventoryManager.instance.SpawnNewItem(ItemPool.instance.items[randomitem].itemSO, inventorySlot);
    }
}
