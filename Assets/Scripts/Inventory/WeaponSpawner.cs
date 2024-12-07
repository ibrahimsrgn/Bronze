using Unity.VisualScripting;
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
            GameObject weapon;
        InventoryManager.instance.AddItem(ItemPool.instance.items[randomWeapon].itemSO,out GameObject addedWeapon);
           weapon= Instantiate(addedWeapon.GetComponent<InventoryItem>().prefab);
            weapon.transform.SetParent(PlayerData.Instance.WeaponLoc.transform);
            weapon.SetActive(false);
            addedWeapon.GetComponent<InventoryItem>().prefab = weapon;
            Debug.Log("Fire in the Air");
        }
    }
}
