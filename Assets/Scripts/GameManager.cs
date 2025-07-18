using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public float TimeSinceGameBegan;
    public bool IsPlaying = false;

    public static GameManager Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartGame();
    }
    
    public void StartGame() {
        TimeSinceGameBegan = 0;
        
        IsPlaying = true;
    }

    private void Update() {
        if (IsPlaying == false) return;
        TimeSinceGameBegan += Time.deltaTime;
    }
}
