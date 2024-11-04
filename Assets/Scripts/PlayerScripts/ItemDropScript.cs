using Unity.VisualScripting;
using UnityEngine;

public class ItemDropScript : MonoBehaviour
{
    PlayerData playerData;
    private void Start()
    {
        playerData = GetComponent<PlayerData>();
    }
    public void DropItem(bool Value)
    {
        if (Value && playerData.ItemOnHand != null)
        {
            playerData.ItemOnHand.transform.SetParent(null);
            playerData.ItemOnHand.transform.AddComponent<Rigidbody>();
            playerData.ItemOnHand.transform.GetComponent<Rigidbody>().AddForce(Vector3.forward * 25);
            playerData.ItemOnHand = null;
        }
    }
}
