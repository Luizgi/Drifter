using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public int playerNumber = 1;

    CarController carController;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    private void Update()
    {
        Vector2 inputVector = Vector2.zero;

        switch(playerNumber)
        {
             case 1:
                inputVector.x = Input.GetAxis("Horizontal_P1");
                inputVector.y = Input.GetAxis("Vertical_P1");
                break;
            case 2:
                inputVector.x = Input.GetAxis("Horizontal_P2");
                inputVector.y = Input.GetAxis("Vertical_P2");
                break;
            case 3:
                inputVector.x = Input.GetAxis("Horizontal_P3");
                inputVector.y = Input.GetAxis("Vertical_P3");
                break;
            case 4: 
                inputVector.x = Input.GetAxis("Horizontal_P4");
                inputVector.y = Input.GetAxis("Vertical_P4");
                break;

    }


        carController.SetInputVector(inputVector);
    }
}
