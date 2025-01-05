using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITimeSelector : MonoBehaviour
{
    [SerializeField] private GameObject timeSelectorCursor;
    [SerializeField] private TextMeshProUGUI[] times;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TextMeshProUGUI selectedTime;
    [SerializeField] private TextMeshProUGUI setSelectedTime;
    private bool isScrolling = false;
    private void Update()
    {
        if(scrollRect.velocity.magnitude < 0.1f&&selectedTime==setSelectedTime&&setSelectedTime!=null)
        {
            Debug.Log("Selected Time2: " + setSelectedTime.text);
            selectedTime = null;
        }
    }
    public void OnTimeSelected()
    {
        selectedTime = null;
        float closestDistance = Mathf.Infinity;
        foreach (TextMeshProUGUI time in times)
        {
            float distance = Vector3.Distance(time.transform.position, timeSelectorCursor.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                selectedTime = time;
            }
        }
        if (selectedTime != null)
        {
            setSelectedTime=selectedTime;
            //Debug.Log("Selected Time: " + selectedTime.text);
        }
        else
        {
            Debug.Log("No time selected");
        }
    }
}
