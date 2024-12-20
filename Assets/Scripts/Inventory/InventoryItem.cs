using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
{
    [Header("UI")]
    public int id=0;
    public Image image;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI displayNameTxt;
    public GameObject itemPrefab;
    public string displayName;
    public string description;
    [SerializeField] private Canvas canvas;

    public ItemSO item;
    public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;


    public void InitialiseItem(ItemSO newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        itemPrefab = newItem.objPrefab;
        description = newItem.description;
        displayName=newItem.displayName;
        displayNameTxt.text = displayName;
        if (itemPrefab.TryGetComponent<ItemInteraction>(out ItemInteraction itemInteraction))
        {
            id = itemInteraction.id;
        }
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
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryManager.instance.rightClickMenu.transform.position = Input.mousePosition;
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
        if (item.type == ItemType.Weapon)
        {
            rightClickMenu.use.interactable = false;
        }
        else
        {
            rightClickMenu.use.interactable = true;
        }
        rightClickMenu.use.onClick.RemoveAllListeners();
        rightClickMenu.use.onClick.AddListener(UseItem);
        rightClickMenu.drop.onClick.RemoveAllListeners();
        rightClickMenu.drop.onClick.AddListener(DropItem);
    }
    public void UseItem()
    {
        itemPrefab.GetComponent<IConsumableEffect>()?.ConsumeItem();
        InventoryManager.instance.UseSelectedItem(GetComponentInParent<InventorySlot>());
    }
    public void DropItem()
    {
        InventoryManager.instance.rightClickMenu.SetActive(false);
        if (itemPrefab == null) return;
        InventoryManager.instance.DropSelectedItem(GetComponentInParent<InventorySlot>());
        if (count >= 1)
        {
            Debug.Log("1");
            GameObject droppedItem = Instantiate(itemPrefab, PlayerData.Instance.ItemOnHand);
            droppedItem.SetActive(true);
            droppedItem.transform.SetParent(null);
            droppedItem.gameObject.AddComponent<Rigidbody>();
            droppedItem.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 150 + Vector3.up * 250);
            return;
        }
        else if (itemPrefab == PlayerData.Instance.ItemOnHand)
        {
            Debug.Log("2");
            PlayerData.Instance.LeftHandLayer.data.target = null;
            PlayerData.Instance.RightHandLayer.data.target = null;
            PlayerData.Instance.ItemOnHand.transform.SetParent(null);
            PlayerData.Instance.ItemOnHand.gameObject.AddComponent<Rigidbody>();
            PlayerData.Instance.ItemOnHand.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 150 + Vector3.up * 250);
            PlayerData.Instance.ItemOnHand = null;
            return;
        }
        else
        {
            Debug.Log("3");
            itemPrefab.SetActive(true);
            itemPrefab.transform.SetParent(null);
            itemPrefab.gameObject.AddComponent<Rigidbody>();
            itemPrefab.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 150 + Vector3.up * 250);
            itemPrefab = null;
            return;
        }
    }
    public void SetInspectMenuVariables()
    {
        InspectMenu inspectMenu = InventoryManager.instance.inspectMenu.GetComponent<InspectMenu>();
        inspectMenu.itemDescription.text = description;
        inspectMenu.itemImg.sprite = image.sprite;
    }
}
