using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform prefab;

    private float timer = 0.0f;

    private void Update()
    {
        if (Input.anyKey == true && timer >= 0.3f) // On player input with delay. 
        {
            Instantiate(prefab, transform.position, Quaternion.identity); // Spawn object at spawner's location.
            timer = 0.0f; // Reset timer.
        }

        timer += Time.deltaTime; // Tick tock.
    }
}
