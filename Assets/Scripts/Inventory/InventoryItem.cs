using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;

    [SerializeField]private Canvas canvas;

    [HideInInspector] public ItemSO item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
 
    public void InitialiseItem(ItemSO newItem)
    {
        item= newItem;
        image.sprite=newItem.image;
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
        image.raycastTarget=true;
        transform.SetParent(parentAfterDrag);
    }
    public void OnPointerDown(PointerEventData eventData)
    {

        InventorySlot selectedSlot = GetComponentInParent<InventorySlot>();
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
        }
    }
}
