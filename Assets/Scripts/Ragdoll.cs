using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Ragdoll : MonoBehaviour
{
    public List<Rigidbody> rigidBodies = new List<Rigidbody>();

    public bool RagdollToggle
    {
        get { return !m_animator.enabled; }
        set 
        { 
            // If ragdoll is enabled then disable animator and character controller, then set all rigidbodies in the ragdoll to kinematic.
            m_animator.enabled = !value;

            CharacterController cc = GetComponentInParent<CharacterController>();
            if (cc) cc.enabled = !value;

            foreach (Rigidbody r in rigidBodies) r.isKinematic = !value;
        } 
    }

    private Animator m_animator = null;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        foreach (Rigidbody r in rigidBodies) r.isKinematic = true; // Setup ragdoll bodies.
    }
}
