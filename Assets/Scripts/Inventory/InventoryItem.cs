using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public ItemSO item;
    [Header("UI")]
    public Image image;

    [HideInInspector] public Transform parentAfterDrag;
 
    public void InitialiseItem(ItemSO newItem)
    {
        item= newItem;
        image.sprite=newItem.image;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        
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
}
