using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderboardUIHandler : MonoBehaviour
{

    public GameObject leaderboardItemPrefab;

    SetLeaderboardItemInfo[] setLeaderboardItemInfos;

    private void Awake()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
        CarLapCounter[] carLapCounters = FindObjectsOfType<CarLapCounter>();

        setLeaderboardItemInfos = new SetLeaderboardItemInfo[carLapCounters.Length];

        for(int i = 0; i < carLapCounters.Length; i++)
        {
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);
            setLeaderboardItemInfos[i] = leaderboardInfoGameObject.GetComponent<SetLeaderboardItemInfo>();
            setLeaderboardItemInfos[i].SetPositionText($"{i + 1}.");
        }
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        for(int i = 0; i < lapCounters.Count; i++)
        {
            setLeaderboardItemInfos[i].SetDriverNameText(lapCounters[i].gameObject.name);
        }
    }
}
