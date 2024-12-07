using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class WeaponSpawner : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Fire in the hole");
        int randomWeapon = Random.Range(12, 17);
        InventoryManager.instance.AddItem(ItemPool.instance.items[randomWeapon].itemSO,out GameObject addedWeapon);
            addedWeapon.GetComponent<InventoryItem>().item.objPrefab.transform.SetParent(PlayerData.Instance.WeaponLoc.transform);
            addedWeapon.GetComponent<InventoryItem>().item.objPrefab = addedWeapon;
            Debug.Log("Fire in the Air");
        }
    }
}
