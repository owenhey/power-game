using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public float TimeSinceGameBegan;
    public bool IsPlaying = false;

    public static bool Lost;
    
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

    private float TimeGameBegun {
        get {
            return _timeGameBegun;
        }
        set {
            _timeGameBegun = value;
            UI.Refresh();
        }
    }
    private float _timeGameBegun;

    public void EndGame() {
        IsPlaying = false;
        UI.ShowEndGame();
    }
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartGame();
    }
    
    public void StartGame()
    {
        _timeGameBegun = Time.time;
        TimeSinceGameBegan = 0;
        Kilowatts = 0;
        TimeGameBegun = Time.time;
        Lost = false;
        
        IsPlaying = true;
    }

    private void Update() {
        if (IsPlaying == false) return;
        TimeSinceGameBegan += Time.deltaTime;

        if (TimeSinceGameBegan > GameSeconds) {
            Lost = false;
            EndGame();
        }
    }

    public float GetTimeOfBuildingCycle(float percent) {
        return Mathf.Lerp(TimeGameBegun, TimeGameBegun + GameSeconds, percent);
    }
}
