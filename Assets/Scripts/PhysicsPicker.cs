using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PhysicsPicker : MonoBehaviour
{
    public float force = 100.0f;

    private int layerMask;

    Camera m_cam;

    private void Start()
    {
        m_cam = GetComponent<Camera>();

        string[] layers = { "Default" };
        layerMask = LayerMask.GetMask(layers);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // On left click.
        {
            RaycastHit hit;
            Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) // Find object the mouse has clicked on.
            { 
                Ragdoll ragdoll = hit.collider.GetComponentInParent<Ragdoll>();
                if (ragdoll)
                {
                    ragdoll.RagdollToggle = true; // Enable ragdoll.
                }

                Rigidbody body = hit.collider.GetComponent<Rigidbody>();
                if (body)
                {
                    body.AddForce(ray.direction * force); // Push the object.
                }
            }
        }
    }
}
