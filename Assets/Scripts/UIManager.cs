using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject Inventory;


    //Remove later
    private bool active=false;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            active = !active;
            Inventory.SetActive(active);
            if (!active)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState= CursorLockMode.Confined;
            }
        }
    }
    public void HideInventory()
    {
        active=false;
    Inventory.SetActive(active);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
