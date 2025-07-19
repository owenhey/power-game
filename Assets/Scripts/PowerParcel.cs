using System;
using UnityEngine;

public class PowerParcel : MonoBehaviour
{
    public int PowerToGive = 100;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.Instance != null)
             GameManager.Instance.Kilowatts += PowerToGive;
            Destroy(gameObject);
        }
    }
}
