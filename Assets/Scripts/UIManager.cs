using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public CinemachinePanTilt cinemachinePanTilt;

    public static UIManager instance { get; private set; }

    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject EscMenu;
    [Header("Health")]
    [SerializeField] private Image healthMain;
    [SerializeField] private Image healthFollower;
    [SerializeField] private Image healthFrame;
    [SerializeField] private CanvasGroup healthUIGroup;
    [SerializeField] private CanvasGroup toolBoxUIGroup;
    [SerializeField] private CanvasGroup ammoUIGroup;
    [SerializeField] private float fadeOutTimer;
    [SerializeField] private float fadeDur;
    private float healthLerpValue = 0;
    private Coroutine hideHealthCoroutine;
    private Coroutine hideToolBoxCoroutine;
    private Coroutine hideAmmoCoroutine;
    private bool isTimeStopped = false;

    [Header("UIListManager")]
    [SerializeField] public List<GameObject> UIList;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        hideHealthCoroutine = StartCoroutine(HideCanvasGroup(healthUIGroup, fadeOutTimer));
        hideToolBoxCoroutine = StartCoroutine(HideCanvasGroup(toolBoxUIGroup, fadeOutTimer));
        hideAmmoCoroutine = StartCoroutine(HideCanvasGroup(ammoUIGroup, fadeOutTimer));
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

        if (hideHealthCoroutine != null)
        {
            StopCoroutine(hideHealthCoroutine);
        }
        hideHealthCoroutine = StartCoroutine(HideCanvasGroup(healthUIGroup, fadeOutTimer));
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
    public void ShowHealtUI()
    {
        healthUIGroup.alpha = 1;
    }
    public void ShowAmmoUI()
    {
        ammoUIGroup.alpha = 1;
    }
    public void ShowToolBoxUI()
    {
        toolBoxUIGroup.alpha = 1;
    }
    /// <summary>
    /// Healt UI ID 0
    /// Ammo UI ID 1
    /// ToolBox UI ID 2
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="timer"></param>
    /// <param name="canvasID"></param>
    public void HideCanvasTimer(int canvasID)
    {
        if (canvasID == 0)
        {
            if (hideHealthCoroutine != null)
            {
                StopCoroutine(hideHealthCoroutine);
            }
            hideHealthCoroutine = StartCoroutine(HideCanvasGroup(healthUIGroup, fadeOutTimer));
        }
        else if (canvasID == 1)
        {
            if (hideAmmoCoroutine != null)
            {
                StopCoroutine(hideAmmoCoroutine);
            }
            hideAmmoCoroutine = StartCoroutine(HideCanvasGroup(ammoUIGroup, fadeOutTimer));
        }
        else if (canvasID == 2)
        {
            if (hideToolBoxCoroutine != null)
            {
                StopCoroutine(hideToolBoxCoroutine);
            }
            hideToolBoxCoroutine = StartCoroutine(HideCanvasGroup(toolBoxUIGroup, fadeOutTimer));

        }
        
    }
    private IEnumerator HideCanvasGroup(CanvasGroup canvasGroup, float timer)
    {
        yield return new WaitForSeconds(timer);
        float elapsedTime = 0;
        while (elapsedTime < fadeDur)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDur);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
    public void UIListManager(GameObject CurrentUI)
    {
        //To Do
        //Lootbox açýlýnca envanterde açýlsýn 
        //Envanter açýkken tab a basýlýrsa herþey kapansýn
        CurrentUI.SetActive(!CurrentUI.gameObject.activeInHierarchy);
        if (CurrentUI.gameObject.activeInHierarchy)
        {
            cinemachinePanTilt.enabled = false;
            UIList.Add(CurrentUI);
        }
        else
        {
            if (UIList.Count <= 1)
                cinemachinePanTilt.enabled = true;
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
        if (UIList.Count <= 1)
            cinemachinePanTilt.enabled = true;
    }
    public void ShowEscMenu()
    {
        isTimeStopped = !isTimeStopped;
        if(isTimeStopped)
        {
            Time.timeScale = 0;
            UIListManager(EscMenu);
        }
        else
        {
            Time.timeScale = 1;
            EscMenu.SetActive(false);
        }
    }
}
