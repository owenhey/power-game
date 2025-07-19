using UnityEngine;

public class PowerParcel : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] collectSounds;
    public int PowerToGive = 100;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.Instance != null)
             GameManager.Instance.Kilowatts += PowerToGive;

            SoundsOnObjectDestroy.instance.PlayCollect();
            Destroy(gameObject);
        }
    }
}
