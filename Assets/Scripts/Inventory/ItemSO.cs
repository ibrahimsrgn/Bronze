using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemSO : ScriptableObject{

    

    [Header("Only gameplay")]
    public ItemType type;
    public ActionType action;
    public Vector2Int range = new Vector2Int(5, 4);
    [Header("Only UI")]
    public bool stackable = true;
    [Header("Both")]
    public Sprite image;

}
public enum ItemType
{
    HealingItem,
    Weapon
}
public enum ActionType
{
    Heal,
    Fire,
    Swing
}
