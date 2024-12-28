using Unity.VisualScripting;
using UnityEngine;

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
            if (ItemPool.instance.items[randomWeapon].itemSO == null) return;
            InventoryManager.instance.AddItem(ItemPool.instance.items[randomWeapon].itemSO, out GameObject addedWeapon);
            if(addedWeapon ==null) return;
            weapon = Instantiate(addedWeapon.GetComponent<InventoryItem>().itemPrefab);
            weapon.transform.SetParent(PlayerData.Instance.WeaponLoc.transform);
            Destroy(weapon.GetComponent<Rigidbody>());
            weapon.SetActive(false);
            addedWeapon.GetComponent<InventoryItem>().itemPrefab = weapon;
        }
    }
}
