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
    KeyCode Left = KeyCode.A;
    KeyCode Right = KeyCode.D;
    [SerializeField] string Player;
    

    private void Start()
    {
        if(Player == "1")
        {
            Left = KeyCode.A;
            Right = KeyCode.D;
        }
        else if(Player == "2")
        {
            Left = KeyCode.LeftArrow;
            Right = KeyCode.RightArrow;
        }
        else if(Player == "3")
        {
            Left = KeyCode.J;
            Right = KeyCode.L;
        }
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
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnSelectP1();
            manager.hasP1 = true;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnSelectP2();
            manager.hasP2 = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnSelectP3();
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

    public void OnSelectP1()
    {
        PlayerPrefs.SetInt("P1SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.Save();
        Debug.Log(("Player 1: ") + PlayerPrefs.GetInt("P1SelectedCarID"));
    }

    public void OnSelectP2()
    {
        PlayerPrefs.SetInt("P2SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.Save();
        Debug.Log(("Player 2: ") + PlayerPrefs.GetInt("P2SelectedCarID"));
    }

    public void OnSelectP3()
    {
        PlayerPrefs.SetInt("P3SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.Save();

        Debug.Log(("Player 3: ") + PlayerPrefs.GetInt("P3SelectedCarID"));
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
        
        yield return new WaitForSeconds(0.3f);
        isChangingCar = false;
    }
}
