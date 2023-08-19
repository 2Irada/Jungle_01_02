using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    //public SaveData data;

    public bool isRestart;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //if (data.sceneIndex != SceneManager.GetActiveScene().buildIndex) ColorManager.instance.SwitchMainColoring(Coloring.Red);
        //else Load();
        isRestart = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isRestart)
        {
            StartCoroutine(FindObjectOfType<PlayerController>().DeathCoroutine());
        }
        else if (Input.GetKeyDown(KeyCode.F11)) 
        {
            ResetSavedata();
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ResetSavedata()
    {
        DataManager.instance.gameData.sceneIndex = -1;
        DataManager.instance.gameData.deathCount = 0;
    }

    public void NextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings - 1 > SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}