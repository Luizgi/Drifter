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

    CarData[] carDatas;
    int selectedCarIndex = 0;

    [Header("Controls")]
    [SerializeField] KeyCode Left = KeyCode.A;
    [SerializeField] KeyCode Right = KeyCode.D;
    [SerializeField] KeyCode Enter = KeyCode.Space;
    

    private void Start()
    {
        carDatas = Resources.LoadAll<CarData>("CarData/");
        StartCoroutine(SpawnCarCO(true));
    }

    private void Update()
    {

        #region Player1
        if (Input.GetKeyDown(Left))
        {
            OnPreviousCar();
        }
        else if(Input.GetKeyDown(Right))
        {
            OnNextCar();
        }
        if (Input.GetKeyDown(Enter))
        {
            OnSelectCar(1);
        }
        #endregion
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
        PlayerPrefs.SetInt($"P{player}SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt($"P{player}SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt($"P{player}SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
      
        PlayerPrefs.Save();

        SceneManager.LoadScene("Multiplayer");
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
