using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
    public static BuildingManager Instance;

    private PowerableBuildings[] buildings;

    [SerializeField] private BuildingPowerDownData[] PowerDownData;

    public int numBuildingsToDepower = 100;
    public AnimationCurve Distrbution;
    public int ParcelsSpawnedSoFar = 0;
    public int BuildingsDown => buildings.Count(x => !x.PoweredOn);
    public int TotalBuildings => buildings.Length;

    private void Awake()
    {
        Instance = this;
        buildings = GetComponentsInChildren<PowerableBuildings>();
    }

    private void Update() {
        Debug.Log("1");
        if (GameManager.Instance.IsPlaying == false) return;
        float percentThroughGame = GameManager.Instance.TimeSinceGameBegan / GameManager.Instance.GameSeconds;
        if (percentThroughGame > 1.0f) return;

        float percent = (float)(ParcelsSpawnedSoFar + 1) / (numBuildingsToDepower + 1);
        if (percent < Distrbution.Evaluate(percentThroughGame))
        {
            SpawnParcel();
        }
    }

    private void SpawnParcel()
    {
        ParcelsSpawnedSoFar++;
        if (ParcelsSpawnedSoFar % 2 == 0 && GameSettings.PlayerCount == 1) return;
        var poweredBuildings = GetPoweredBuildings(true);
        int index = Random.Range(0, poweredBuildings.Count);
        poweredBuildings[index].PowerDown();
        poweredBuildings.RemoveAt(index);
    }

    private void FireCycle(BuildingPowerDownCycle cycle) {
        cycle.Fired = true;
        var poweredBuildings = GetPoweredBuildings(true);
        for (int i = 0; i < cycle.NumToPowerDown; i++) {
            int index = Random.Range(0, poweredBuildings.Count);
            poweredBuildings[index].PowerDown();
            poweredBuildings.RemoveAt(index);
        }
    }

    public void RecalculateLoseCondition() {
        var unpoweredBuildings = GetPoweredBuildings(false);
        if (unpoweredBuildings.Count >= buildings.Length / 3.0) {
            GameManager.Lost = true;
            GameManager.Instance.EndGame();
        }
    }

    private List<PowerableBuildings> GetPoweredBuildings(bool powered) {
        return buildings.Where(x => x.PoweredOn == powered).ToList();
    }
}

[System.Serializable]
public class BuildingPowerDownData {
    public int NumToPowerDown;
    public int NumCycles;
    public int MaxNumAtOnce;

    [Range(0, 1)] public float PercentThrough;

    public List<BuildingPowerDownCycle> GetPowerDowns(float timeNow, float nextTimeEnd) {
        var cycleData = new List<BuildingPowerDownCycle>();
   
        if (NumToPowerDown <= 0 || NumCycles <= 0) return cycleData;
   
        float totalDuration = nextTimeEnd - timeNow;
        float cycleDuration = totalDuration / NumCycles;
   
        int[] powerDownsPerCycle = new int[NumCycles];
        int basePowerDowns = NumToPowerDown / NumCycles;
        int remainder = NumToPowerDown % NumCycles;
   
        for (int i = 0; i < NumCycles; i++) {
            powerDownsPerCycle[i] = basePowerDowns;
        }
   
        for (int i = 0; i < remainder; i++) {
            int randomIndex = Random.Range(0, NumCycles);
            while (powerDownsPerCycle[randomIndex] >= MaxNumAtOnce) {
                randomIndex = (randomIndex + 1) % NumCycles;
            }
            powerDownsPerCycle[randomIndex]++;
        }
   
        for (int i = 0; i < NumCycles; i++) {
            float baseTime = timeNow + (i * cycleDuration);
            float jitter = Random.Range(-cycleDuration * 0.2f, cycleDuration * 0.2f);
            float cycleTime = Mathf.Clamp(baseTime + jitter, timeNow, nextTimeEnd);
       
            cycleData.Add(new BuildingPowerDownCycle {
                NumToPowerDown = powerDownsPerCycle[i],
                Time = cycleTime + .1f
            });
        }
   
        return cycleData;
    }
}

public class BuildingPowerDownCycles {
    public List<BuildingPowerDownCycle> Cycles = new();
    public float GameStartTime;
    public float GameEndTime;
}

public class BuildingPowerDownCycle {
    public int NumToPowerDown;
    public float Time;
    public bool Fired = false;

    public override string ToString() {
        return ("Time: " + Time + ", num: " + NumToPowerDown);
    }
}
