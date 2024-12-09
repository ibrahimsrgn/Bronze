using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;
    public GameObject prefab;
    public string description;
    [SerializeField] private Canvas canvas;

    public ItemSO item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;


    public void InitialiseItem(ItemSO newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        prefab = newItem.objPrefab;
        description = newItem.description;
        RefreshCount();
    }
    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool setActiveText = count > 1;
        countText.gameObject.SetActive(setActiveText);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(GameObject.Find("Canvas").transform);

    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
    public void OnPointerDown(PointerEventData eventData)
    {

       /* InventorySlot selectedSlot = GetComponentInParent<InventorySlot>();
        selectedSlot.Selected();
        int slotIndex = -1;
        for (int i = 0; i < InventoryManager.instance.inventorySlots.Length; i++)
        {
            if (InventoryManager.instance.inventorySlots[i] == selectedSlot)
            {
                slotIndex = i;
                break;
            }
        }

        if (slotIndex != -1)
        {
            InventoryManager.instance.ChangeSelectedSlot(slotIndex);
        }*/
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //sol click ile yapılacaklar
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            InventorySlot selectedSlot = GetComponentInParent<InventorySlot>();
            int slotIndex = -1;
            for (int i = 0; i < InventoryManager.instance.inventorySlots.Length; i++)
            {
                if (InventoryManager.instance.inventorySlots[i] == selectedSlot)
                {
                    slotIndex = i;
                    break;
                }
            }

            if (slotIndex != -1)
            {
                InventoryManager.instance.ChangeSelectedSlot(slotIndex);
                selectedSlot.Selected();
            }
        }
        //sağ click ile yapılacaklar
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            SetButtonsListeners();
            
        }
    }
    private void SetButtonsListeners()
    {
        InventoryManager.instance.rightClickMenu.gameObject.SetActive(true);
        RightClickMenu rightClickMenu = InventoryManager.instance.rightClickMenu.GetComponent<RightClickMenu>();
        rightClickMenu.inspect.onClick.RemoveAllListeners();
        rightClickMenu.inspect.onClick.AddListener(() =>
        {
            InventoryManager.instance.rightClickMenu.SetActive(false);
            InventoryManager.instance.inspectMenu.SetActive(true);
            SetInspectMenuVariables();
        });




        rightClickMenu.use.onClick.RemoveAllListeners();
        rightClickMenu.use.onClick.AddListener(() =>
        {
            //Hatalı
            InventoryManager.instance.UseSelectedItem(GetComponentInParent<InventorySlot>());
        });
        rightClickMenu.drop.onClick.RemoveAllListeners();
        rightClickMenu.drop.onClick.AddListener(() =>
        {
            //Hatalı
            InventoryManager.instance.DropSelectedItem(GetComponentInParent<InventorySlot>());
        });
    }
    public void SetInspectMenuVariables()
    {
        InspectMenu inspectMenu = InventoryManager.instance.inspectMenu.GetComponent<InspectMenu>();
        inspectMenu.itemDescription.text = description;
        inspectMenu.itemImg.sprite = image.sprite;
    }
}
