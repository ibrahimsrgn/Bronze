using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject Inventory;

    [Header("Health")]
    [SerializeField] private Image healthMain;
    [SerializeField] private Image healthFollower;
    public float maxHealth=100;


    //Remove later
    private bool active = false;
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
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            UpdateHealth(Random.Range(0, 100));
        }
    }
    public void HideInventory()
    {
        active = false;
        Inventory.SetActive(active);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateHealth(float healthAmaount)
    {
        healthMain.fillAmount =healthAmaount/maxHealth;
        healthFollower.fillAmount = Mathf.Lerp(healthFollower.fillAmount, healthMain.fillAmount, 0.1f);
    }
}
