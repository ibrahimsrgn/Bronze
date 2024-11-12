using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public static ItemPool instance { get; private set; }
    public item[] items;
    

    [System.Serializable]
    public struct item
    {
        public int id;
        public string name;
        public string description;
        public ItemSO itemSO;
    }
    private void Start()
    {
        instance = this;
    }
}
