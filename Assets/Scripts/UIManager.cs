using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] private GameObject Inventory;
    [Header("Health")]
    [SerializeField] private Image healthMain;
    [SerializeField] private Image healthFollower;
    [SerializeField] private Image healthFrame;
    [SerializeField] private CanvasGroup healthUIGroup;
    [SerializeField] private float fadeOutTimer;
    [SerializeField] private float fadeDur;
    private float healthLerpValue = 0;
    private Coroutine hideHealthCoroutine;

    [Header("UIListManager")]
    [SerializeField] private List<GameObject> UIList;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        hideHealthCoroutine = StartCoroutine(HideHealthUI());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UIList.Count > 0)
        {
            CloseCurrentUI();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            UIListManager(Inventory);
        }
        if (UIList.Count <= 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        LazyHealthBar();
    }

    public void UpdateHealth(float healthBarFillAmaount)
    {
        healthMain.fillAmount = healthBarFillAmaount;
        healthLerpValue = 0;
        ShowHealtUI();

        if(hideHealthCoroutine != null)
        {
            StopCoroutine(hideHealthCoroutine);
        }
        hideHealthCoroutine = StartCoroutine(HideHealthUI());
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
    private void ShowHealtUI()
    {
        healthUIGroup.alpha = 1;
    }
    private IEnumerator HideHealthUI()
    {
        yield return new WaitForSeconds(fadeOutTimer);
        float elapsedTime = 0;
        while (elapsedTime < fadeDur)
        {
            elapsedTime += Time.deltaTime;
            healthUIGroup.alpha=Mathf.Lerp(1f,0f,elapsedTime/fadeDur);
            yield return null;
        }
        healthUIGroup.alpha = 0f;
    }
    public void UIListManager(GameObject CurrentUI)
    {
        //To Do
        //Lootbox açýlýnca envanterde açýlsýn 
        //Envanter açýkken tab a basýlýrsa herþey kapansýn
        CurrentUI.SetActive(!CurrentUI.gameObject.activeInHierarchy);
        if (CurrentUI.gameObject.activeInHierarchy)
        {
            UIList.Add(CurrentUI);
        }
        else
        {
            UIList.Remove(CurrentUI);
        }
        if (!UIList.Contains(Inventory))
        {
            UIListManager(Inventory);
        }
    }
    private void CloseCurrentUI()
    {
        UIList[UIList.Count - 1].gameObject.SetActive(false);
        UIList.Remove(UIList[UIList.Count - 1]);
    }
}
