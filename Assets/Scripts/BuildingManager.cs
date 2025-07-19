using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
    public static BuildingManager Instance;

    private PowerableBuildings[] buildings;

    [SerializeField] private BuildingPowerDownData[] PowerDownData;

    private List<BuildingPowerDownCycles> cycles;

    public int BuildingsDown => buildings.Count(x => !x.PoweredOn);
    public int TotalBuildings => buildings.Length;

    private void Awake()
    {
        Instance = this;
        buildings = GetComponentsInChildren<PowerableBuildings>();
    }

    private void Update() {
        if (GameManager.Instance.IsPlaying == false) return;
        if (cycles == null && GameManager.Instance.IsPlaying) {
            GenerateCycles();
        }

        if (GameManager.Instance.TimeSinceGameBegan > cycles[0].GameEndTime) {
            cycles.RemoveAt(0);
            if (cycles.Count == 0) {
                this.enabled = false;
                return;
            }
        }

        var currTime = GameManager.Instance.TimeSinceGameBegan;
        var cyclesData = cycles[0];
        for (int i = 0; i < cyclesData.Cycles.Count; i++) {
            var cycle = cyclesData.Cycles[i];
            if(cycle.Fired) continue;
            if (cyclesData.Cycles[i].Time < currTime) {
                FireCycle(cycle);
                break;
            }
        }
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

    private void GenerateCycles() {
        cycles = new();
        for (int i = 0; i < PowerDownData.Length; i++) {
            var data = PowerDownData[i];
            float myTime = GameManager.Instance.GetTimeOfBuildingCycle(data.PercentThrough);
            float nextTime = i < PowerDownData.Length - 1 ? 
                GameManager.Instance.GetTimeOfBuildingCycle(PowerDownData[i + 1].PercentThrough) :
                GameManager.Instance.GetTimeOfBuildingCycle(1);
            cycles.Add(new BuildingPowerDownCycles() {
                GameStartTime = myTime,
                GameEndTime = nextTime,
                Cycles = data.GetPowerDowns(myTime, nextTime)
            });
        }
        
        // Debugs all the cycles out
        // foreach (var cycle in cycles) {
        //     foreach (var powerDown in cycle.Cycles) {
        //         Debug.Log(powerDown);
        //     }
        // }
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
