using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBuilder : MonoBehaviour
{
    [Tooltip("A prefab that we clone several times as children of this object.")]
    public PhysicsBuilderPart prefab;

    [Tooltip("How many prefabs to clone?")]
    public int count;
    [Tooltip("Offset in local space of this object for positioning each child.")]
    public Vector3 offset;

    [Tooltip("Should the starting part be fixed in place?")]
    public bool fixStart;
    [Tooltip("Should the end part be fixed in place?")]
    public bool fixEnd;

    [Tooltip("The breaking force to set on each joint.")]
    public float breakingForce = float.PositiveInfinity;

    [ContextMenu("Build")]
    public void Build()
    {
        if (prefab == null) // Can't build anything if there's no prefab selected.
            return;

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>()) // Destroy old objects to rebuild.
            DestroyObj(rb.gameObject);

        PhysicsBuilderPart previous = null;
        for (int i = 0; i < count; i++)
        {
            // Instantiate a new part and set it up.
            PhysicsBuilderPart instance = Instantiate(prefab, transform);
            instance.transform.localPosition = i * offset;
            instance.transform.localRotation = prefab.transform.localRotation;
            instance.name = name + "_" + i;

            // Set part to kinematic if the chain/rope is configured to be fixed from the starting part or the end part.
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.isKinematic = ((i == 0 && fixStart) || (i == count - 1 && fixEnd));

            // Hook up the part with the previous part to create the chain/rope.
            if (previous)
            {
                foreach (Joint joint in previous.forwardJoints)
                {
                    joint.connectedBody = rb;
                    joint.breakForce = breakingForce;
                }
            }
            previous = instance;
        }
    }

    [ContextMenu("Set Break Force")]
    public void SetBreakingForce()
    {
        if (breakingForce != 0.0f)
        {
            foreach (Joint joint in GetComponentsInChildren<Joint>())
            {
                joint.breakForce = breakingForce;
            }
        }
    }

    private void DestroyObj(Object obj)
    {
        // Check whether the application is playing or in edit mode and call the appropriate function.
        if (Application.isPlaying)
            Destroy(obj);
        else
            DestroyImmediate(obj);
    }
}
