using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    public int manyPlayer;
    [SerializeField] SelectCarUIHandler selectCarUIHandler;
    public bool hasAll = false;
    [HideInInspector] public bool hasP1;
    [HideInInspector] public bool hasP2;
    [HideInInspector] public bool hasP3;

    private void Update()
    {
        /*
        Debug.Log("Todas" + hasAll);
        Debug.Log("Player 1" + hasP1);
        Debug.Log("Player 2" + hasP2);
        */

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
    public void StartGame()
    {

        if (hasAll == true && manyPlayer == 2)
        {
            SceneManager.LoadScene("2Players");
        }
        else if(hasAll == true &&manyPlayer == 3)
        {
            //SceneManager.LoadScene("3Players");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ManyPlayer(int choosedManyPlayer)
    {
        manyPlayer = choosedManyPlayer;
    }

    public void MultiplayerScene()
    {
        SceneManager.LoadScene(1);
    }
}
