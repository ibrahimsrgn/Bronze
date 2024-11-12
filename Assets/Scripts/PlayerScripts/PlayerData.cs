using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerData : MonoBehaviour
{
    #region Variables
    private Vector2 MoveInput;
    private Vector3 CurrentInput;

    [Header("Player Movement")]
    public float PlayerWalkSpeed;
    public float SprintSpeed;
    public float SprintSpeedData;

    [Header("Camera Look")]
    private Ray Ray;
    public float MouseSensivity;
    public Transform _Camera;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV pov;
    private Vector2 MouseInput;
    private float MouseX, MouseY, xRotation;

    [Header("Gravity & Jump")]
    [SerializeField] private float Gravity;
    [SerializeField] private float JumpHeight;
    private Vector3 Velocity;
    private bool JumpInput;

    [Header("Left&Right Mouse Button")]
    public bool MouseClickInput;

    [Header("Animator Component")]
    [SerializeField] private Animator animator;
    [SerializeField] private float SmoothSpeed;
    [SerializeField] private RigBuilder _RigBuilder;
    private Vector2 SmoothInput;

    [Header("ItemInteraction")]
    public Transform ItemOnHand;
    public Transform WeaponLoc;
    public CharacterController _CharacterController;
    private bool _OnDrop;
    private bool _OnCollect;

    [SerializeField] private int angle;
    [SerializeField] private int angleZ;
    #endregion

    private void Start()
    {
        pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    void Update()
    {
        MovementGravityFunctions();
        RayOfPlayer();
        LookAround();
    }

    #region Movement & Gravity Functions

    private void MovementGravityFunctions()
    {
        ApplyGravity();
        Jump();
        CharacterMove();
        MovementAnimations();
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

    #region Animations
    private void MovementAnimations()
    {
        foreach (var layer in _RigBuilder.layers)
        {
            if (layer.rig != null && layer.rig.name == "RigLayer-HandRig")
            {
                layer.active = ItemOnHand;
                break;
            }
        }
        animator.SetBool("ItemOnHand", ItemOnHand);
        float targetSpeed = SprintSpeedData > 1 ? 2f : 1f;
        SmoothInput = Vector2.Lerp(SmoothInput, MoveInput * targetSpeed, SmoothSpeed * Time.deltaTime);
        animator.SetFloat("X", SmoothInput.x);
        animator.SetFloat("Y", SmoothInput.y);
    }
    #endregion

    #region RayOfPlayerFunction

    private void RayOfPlayer()
    {
        Ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(Ray, out RaycastHit hitInfo, 2f))
        {
            Debug.DrawRay(Ray.origin, hitInfo.distance * Ray.direction, Color.yellow);
            hitInfo.transform.SendMessage("OnRayHit", this, SendMessageOptions.DontRequireReceiver);
        }
    }
    #endregion


    #region ItemDropScript
    public void DropItem(bool Value)
    {
        if (Value && ItemOnHand != null)
        {
            ItemOnHand.gameObject.GetComponent<WeaponInteraction>().enabled = enabled;
            ItemOnHand.transform.SetParent(null);
            ItemOnHand.gameObject.AddComponent<Rigidbody>();
            ItemOnHand.transform.GetComponent<Rigidbody>().AddForce(transform.forward * angleZ + Vector3.up * angle);
            ItemOnHand = null;
        }
    }
    #endregion


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

    private void OnSprint(InputValue Value)
    {
        SprintSpeedData = Value.isPressed ? SprintSpeed : 1;
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
    #endregion

    #region CameraLook
    private void LookAround()
    {
        if (pov.m_HorizontalAxis.Value <= pov.m_HorizontalAxis.m_MinValue + 20)
        {
            Debug.Log("Yatay eksende minimum sýnýra ulaþýldý.");
            RotateCharacter();
        }
        else if (pov.m_HorizontalAxis.Value >= pov.m_HorizontalAxis.m_MaxValue - 20)
        {
            Debug.Log("Yatay eksende maksimum sýnýra ulaþýldý.");
            RotateCharacter();
        }
    }

    private void RotateCharacter()
    {
        virtualCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 100, 0));
        float TargetLocation = Mathf.LerpAngle(transform.eulerAngles.y, Camera.main.transform.eulerAngles.y, 5f * Time.deltaTime);
        transform.eulerAngles = Quaternion.Euler(transform.eulerAngles.x, TargetLocation, transform.eulerAngles.z).eulerAngles;
    }
    #endregion
}
