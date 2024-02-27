using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBallPrefab = null;
    public Transform spawnPoint = null;

    public float forceOnFire = 1200;
    public float cooldownTime = 0.2f;

    float cooldownTimer = 0.0f;

    private void Update()
    {
        if (Input.anyKeyDown && cooldownTimer <= 0.0f) // On player input with delay.
        {
            GameObject go = Instantiate(cannonBallPrefab, spawnPoint.position, spawnPoint.rotation); // Spawn new cannon ball.
            if (go == null)
                return;

            Rigidbody rb = go.GetComponent<Rigidbody>(); // Get cannon ball's rigid body.
            if (rb == null)
                return;

            rb.AddForce(spawnPoint.up * forceOnFire); // Shoot the cannon ball out of the cannon.

            cooldownTimer = cooldownTime; // Reset timer.
        }

        if (cooldownTimer > 0.0f) cooldownTimer -= Time.deltaTime; // Tick tock.
    }
}
