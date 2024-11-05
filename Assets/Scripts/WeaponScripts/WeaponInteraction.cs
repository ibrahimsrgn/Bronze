using UnityEngine;

public class WeaponInteraction : MonoBehaviour
{
    //public PlayerData playerData;
    private bool GUIActivater;
    private Rigidbody rb;
   /* public void OnRayHit(PlayerData playerData)
    {
        if (playerData != null)
        {
            rb = GetComponent<Rigidbody>();
            GUIActivater = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(rb);
                transform.position = playerData.WeaponLoc.transform.position;
                transform.rotation = playerData.WeaponLoc.transform.rotation;
                transform.SetParent(playerData.WeaponLoc.transform);
                playerData.ItemOnHand = transform;
            }
        }
    }*/

    void OnGUI()
    {
        if (GUIActivater)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 60), "Press 'E' to collect gun");
        }
    }
    private void OnMouseEnter()
    {
        GUIActivater = true;
    }
    private void OnMouseExit()
    {
        GUIActivater = false;
    }
}
