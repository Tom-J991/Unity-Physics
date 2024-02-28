using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    public Material awakeMaterial = null;
    public Material sleepingMaterial = null;

    private Rigidbody m_rigidBody = null;

    bool wasSleeping = false;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (m_rigidBody.IsSleeping() && !wasSleeping && sleepingMaterial != null)
        {
            wasSleeping = true;
            GetComponent<MeshRenderer>().material = sleepingMaterial; // Change material if rigid body is sleeping.
        }
        if (!m_rigidBody.IsSleeping() && wasSleeping && awakeMaterial != null)
        {
            wasSleeping = false;
            GetComponent<MeshRenderer>().material = awakeMaterial; // Change material if rigid body is awake.
        }
    }
}
