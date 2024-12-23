using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Cinemachine;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, deSelectedColor;
    [SerializeField] private bool inToolBox;

    public void Selected()
    {
        image.color = selectedColor;

        if (inToolBox)
        {
           Invoke(nameof(EquipItem),0.1f);
        }
        if (GetComponentInChildren<InventoryItem>()==null)
        {
            PlayerData.Instance.UnEquipItem();
        }

    }
    public void DeSelected()
    {
        image.color = deSelectedColor;
        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        inventoryItem?.itemPrefab.gameObject.SetActive(false);
    }
    public void DeSelectWithOutSettingInActive()
    {
        image.color = deSelectedColor;
    }
    public void EquipItem()
    {
        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        Debug.Log(gameObject.name);
        if (inventoryItem != null)
        {
            PlayerData.Instance.ItemOnHand = inventoryItem.itemPrefab.transform;
            GunFire gunFire = inventoryItem.itemPrefab.GetComponent<GunFire>();
            //Nesne Silah değil ise
            if (gunFire == null)
            {
                inventoryItem.itemPrefab.transform.position = PlayerData.Instance.WeaponLoc.transform.position;
                inventoryItem.itemPrefab.SetActive(true);
                return;
            }
            PlayerData.Instance.LeftHandLayer.data.target = gunFire.LeftHandRigRef;
            PlayerData.Instance.RightHandLayer.data.target = gunFire.RightHandRigRef;
            inventoryItem.itemPrefab.transform.position = PlayerData.Instance.WeaponLoc.transform.position;
            inventoryItem.itemPrefab.transform.rotation = PlayerData.Instance.WeaponLoc.transform.rotation;
            PlayerData.Instance.LeftHandLayer.data.target = gunFire.LeftHandRigRef;
            PlayerData.Instance.RightHandLayer.data.target = gunFire.RightHandRigRef;
            PlayerData.Instance.CamPosRef2 = gunFire.AimCamLocRef;
            gunFire.Animator.enabled = true;
            inventoryItem.itemPrefab.SetActive(true);
            PlayerData.Instance._RigBuilder.Build();
            PlayerData.Instance.WeaponPosRot.position = Vector3.zero;
            PlayerData.Instance.WeaponPosRot.localPosition = gunFire.WeaponLocRef.localPosition;
            InventoryManager.instance.CountAmmo(gunFire.usableAmmoId);
            InventoryManager.instance.RefreshCurrentAmmoUI(gunFire.CurrentAmmoCount);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {

        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            if (InventoryManager.instance.GetSelectedSlot() != null && InventoryManager.instance.GetSelectedSlot() == this)
            {
                Invoke(nameof(EquipItem), .1f);
            }
        }
        else
        {
            InventoryItem comingItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            Debug.Log(comingItem.item.name);
            InventoryItem itemInSlot = gameObject.gameObject.GetComponentInChildren<InventoryItem>();
            Debug.Log(itemInSlot.item.name);
            if (itemInSlot != null && comingItem.item == itemInSlot.item && comingItem.count < comingItem.item.maxStackableCount && comingItem.item.stackable)
            {
                int totalCount = itemInSlot.count += comingItem.count;
                if (totalCount <= itemInSlot.item.maxStackableCount)
                {
                    itemInSlot.count = totalCount;
                    Destroy(comingItem.gameObject);
                }
                else
                {
                    itemInSlot.count = comingItem.item.maxStackableCount;
                    comingItem.count = totalCount - comingItem.item.maxStackableCount;
                }
                itemInSlot.RefreshCount();
                comingItem.RefreshCount();
            }
        }

    }
}
