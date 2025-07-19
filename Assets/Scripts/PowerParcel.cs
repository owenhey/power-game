using UnityEngine;

public class PowerParcel : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] collectSounds;
    public ParticleSystem PowerBurstPreFab;
    public int PowerToGive = 100;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.Instance != null)
             GameManager.Instance.Kilowatts += PowerToGive;

            ParticleSystem particles = Instantiate(PowerBurstPreFab);
            particles.transform.position = other.transform.position;

            SoundsOnObjectDestroy.instance.PlayCollect();
            Destroy(gameObject);
        }
    }
}
