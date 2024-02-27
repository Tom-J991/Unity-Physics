using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallThrow : MonoBehaviour
{
    public float forceOnFire = 300;
    
    private Rigidbody m_rigidBody = null;

    bool fire = false;
    bool canFire = true;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.isKinematic = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && canFire)
        {
            m_rigidBody.isKinematic = false;
            m_rigidBody.AddForce(transform.forward * forceOnFire); // Shoot object forwards on input.
            canFire = false;
        }
    }
}
