using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Transform target = null;

    public float distance = 6.0f; // Distance away from target.
    public float verticalOffset = 1.0f; //

    public float speed = 180.0f; // Rotation speed (degrees per second).
    public float zoomSpeed = 4.0f; //
    public float distanceRelaxSpeed = 12.0f; // The speed which the camera relaxes back to the desired distance after adjusting to a collision.

    public float minPitch = 0.0f;
    public float maxPitch = 70.0f;

    public float minZoomDistance = 2.0f;
    public float maxZoomDistance = 20.0f;

    public bool verticalInvert = false;

    [SerializeField] private float m_currentDistance = 0.0f;

    private int layerMask;

    private void Start()
    {
        m_currentDistance = distance;

        string[] layers = { "Default" };
        layerMask = LayerMask.GetMask(layers);
    }

    private void Update()
    {
        // Do camera rotation.
        if (Input.GetMouseButton(1)) // On right click.
        {
            Vector3 angles = transform.eulerAngles; // Store object's current rotation.

            // Get mouse input.
            float dx = -Input.GetAxis("Mouse Y");
            float dy = Input.GetAxis("Mouse X");
            if (verticalInvert) // Invert vertical input.
                dx *= -1f;

            // Apply mouse movement to rotation.
            angles.x = Mathf.Clamp(angles.x + dx * speed * Time.deltaTime, minPitch, maxPitch); // Clamp pitch so the camera doesn't flip upside down.
            angles.y += dy * speed * Time.deltaTime;

            transform.eulerAngles = angles; // Apply adjusted angles to object.
        }

        // Do camera collisions. 
        RaycastHit hit;
        if (Physics.Raycast(GetTargetPosition(), -transform.forward, out hit, distance, layerMask)) // Cast ray from target position towards the camera.
        {
            m_currentDistance = hit.distance; // Snap the camera distance to the distance between the collision and the target.
        }
        else
        {
            m_currentDistance = Mathf.MoveTowards(m_currentDistance, distance, distanceRelaxSpeed * Time.deltaTime); // Relax the current distance back to the desired distance.
        }

        // Do camera zoom.
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoomDistance, maxZoomDistance);

        // Do camera movement.
        transform.position = GetTargetPosition() - m_currentDistance * transform.forward; // Camera orbits around target object with offset and zoom.
    }

    public Vector3 GetTargetPosition()
    {
        return target.position + verticalOffset * target.up;
    }
}
