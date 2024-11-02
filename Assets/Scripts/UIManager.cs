using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance {  get; private set; }

    [SerializeField] private GameObject Inventory;

    [Header("Health")]
    [SerializeField] private Image healthMain;
    [SerializeField] private Image healthFollower;
    float healthLerpValue = 0;

    //Remove later
    private bool active = false;
    private void Awake()
    {
        instance = this;
    }
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
        LazyHealthBar();
    }
    public void HideInventory()
    {
        active = false;
        Inventory.SetActive(active);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateHealth(float healthBarFillAmaount)
    {
        Debug.Log(healthBarFillAmaount);
        healthMain.fillAmount =healthBarFillAmaount;
        healthLerpValue = 0;
    }
    /// <summary>
    /// Checks if health amount changes and makes smooth hp filling
    /// </summary>
    private void LazyHealthBar()
    {
        if (healthFollower.fillAmount != healthMain.fillAmount)
        {
            healthFollower.fillAmount = Mathf.Lerp(healthFollower.fillAmount, healthMain.fillAmount, healthLerpValue);
            healthLerpValue += 1 * Time.deltaTime;
        }
    }
}
