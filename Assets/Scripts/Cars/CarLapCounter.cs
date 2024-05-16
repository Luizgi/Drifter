using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarLapCounter : MonoBehaviour
{
    public Text carPositionText;

    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;

    int numberOfPassedCheckpoints = 0;

    public int lapsCompleted = 0;
    public const int lapsToComplete = 5;
    [SerializeField] int carPosition = 0;
    public InputHandler input;

    public bool isRaceCompleted = false;


    bool isHideRoutineRunning = false;
    float hideUIDelayTimer;



    public event Action<CarLapCounter> OnPassCheckpoint;

    public bool WinCondition()
    {
        if(lapsCompleted >= lapsToComplete) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Start()
    {
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        input = GetComponent<InputHandler>();
    }
    public void SetCarPosition(int position)
    {
        carPosition = position;
    }

    public int GetNumberOfCheckPointsPassed()
    {
        return numberOfPassedCheckpoints;
    }

    public float GetTimeAtLastCheckPoint()
    {
        return timeAtLastPassedCheckPoint;
    }

    IEnumerator ShowPositionC(float delayUntilHidePosition)
    {
        hideUIDelayTimer += delayUntilHidePosition;

        carPositionText.text = carPosition.ToString();

        carPositionText.gameObject.SetActive(true);

        if (!isHideRoutineRunning)
        {
            isHideRoutineRunning = true;

            yield return new WaitForSeconds(hideUIDelayTimer);
            carPositionText.gameObject.SetActive(false);

            isHideRoutineRunning = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPoint"))
        {
            if (isRaceCompleted)
                return;

            CheckPoint checkPoint = collision.GetComponent<CheckPoint>();

            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;

                numberOfPassedCheckpoints++;

                timeAtLastPassedCheckPoint = Time.time;

                if(checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;
                    Debug.Log(lapsCompleted);

                    if(lapsCompleted >= lapsToComplete)
                    {
                        isRaceCompleted = true;
                    }


                }
                OnPassCheckpoint?.Invoke(this);
                if (isRaceCompleted)
                    StartCoroutine(ShowPositionC(100));
                else StartCoroutine(ShowPositionC(1.5f));

            }

        }
    }
}
