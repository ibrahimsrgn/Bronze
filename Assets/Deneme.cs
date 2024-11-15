using UnityEngine;

public class Deneme : MonoBehaviour
{
    public Transform deneme;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Ekran�n merkezini hesapla
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 20f);

        // Ekran uzay�ndan d�nya uzay�na d�n��t�r
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);

        // Lerping i�lemi ile yava� bir �ekilde hedef pozisyona git
        transform.position = Vector3.Lerp(transform.position, worldPosition, 50 * Time.deltaTime);
    }
}
