using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseScriptDemo : MonoBehaviour
{
    private bool GUIActivater;
    public void OnRayHit()
    {
        GUIActivater = true;
        if (Input.GetKey(KeyCode.E))
        {
            Destroy(gameObject);
        }
    }

    void OnGUI()
    {
        if (GUIActivater)
        {
            Debug.Log("deneme");
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 60), "Press 'E' to eat Cheese");
        }
    }

    private void OnMouseExit()
    {
        GUIActivater = false;
    }
}
