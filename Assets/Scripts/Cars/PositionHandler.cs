using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();
    LeaderboardUIHandler leaderboardUIHandler;

    // Start is called before the first frame update
    void Awake()
    {
        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();

        CarLapCounter[] carLapCountersArray = FindObjectsOfType<CarLapCounter>();

        carLapCounters = carLapCountersArray.ToList<CarLapCounter>();

        foreach (CarLapCounter lapCounters in carLapCounters)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckPoint;
        }
    }

    private void Start()
    {
        leaderboardUIHandler.UpdateList(carLapCounters);
    }


    void OnPassCheckPoint(CarLapCounter carLapCounter)
    {
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckPointsPassed()).ThenBy(s => s.GetTimeAtLastCheckPoint()).ToList();

        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        carLapCounter.SetCarPosition(carPosition);

        leaderboardUIHandler.UpdateList(carLapCounters);
    }
}
