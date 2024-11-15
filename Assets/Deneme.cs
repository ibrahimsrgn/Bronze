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
        // Ekranýn merkezini hesapla
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 20f);

        // Ekran uzayýndan dünya uzayýna dönüþtür
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);

        // Lerping iþlemi ile yavaþ bir þekilde hedef pozisyona git
        transform.position = Vector3.Lerp(transform.position, worldPosition, 50 * Time.deltaTime);
    }
}
