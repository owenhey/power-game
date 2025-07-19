using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [SerializeField] private AudioSource ASIntro;
    [SerializeField] private AudioSource ASLoop;
    [SerializeField] private AudioClip introSound;
    [SerializeField] private AudioClip loopingSound;
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        ASIntro.clip = introSound;
        ASIntro.Play();
        StartCoroutine(DelayedLoop());
    }

    private IEnumerator DelayedLoop() {
        ASLoop.clip = loopingSound;
        yield return new WaitForSeconds(ASIntro.clip.length);
        ASLoop.Play();
    }
}
