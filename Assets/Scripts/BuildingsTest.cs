using System;
using UnityEngine;

public class BuildingsTest : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating(nameof(MyMethod), 1, 4);
    }

    private void Update()
    {
            Debug.Log("Clicking");
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000))
            {
                transform.position = hit.point;
            }
    }

    private void MyMethod()
    {
        GameManager.Instance.Kilowatts += 100;
    }
}
