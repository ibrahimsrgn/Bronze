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
            bool itemAdded = InventoryManager.instance.AddItem(ItemPool.instance.items[id].itemSO, out GameObject addedItem);
            if (addedItem != null)
            {
                addedItem.GetComponent<InventoryItem>().prefab = gameObject;
                Destroy(gameObject.GetComponent<Rigidbody>());
                transform.SetParent(PlayerData.Instance.WeaponLoc.transform);
                gameObject.SetActive(false);
            }
            if (itemAdded&&addedItem==null)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Item could not added");
            }
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
