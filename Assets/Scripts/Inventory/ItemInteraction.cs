using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    private bool GUIActivater;
    [SerializeField] private int id;
    public void OnRayHit()
    {
        GUIActivater = true;
        if (Input.GetKeyDown(KeyCode.E))
        {
            InventoryManager.instance.AddItem(ItemPool.instance.items[id].itemSO, out GameObject addedItem);
            addedItem.GetComponent<InventoryItem>().prefab = gameObject;
            Debug.Log(addedItem);

            Destroy(gameObject.GetComponent<Rigidbody>());
            transform.SetParent(PlayerData.Instance.WeaponLoc.transform);
            gameObject.SetActive(false);
        }
    }
    void OnGUI()
    {
        if (GUIActivater)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 60), "Press 'E' to collect gun");
        }
    }
    private void OnMouseExit()
    {
        GUIActivater = false;
    }
}
