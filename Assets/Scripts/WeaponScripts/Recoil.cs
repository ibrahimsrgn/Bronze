using Unity.Cinemachine;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    public Transform TargetNearWall;
    public Transform target;
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    public void WeaponTargetLocalizer(bool NearToWall)
    {
        if (!NearToWall)
        {
            targetRotation = Vector3.Lerp(targetRotation, target.position, returnSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
            transform.position = targetRotation;
        }
        else if (NearToWall)
        {
            targetRotation = Vector3.Lerp(targetRotation, TargetNearWall.position, returnSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
            transform.position = targetRotation;
        }
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(Random.Range(-recoilX, recoilX), recoilY, Random.Range(-recoilX, recoilX));
    }
}
