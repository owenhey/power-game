using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Transform cameraCenter;
    public float rotateSpeed = 10;

    [SerializeField] private Image screenDarken;

    public Button playButton1p;
    public Button playButton2p;

    private void Awake()
    {
        screenDarken.color = Color.clear;
        
        playButton1p.onClick.AddListener(PlayOneP);
        playButton2p.onClick.AddListener(PlayTwoP);
    }

    private void PlayOneP()
    {
        GameSettings.PlayerCount = 1;
        PlayGame();
    }
    
    private void PlayTwoP()
    {
        GameSettings.PlayerCount = 2;
        PlayGame();
    }

    public void PlayGame()
    {
        playButton1p.interactable = false;
        playButton2p.interactable = false;

        screenDarken.DOColor(Color.black, .5f).OnComplete(() =>
        {
            SceneManager.LoadScene("Tutorial");
        });
    }

    private void Update()
    {
        cameraCenter.transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }
}
