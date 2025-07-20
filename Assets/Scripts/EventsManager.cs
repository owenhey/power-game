using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public AnimationCurve PowerParcelsDistribution;
    public int NumParcelsToSpawn = 10;
    public int ParcelsSpawnedSoFar = 0;
    public PowerParcel PowerParcelPrefab;

    [Space(30)] 
    [SerializeField] private List<Transform> ParcelSpawnPos;
    
    private void Update() {
        if (GameManager.Instance.IsPlaying == false) return;

        float percentThroughGame = GameManager.Instance.TimeSinceGameBegan / GameManager.Instance.GameSeconds;
        if (percentThroughGame > 1.0f) return;

        float percent = (float)(ParcelsSpawnedSoFar + 1) / (NumParcelsToSpawn + 1);
        if (percent < PowerParcelsDistribution.Evaluate(percentThroughGame))
        {
            SpawnParcel();
        }
    }

    private void SpawnParcel()
    {
        ParcelsSpawnedSoFar++;
        if (ParcelsSpawnedSoFar % 2 == 0 && GameSettings.PlayerCount == 1) return;
        var randomizedSpawns = ParcelSpawnPos.OrderBy(x => Random.Range(0.0f, 1.0f)).ToArray();
        for (int i = 0; i < randomizedSpawns.Length; i++)
        {
            var ranSpawn = randomizedSpawns[i];
            if (ranSpawn.transform.childCount == 0)
            {
                Instantiate(PowerParcelPrefab, ranSpawn);
                return;
            }
        }
    }
}