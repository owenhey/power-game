using System;
using UnityEngine;

public class BuildingsTest : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating(nameof(MyMethod), .25f, 4);
    }

    private void Update()
    {
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
