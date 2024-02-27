using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public TextMeshProUGUI output;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 500) == true)
        {
            output.text = hitInfo.transform.gameObject.name;
        }
    }
}
