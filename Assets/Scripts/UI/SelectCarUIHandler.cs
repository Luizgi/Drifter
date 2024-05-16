using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SelectCarUIHandler : MonoBehaviour
{
    [Header("Car Prefab")]
    public GameObject carPrefab;

    [Header("Spawn On")]
    public Transform spawnOnTransform;

    bool isChangingCar = false;

    CarUIHandler carUIHandler = null;
    Manager manager;

    CarData[] carDatas;
    int selectedCarIndex = 0;

    [Header("Controls")]
    [SerializeField] KeyCode Left = KeyCode.A;
    [SerializeField] KeyCode Right = KeyCode.D;

    

    private void Start()
    {
        manager = FindAnyObjectByType<Manager>();

        carDatas = Resources.LoadAll<CarData>("CarData/");
        StartCoroutine(SpawnCarCO(true));
    }

    private void Update()
    {
        //P1
        if (Input.GetKeyDown(Left))
        {
            OnPreviousCar();
        }
        else if(Input.GetKeyDown(Right))
        {
            OnNextCar();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSelectCar(1);
            manager.hasP1 = true;
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            OnSelectCar(2);
            manager.hasP2 = true;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton15))
        {
            OnSelectCar(3);
            manager.hasP3 = true;
        }
    }

    public void OnPreviousCar()
    {
        if (isChangingCar)
            return;
        selectedCarIndex--;

        if (selectedCarIndex < 0)
        {
            selectedCarIndex = carDatas.Length - 1;
        }
        StartCoroutine(SpawnCarCO(true));
    }

    public void OnNextCar()
    {
        if (isChangingCar)
            return;
        selectedCarIndex++;

        if (selectedCarIndex > carDatas.Length - 1)
        {
            selectedCarIndex = 0;
        }

        StartCoroutine(SpawnCarCO(false));
    }

    public void OnSelectCar(int player)
    {
        PlayerPrefs.SetInt($"P{player}SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.Save();
    }



    IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide)
    {
        isChangingCar = true;
        if(carUIHandler != null)
            carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);

        GameObject instantiateCar = Instantiate(carPrefab, spawnOnTransform);

        carUIHandler = instantiateCar.GetComponent<CarUIHandler>();
        carUIHandler.SetupCar(carDatas[selectedCarIndex]);
        carUIHandler.StartCarEntranceAnimation(isCarAppearingOnRightSide);
        
        yield return new WaitForSeconds(0.4f);
        isChangingCar = false;
    }
}
