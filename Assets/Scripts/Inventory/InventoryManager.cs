﻿using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public Transform LootParent;
    public GameObject inspectMenu;
    public GameObject rightClickMenu;
    [SerializeField] private TextMeshProUGUI currentAmmoCountText;
    [SerializeField] private TextMeshProUGUI totalAmmoCountText;
    public int totalAmmoCount = 0;
    private List<InventoryItem> ammoItems;

    public int selectedSlot = -1;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9 && PlayerData.Instance.canChangeWeapon)
            {
                ChangeSelectedSlot(number - 1);
                UIManager.instance.ShowToolBoxUI();
                UIManager.instance.HideCanvasTimer(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            InventorySlot slot = inventorySlots[selectedSlot];
            slot.GetComponentInChildren<InventoryItem>().UseItem();
        }
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (newValue != selectedSlot)
        {
            inventorySlots[newValue].Selected();
            if (selectedSlot >= 0)
            {
                inventorySlots[selectedSlot].DeSelected();
            }
            selectedSlot = newValue;
        }

    }
    public bool AddItem(ItemSO item, out GameObject gameObject)
    {
        //Searching for same stackable item
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < itemInSlot.item.maxStackableCount && itemInSlot.item.stackable)
            {
                //itemInSlot.count++;
                itemInSlot.count = item.maxStackableCount;
                itemInSlot.RefreshCount();
                gameObject = null;
                return true;
            }
        }
        //Searching for empty slot
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                gameObject = SpawnNewItem(item, slot);
                return true;
            }
        }
        gameObject = null;
        return false;
    }
    public InventorySlot GetSelectedSlot()
    {
        if (selectedSlot == -1) return null;
        return inventorySlots[selectedSlot];
    }
    public GameObject SpawnNewItem(ItemSO item, InventorySlot slot)
    {

        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
        if (inventoryItem.item.type == ItemType.Ammunation)
        {
            inventoryItem.count = item.maxStackableCount;
            inventoryItem.RefreshCount();
        }
        return newItem;
    }
    public ItemSO UseSelectedItem()
    {
        if (selectedSlot == -1) return null;
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            ItemSO item = itemInSlot.item;
            if (item.usable == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.itemPrefab);
                    Destroy(itemInSlot.gameObject);
                    DeSelectAllSlots();
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }
    public ItemSO UseSelectedItem(InventorySlot slot)
    {
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            ItemSO item = itemInSlot.item;
            if (item.usable == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.itemPrefab);
                    Destroy(itemInSlot.gameObject);
                    DeSelectAllSlots();
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }
    public int GetSelectedItemCount()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        return itemInSlot.count;
    }
    public ItemSO DropSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            ItemSO item = itemInSlot.item;
            itemInSlot.count--;
            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject);
                InventoryManager.instance.DeSelectAllSlots();
            }
            else
            {
                itemInSlot.RefreshCount();
            }
            return item;
        }
        return null;
    }
    public ItemSO DropSelectedItem(InventorySlot slot)
    {
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            ItemSO item = itemInSlot.item;
            itemInSlot.count--;
            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject);
                InventoryManager.instance.DeSelectAllSlots();
            }
            else
            {
                itemInSlot.RefreshCount();
            }
            return item;
        }
        return null;
    }
    public void DeSelectAllSlots()
    {
        if (selectedSlot == -1) return;
        inventorySlots[selectedSlot].DeSelectWithOutSettingInActive();
        selectedSlot = -1;
    }
    public void CountAmmo(int ammoId)
    {
        totalAmmoCount = 0;
        ammoItems = new List<InventoryItem>();
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.GetComponentInChildren<InventoryItem>() != null)
            {
                InventoryItem ammoInventoryItem = slot.GetComponentInChildren<InventoryItem>();
                if (ammoId == ammoInventoryItem.id)
                {
                    totalAmmoCount += ammoInventoryItem.count;
                    RefreshMaxAmmoUI();
                    ammoItems.Add(ammoInventoryItem);
                }
            }
        }
    }
    public int ReloadMagazine(int magazineCap, int requiredAmmo)
    {
        if (requiredAmmo <= totalAmmoCount)
        {
            totalAmmoCount -= requiredAmmo;
            RefreshMaxAmmoUI();
            ReloadCalculator(requiredAmmo, magazineCap);
            return magazineCap;
        }
        else if (requiredAmmo > totalAmmoCount)
        {
            int returnAmmoCount = totalAmmoCount;
            totalAmmoCount = 0;
            RefreshMaxAmmoUI();

            return (magazineCap - requiredAmmo) + ReloadCalculator(returnAmmoCount, magazineCap);
        }
        else { return 0; }
    }
    public int ReloadCalculator(int requiredAmmo, int magazineCap)
    {
        if (ammoItems == null || ammoItems.Count == 0)
        {
            return 0;
        }

        int remainingAmmoToLoad = requiredAmmo;

        // Baştan sona en son eklenene kadar işle
        for (int i = ammoItems.Count - 1; i >= 0 && remainingAmmoToLoad > 0; i--)
        {
            InventoryItem ammoItem = ammoItems[i];

            if (ammoItem.count >= remainingAmmoToLoad)
            {
                ammoItem.count -= remainingAmmoToLoad;
                remainingAmmoToLoad = 0;

                if (ammoItem.count == 0)
                {
                    // Eğer öğe biterse çantadan çıkar
                    ammoItems.RemoveAt(i);
                    Destroy(ammoItem.gameObject);
                }
                ammoItem.RefreshCount();
                return requiredAmmo - remainingAmmoToLoad;
            }
            else
            {
                // Yetersizse tüm mermiyi kullan ve sonraki öğeye geç
                remainingAmmoToLoad -= ammoItem.count;
                ammoItems.RemoveAt(i);
                Destroy(ammoItem.gameObject);
            }
        }

        // Mermi sayısını UI ile güncelle
        //CountAmmo(ammoItems.Count > 0 ? ammoItems[0].id : -1);
        RefreshMaxAmmoUI();
        if (remainingAmmoToLoad > 0)
        {
            return requiredAmmo - remainingAmmoToLoad;
        }
        return 0;
    }
    public void RefreshMaxAmmoUI()
    {
        UIManager.instance.ShowAmmoUI();
        UIManager.instance.HideCanvasTimer(1);
        totalAmmoCountText.text = "/" + totalAmmoCount.ToString();
    }
    public void RefreshCurrentAmmoUI(int currentAmmo)
    {
        UIManager.instance.ShowAmmoUI();
        UIManager.instance.HideCanvasTimer(1);
        currentAmmoCountText.text = currentAmmo.ToString();
    }
}

