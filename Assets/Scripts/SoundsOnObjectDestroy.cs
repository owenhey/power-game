using UnityEngine;

public class SoundsOnObjectDestroy : MonoBehaviour
{
    public static SoundsOnObjectDestroy instance;
    [SerializeField] private AudioSource[] collectParcelSounds;

    public void Awake()
    {
        instance = this;
    }
    public void PlayCollect()
    {
        collectParcelSounds[Random.Range(0, collectParcelSounds.Length)].Play();
    }
}
