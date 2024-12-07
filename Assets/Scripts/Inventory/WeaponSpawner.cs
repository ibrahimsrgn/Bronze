using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private int random1;
[SerializeField] private int random2;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            int randomWeapon = Random.Range(random1, random2);
            GameObject weapon;
            InventoryManager.instance.AddItem(ItemPool.instance.items[randomWeapon].itemSO, out GameObject addedWeapon);
            weapon = Instantiate(addedWeapon.GetComponent<InventoryItem>().prefab);
            weapon.transform.SetParent(PlayerData.Instance.WeaponLoc.transform);
            weapon.SetActive(false);
            addedWeapon.GetComponent<InventoryItem>().prefab = weapon;
        }
    }
}
