using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    private Manager instance;
    public int manyPlayer;
    [SerializeField] SelectCarUIHandler selectCarUIHandler;
    public bool hasAll = false;
    [HideInInspector] public bool hasP1;
    [HideInInspector] public bool hasP2;
    [HideInInspector] public bool hasP3;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log("Todas" + hasAll);
        Debug.Log("Player 1" + hasP1);
        Debug.Log("Player 2" + hasP2);

        if (manyPlayer == 2)
        {
            if(hasP1 && hasP2)
            {
                hasAll = true;
            }
        }
        else if (manyPlayer == 3)
        {
            if(hasP1 && hasP2 && hasP3)
            {
                hasAll = true;
            }
        }
    }
    #region UIMANAGER
    public void StartGame()
    {

        if (hasAll == true)
        {
            SceneManager.LoadScene("Multiplayer");
        }
    }
    public void ManyPlayer(int choosedManyPlayer)
    {
        manyPlayer = choosedManyPlayer;
    }

    public void MultiplayerScene()
    {
        SceneManager.LoadScene(1);
    }
    #endregion
}
