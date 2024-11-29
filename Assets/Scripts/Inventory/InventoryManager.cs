using System.Net;
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
            if (isNumber && number > 0 && number < 9)
            {
                ChangeSelectedSlot(number - 1);
                EquipToHand();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetSelectedItem();
        }
    }
    public void EquipToHand()
    {

    }
    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].DeSelected();
        }
        inventorySlots[newValue].Selected();
        selectedSlot = newValue;
    }
    public bool AddItem(ItemSO item)
    {
        //Searching for same stackable item
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < itemInSlot.item.maxStackableCount && itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                //Eğer silah ise instantiate edilip ele alıancak
               /* if (itemInSlot.item.type == ItemType.Weapon)
                {
                  GameObject weapon=  Instantiate(itemInSlot.item.objPrefab);
                   weapon.transform.SetParent(playerData.WeaponLoc.transform);
                    playerData.ItemOnHand = transform;

                    playerData.LeftHandLayer.data.target = LeftHandRigRef;
                    playerData.RightHandLayer.data.target = RightHandRigRef;

                    transform.position = playerData.WeaponLoc.transform.position;
                    transform.rotation = playerData.WeaponLoc.transform.rotation;

                    playerData.WeaponPosRot.position = WeaponLocRef.position;

                    playerData.CamPosRef2 = AimCamLocRef;
                    Animator.enabled = true;
                    playerData._RigBuilder.Build();
                }*/
                return true;
            }
        }
        //Searching for empty slot
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }
    public void SpawnNewItem(ItemSO item, InventorySlot slot)
    {

        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
    public ItemSO GetSelectedItem()
    {
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
                    Destroy(itemInSlot.gameObject);
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
}
