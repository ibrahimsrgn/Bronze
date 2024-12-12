using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemSO : ScriptableObject{

    

    [Header("Only gameplay")]
    public ItemType type;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;
    public bool usable = true;
    public int maxStackableCount;
    public string description;

    [Header("Both")]
    public GameObject objPrefab;
    public Sprite image;

}
public enum ItemType
{
    Consumable,
    Weapon,
    Ammunation,
    Misc
}
