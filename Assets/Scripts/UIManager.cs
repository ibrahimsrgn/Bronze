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
            if (Cursor.lockState == CursorLockMode.Confined)
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

    Inventory.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
