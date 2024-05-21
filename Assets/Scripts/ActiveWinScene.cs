using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWinScene : MonoBehaviour
{
    [SerializeField] GameObject Player1Win;
    [SerializeField] GameObject Player2Win;

    CarLapCounter p1LapCounter;
    CarLapCounter p2LapCounter;

    private void Start()
    {
        // Obtém todos os objetos do tipo CarLapCounter
        CarLapCounter[] lapCounters = FindObjectsOfType<CarLapCounter>();

        // Itera por todos os objetos encontrados
        foreach (CarLapCounter lapCounter in lapCounters)
        {
            // Verifica o playerNumber e atribui ao jogador apropriado
            if (lapCounter.input.playerNumber == 1)
            {
                p1LapCounter = lapCounter;
                Debug.Log("Player 1 carLapCounter assigned");
            }
            else if (lapCounter.input.playerNumber == 2)
            {
                p2LapCounter = lapCounter;
                Debug.Log("Player 2 carLapCounter assigned");
            }
        }

        // Verifica se os objetos foram atribuídos corretamente
        if (p1LapCounter == null)
        {
            Debug.LogWarning("Player 1 carLapCounter not found!");
        }
        if (p2LapCounter == null)
        {
            Debug.LogWarning("Player 2 carLapCounter not found!");
        }
    }

    void Update()
    {
        if (p1LapCounter != null && p1LapCounter.WinCondition())
        {
            Debug.Log("Player 1 completed the race");
            Player1Win.SetActive(true);
        }
        else if (p2LapCounter != null && p2LapCounter.WinCondition())
        {
            Debug.Log("Player 2 completed the race");
            Player2Win.SetActive(true);
        }
    }
}
