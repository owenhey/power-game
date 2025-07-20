using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [SerializeField] private AudioSource ASIntro;
    [SerializeField] private AudioSource ASLoop;
    [SerializeField] private AudioSource ASLoop2;
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
        ASLoop2.clip = loopingSound;
        yield return new WaitForSeconds(ASIntro.clip.length - 3);
        ASLoop.Play();
        ASLoop.DOFade(1, 3.0f);
        yield return new WaitForSeconds(ASIntro.clip.length - 10);

        ASLoop.DOFade(0, 5);
        ASLoop2.DOFade(1, 5).From(0);
        ASLoop2.Play();
    }
}
