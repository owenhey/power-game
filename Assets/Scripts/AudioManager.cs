using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [SerializeField] private AudioSource ASLoop;
    [SerializeField] private AudioSource ASLoop2;
    [SerializeField] private AudioClip loopingSound;

    public void StopMusic()
    {
        ASLoop.Stop();    
        ASLoop2.Stop();    
        StopAllCoroutines();
    }
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartCoroutine(DelayedLoop());
    }

    private IEnumerator DelayedLoop() {
        ASLoop.clip = loopingSound;
        ASLoop2.clip = loopingSound;
        ASLoop.Play();
        yield return new WaitForSeconds(ASLoop.clip.length - 30);
        ASLoop.DOFade(0, 10).SetDelay(3.0f);
        ASLoop2.DOFade(1, 10).From(0);
        ASLoop2.Play();
    }
}
