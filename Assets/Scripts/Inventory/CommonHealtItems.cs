using UnityEngine;

public class CommonHealtItems : MonoBehaviour, IConsumableEffect
{
    [SerializeField] private int healthAmount;
    public void ConsumeItem()
    {
        Debug.Log("1");
        PlayerData.Instance.healthManager.GetHeal(healthAmount);
    }
}
