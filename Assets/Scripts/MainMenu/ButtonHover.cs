using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private AudioSource buttonSwitchSound;

    private void Awake()
    {
        Color color = image.color;
        color.a = 0.0f;
        image.color = color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = image.color;
        color.a = 1f;
        image.color = color;
        buttonSwitchSound.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color color = image.color;
        color.a = 0.0f;
        image.color = color;
    }
}
