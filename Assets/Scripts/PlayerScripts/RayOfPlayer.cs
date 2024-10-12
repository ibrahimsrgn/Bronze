using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayOfPlayer : MonoBehaviour
{
    private PlayerData playerData;

    void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    void Update()
    {
        playerData.Ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(playerData.Ray, out RaycastHit hitInfo, 2f))
        {
            Debug.DrawRay(playerData.Ray.origin, hitInfo.distance * playerData.Ray.direction, Color.yellow);
            hitInfo.transform.SendMessage("OnRayHit", hitInfo, SendMessageOptions.DontRequireReceiver);
        }
    }
}
