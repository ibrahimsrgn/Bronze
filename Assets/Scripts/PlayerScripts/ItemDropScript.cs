using Unity.VisualScripting;
using UnityEngine;

public class ItemDropScript : MonoBehaviour
{
    [SerializeField] private int angle;
    [SerializeField] private int angleZ;


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
            playerData.ItemOnHand.transform.GetComponent<Rigidbody>().AddForce(transform.forward * angleZ + Vector3.up * angle);
            playerData.ItemOnHand = null;
        }
    }
}
