using System;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public float TimeSinceGameBegan;
    public bool IsPlaying = false;

    public int Kilowatts
    {
        get
        {
            return kilowatts;
        }
        set
        {
            kilowatts = value;
            UI.Refresh();
        }
    }
    private int kilowatts;

    [Header("Game Stats")] 
    public int GameSeconds = 180;

    public static GameManager Instance;

    private float TimeGameBegun;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartGame();
    }
    
    public void StartGame() {
        TimeSinceGameBegan = 0;
        Kilowatts = 0;
        TimeGameBegun = Time.time;
        
        IsPlaying = true;
    }

    private void Update() {
        if (IsPlaying == false) return;
        TimeSinceGameBegan += Time.deltaTime;
    }

    public float GetTimeOfBuildingCycle(float percent) {
        return Mathf.Lerp(TimeGameBegun, TimeGameBegun + GameSeconds, percent);
    }
}
