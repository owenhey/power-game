using System;
using UnityEngine;

public class PowerParcel : MonoBehaviour
{
    public int PowerToGive = 100;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Kilowatts += PowerToGive;
            Destroy(gameObject);
        }
    }
}
