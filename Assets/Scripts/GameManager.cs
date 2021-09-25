using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    //making sure you have one game manager for different levels 
    public static GameManager singleton;
    public GroundPiece[] allGroundPieces;
   

    void Start()
    {

        SetupNewLevels();
    }

    private void SetupNewLevels()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }
    // awake method is called just before the awake method
    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        } else if (singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    //method for moving to the next level
    private void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode)
    {
        SetupNewLevels();

    }
    public void CheckComplete()
    {
        bool isFinished = true;
        for(int i = 0; i<allGroundPieces.Length; i++)
        {
            if(allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }
        if (isFinished)
        {
            //Next level
            NextLevel();
            
        }
    }
    //loads scene
    private void NextLevel()
    {
        //check if the player has reached number of levels
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            //in that case load scene 0 which is level 1
            SceneManager.LoadScene(0);
        }
        else
        {
            //go to next level

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        //go to next level

       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
