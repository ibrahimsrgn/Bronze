using UnityEngine;

public class DoorScript : MonoBehaviour
{
    bool GUIActivater;
    private Animator Animator;

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }
    public void OnRayHit(PlayerData playerData)
    {
        if (playerData != null)
        {
            GUIActivater = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Animator.SetTrigger("Open");
            }
        }
    }

    void OnGUI()
    {
        if (GUIActivater)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 60), "Press 'E' to interact with Door");
        }
    }
    private void OnMouseExit()
    {
        GUIActivater = false;
    }
}
