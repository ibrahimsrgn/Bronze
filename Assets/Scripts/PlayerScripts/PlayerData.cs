using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerData : MonoBehaviour
{
    #region Variables
    public static PlayerData Instance;
    [HideInInspector] public Vector2 MoveInput;
    private Vector3 CurrentInput;
    public HealthManager healthManager;

    [Header("Player Movement")]
    public float PlayerWalkSpeed;
    public float SprintSpeed;
    public float SprintSpeedData;
    [HideInInspector]
    public bool MouseClickInput;
    public bool _OnCrouch;

    [Header("Camera Look")]
    [SerializeField] private float MouseSensivity;
    [SerializeField] private Transform CamPosRef1;
    [SerializeField] public Transform CamPosRef2;
    [SerializeField] private Transform MainCamPos;
    [SerializeField] public Transform WeaponPosRot;
    [SerializeField] private float CamPosSmoothSpeed;
    private Ray Ray;
    private bool OnAimBool;
    private Vector2 MouseInput;
    private float MouseX, MouseY, xRotation;

    [Header("Gravity & Jump")]
    [SerializeField] private float Gravity;
    [SerializeField] private float JumpHeight;
    private Vector3 Velocity;
    private bool JumpInput;


    [Header("Animator Component")]
    [SerializeField] private Animator Animator;
    [SerializeField] private float SmoothSpeed;
    [SerializeField] public RigBuilder _RigBuilder;
    private Vector2 SmoothInput;
    public bool OnReloadBool;

    [Header("ItemInteraction")]
    public Transform ItemOnHand;
    public Transform WeaponLoc;
    public CharacterController _CharacterController;
    public GameObject FlashLight;
    private bool _OnDrop;
    private bool _OnCollect;
    private int FlashCount = 1;

    [Header("Rig Layer References")]
    public TwoBoneIKConstraint LeftHandLayer;
    public TwoBoneIKConstraint RightHandLayer;

    [Header("Pos backups")]
    private TwoBoneIKConstraint LeftHandLayerBckUp;
    private TwoBoneIKConstraint RightHandLayerBckUp;
    public Transform CamPosRef2BckUp;

    [SerializeField] private int angle;
    [SerializeField] private int angleZ;

    #endregion

    //-------------------------------------------------------------------
    #region Update Methods
    private void Awake()
    {
        LeftHandLayerBckUp = LeftHandLayer;
        RightHandLayerBckUp = RightHandLayer;
        CamPosRef2BckUp = CamPosRef2;
        Instance = this;
        if (WeaponLoc.childCount <= 0)
            ItemOnHand = null;
        else
            ItemOnHand = WeaponLoc.GetChild(0);
        LayerMaskUpdater();
    }

    private void LateUpdate()
    {
        WeaponAim(OnAimBool);
        MovementAnimations();
        MovementGravityFunctions();
        RayOfPlayer();
        LookAround();
    }
    #endregion

    //-------------------------------------------------------------------

    #region Movement & Gravity Functions

    private void MovementGravityFunctions()
    {
        ApplyGravity();
        Jump();
        CharacterMove();
        _CharacterController.Move(Velocity * Time.deltaTime);
    }
    private void CharacterMove()
    {
        CurrentInput = transform.right * MoveInput.x + transform.forward * MoveInput.y;
        _CharacterController.Move(CurrentInput * PlayerWalkSpeed * Time.deltaTime * (MoveInput.y > 0 ? SprintSpeedData : 1));
    }

    private void ApplyGravity()
    {
        if (!_CharacterController.isGrounded)
        {
            Velocity.y += Gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (JumpInput && _CharacterController.isGrounded)
        {
            Velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
        if (_CharacterController.isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }
        _CharacterController.Move(Velocity * Time.deltaTime);
    }
    #endregion

    //-------------------------------------------------------------------

    #region Animations
    private void MovementAnimations()
    {
        LayerMaskUpdater();
        Animator.SetBool("ItemOnHand", ItemOnHand);
        float targetSpeed = SprintSpeedData > 1 ? 2f : 1f;
        SmoothInput = Vector2.Lerp(SmoothInput, MoveInput * targetSpeed, SmoothSpeed * Time.deltaTime);
        Animator.SetFloat("X", SmoothInput.x);
        Animator.SetFloat("Y", SmoothInput.y);
    }

    private void LayerMaskUpdater()
    {
        foreach (var layer in _RigBuilder.layers)
        {
            if (layer.rig != null && layer.rig.name == "RigLayer-HandRig")
            {
                layer.active = ItemOnHand;
                break;
            }
        }
    }
    #endregion

    //-------------------------------------------------------------------

    #region RayOfPlayerFunction

    private void RayOfPlayer()
    {
        Ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(Ray, out RaycastHit hitInfo, 2f))
        {
            Debug.DrawRay(Ray.origin, hitInfo.distance * Ray.direction, Color.yellow);
            hitInfo.transform.SendMessage("OnRayHit", this, SendMessageOptions.DontRequireReceiver);
            ZombieAi zombi = hitInfo.transform.GetComponentInParent<ZombieAi>();
            if (zombi != null)
            {
                zombi.OnRayHit(this);
            }
        }
    }
    #endregion

    //-------------------------------------------------------------------

    #region ItemDropScript
    public void DropItem(bool Value)
    {
        if (Value && ItemOnHand != null)
        {
            if (ItemOnHand.GetComponent<GunFire>() != null)
            {
                ItemOnHand.gameObject.GetComponent<Animator>().enabled = false;
                ItemOnHand.gameObject.GetComponent<GunFire>().enabled = enabled;
            }
            if (ItemOnHand.TryGetComponent<ItemInteraction>(out ItemInteraction itemInteraction) && InventoryManager.instance.GetSelectedItemCount() > 1)
            {
                GameObject droppedItem = Instantiate(ItemOnHand.gameObject, ItemOnHand);
                droppedItem.transform.SetParent(null);
                droppedItem.gameObject.AddComponent<Rigidbody>();
                droppedItem.transform.GetComponent<Rigidbody>().AddForce(transform.forward * angleZ + Vector3.up * angle);
            }
            else
            {
                LeftHandLayer.data.target = null;
                RightHandLayer.data.target = null;
                ItemOnHand.transform.SetParent(null);
                ItemOnHand.gameObject.AddComponent<Rigidbody>();
                ItemOnHand.transform.GetComponent<Rigidbody>().AddForce(transform.forward * angleZ + Vector3.up * angle);
                ItemOnHand = null;
            }
            InventoryManager.instance.DropSelectedItem();
        }
    }
    public void UnEquipItem()
    {

        LeftHandLayer.data.target = null;
        RightHandLayer.data.target = null;
        ItemOnHand = null;
        if (ItemOnHand == null) return;
        if (ItemOnHand.gameObject.GetComponent<Animator>() != null)
        {
            ItemOnHand.gameObject.GetComponent<Animator>().enabled = false;
        }
    }
    #endregion

    //-------------------------------------------------------------------

    #region InputManagerHandler
    private void OnWasd(InputValue Value)
    {
        MoveInput = Value.Get<Vector2>();
    }

    private void OnLook(InputValue Value)
    {
        MouseInput = Value.Get<Vector2>();
    }

    private void OnJump(InputValue Value)
    {
        JumpInput = Value.isPressed;
    }

    private void OnFlash(InputValue Value)
    {
        if (Value.isPressed)
        {
            FlashCount++;
        }
        FlashLight.active = (FlashCount % 2 == 0);
    }

    private void OnSprint(InputValue Value)
    {
        if (!OnAimBool && Value.isPressed && !_OnCrouch)
        {
            SprintSpeedData = SprintSpeed;
        }
        else
            SprintSpeedData = 1;
    }

    private void OnFire(InputValue Value)
    {
        MouseClickInput = Value.isPressed;
    }

    private void OnDrop(InputValue Value)
    {
        _OnDrop = Value.isPressed;
        DropItem(Value.isPressed);
    }

    private void OnCollect(InputValue Value)
    {
        _OnCollect = Value.isPressed;
    }

    private void OnAim(InputValue Value)
    {
        OnAimBool = Value.isPressed;
    }

    private void OnReload(InputValue Value)
    {
        OnReloadBool = Value.isPressed;
    }

    private void OnCrouch(InputValue Value)
    {
        _OnCrouch = Value.isPressed;
        Animator.SetBool("Crouch", Value.isPressed);
    }

    private void InstaTransfer(InputValue value)
    {

    }
    #endregion

    //-------------------------------------------------------------------

    #region CameraLook
    private void LookAround()
    {
        float TargetLocation = Mathf.LerpAngle(transform.eulerAngles.y, Camera.main.transform.eulerAngles.y, 0.1f);
        transform.eulerAngles = Quaternion.Euler(transform.eulerAngles.x, TargetLocation, transform.eulerAngles.z).eulerAngles;
    }

    private void WeaponAim(bool Aiming)
    {
        if (Aiming && ItemOnHand != null)
        {
            CameraLocStabilizer(CamPosRef2, CamPosSmoothSpeed);
        }
        else
        {
            CameraLocStabilizer(CamPosRef1, CamPosSmoothSpeed / 2);
        }
    }

    void CameraLocStabilizer(Transform toCamera, float CamPosSpeedFixer)
    {
        Vector3 desiredPosition = toCamera.position;

        MainCamPos.position = Vector3.Lerp(MainCamPos.position, desiredPosition, CamPosSpeedFixer * Time.deltaTime);
    }

    #endregion
}
