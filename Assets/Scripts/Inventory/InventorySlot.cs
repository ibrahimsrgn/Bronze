using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, deSelectedColor;

    public void Selected()
    {
        image.color = selectedColor;

        //Seçili slotda kuşanılabilicek item varsa kuşan
            InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        if(inventoryItem != null)
        {
            PlayerData.Instance.ItemOnHand = inventoryItem.prefab.transform;
            inventoryItem.prefab.gameObject.SetActive(true);
            GunFire gunFire= inventoryItem.prefab.GetComponent<GunFire>();
            PlayerData.Instance.LeftHandLayer.data.target = gunFire.LeftHandRigRef;
            PlayerData.Instance.RightHandLayer.data.target = gunFire.RightHandRigRef;
            inventoryItem.prefab.transform.position = PlayerData.Instance.WeaponLoc.transform.position;
            inventoryItem.prefab.transform.rotation = PlayerData.Instance.WeaponLoc.transform.rotation;
            PlayerData.Instance.WeaponPosRot.position =gunFire.WeaponLocRef.position;

            PlayerData.Instance.CamPosRef2 =gunFire.AimCamLocRef;
            gunFire.Animator.enabled = true;
            PlayerData.Instance._RigBuilder.Build();

        }
        else
        {
            PlayerData.Instance.UnEquipItem();
        }
    }
    public void DeSelected()
    {
        image.color = deSelectedColor;
        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        inventoryItem?.prefab.gameObject.SetActive(false);
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
        else
        {
            InventoryItem comingItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            Debug.Log(comingItem.item.name);
            InventoryItem itemInSlot = gameObject.gameObject.GetComponentInChildren<InventoryItem>();
            Debug.Log(itemInSlot.item.name);
            if (itemInSlot!=null&&comingItem.item == itemInSlot.item && comingItem.count < comingItem.item.maxStackableCount && comingItem.item.stackable)
            {
                int totalCount=itemInSlot.count += comingItem.count;
                if (totalCount <= itemInSlot.item.maxStackableCount)
                {
                    itemInSlot.count=totalCount;
                    Destroy(comingItem.gameObject);
                }
                else
                {
                    itemInSlot.count=comingItem.item.maxStackableCount; 
                    comingItem.count=totalCount-comingItem.item.maxStackableCount;
                }
                itemInSlot.RefreshCount();
                comingItem.RefreshCount();
            }
        }
        
    }
}
