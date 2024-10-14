using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ragdollRoot;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private bool startRagdoll = false;

    private Rigidbody[] rigidbodies;
    private CharacterJoint[] characterJoints;

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        characterJoints = GetComponentsInChildren<CharacterJoint>();
    }
    private void Start()
    {
        if(startRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }
    }
    public void EnableRagdoll()
    {
        animator.enabled = false;
        agent.enabled = false;

        foreach(CharacterJoint characterJoint in characterJoints)
        {
            characterJoint.enableCollision = true;
        }
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
    }
    public void DisableAllRigidbody()
    {
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }
    private void EnableAnimator()
    {
        animator.enabled= true;
        agent.enabled= true;
        foreach(CharacterJoint characterJoint in characterJoints)
        {
            characterJoint.enableCollision = false;
        }
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }
}
