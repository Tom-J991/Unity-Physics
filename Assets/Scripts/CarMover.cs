using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    public List<Rigidbody> wheels = new List<Rigidbody>();

    public float maxSpeed = 1000.0f;
    public float acceleration = 300.0f;
    public float maxTurn = 45.0f;
    public float turnSpeed = 300.0f;
    public float brakeMultiplier = 4.0f;

    private List<HingeJoint> m_backJoints = new List<HingeJoint>();
    private List<HingeJoint> m_frontJoints = new List<HingeJoint>();

    private float m_steeringInput = 0.0f;
    private bool m_accelerationInput = false;
    private bool m_brakeInput = false;

    private float m_initialWheelDrag = 0.0f;
    private float m_currentWheelDrag = 0.0f;

    private void Start()
    {
        List<float> wheelDrags = new List<float>();
        foreach (HingeJoint joint in GetComponentsInChildren<HingeJoint>())
        {
            if (joint.anchor.z < 0.0f) // Is at back of vehicle.
                m_backJoints.Add(joint);
            else if (joint.anchor.z > 0.0f) // Is at front of vehicle.
                m_frontJoints.Add(joint);
        }

        foreach (Rigidbody rb in wheels)
        {
            wheelDrags.Add(rb.drag);
        }

        float averageWheelDrag = 0.0f;
        for (int i = 0; i < wheelDrags.Count; i++)
        {
            averageWheelDrag += wheelDrags[i];
        }
        averageWheelDrag /= 2.0f;

        m_initialWheelDrag = averageWheelDrag;
        m_currentWheelDrag = averageWheelDrag;
    }

    private void Update()
    {
        m_steeringInput = Input.GetAxisRaw("Horizontal");
        m_accelerationInput = Input.GetKey(KeyCode.Space);
        m_brakeInput = Input.GetKey(KeyCode.LeftShift);
    }

    private void FixedUpdate()
    {
        if (m_brakeInput) m_currentWheelDrag = m_initialWheelDrag * brakeMultiplier;
        else m_currentWheelDrag = m_initialWheelDrag;

        foreach (HingeJoint j in m_backJoints)
        {
            if (m_accelerationInput)
            {
                JointMotor nm = new JointMotor();
                nm.targetVelocity = -1.0f * maxSpeed;
                nm.force = acceleration;
                j.motor = nm;
            }
            else
            {
                JointMotor nm = new JointMotor();
                nm.targetVelocity = -1.0f * maxSpeed;
                nm.force = 0.0f;
                j.motor = nm;
            }
        }

        foreach (HingeJoint j in m_frontJoints)
        {
            JointMotor nm = new JointMotor();
            nm.targetVelocity = -m_steeringInput * maxTurn;
            nm.force = turnSpeed;
            j.motor = nm;
        }

        foreach (Rigidbody rb in wheels)
        {
            rb.drag = m_currentWheelDrag;
        }
    }
}
