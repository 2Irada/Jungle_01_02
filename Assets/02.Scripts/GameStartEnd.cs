using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartEnd : MonoBehaviour
{

    public void loadNextScene()
    {
        //SceneManager.LoadScene("Stage1");
        DataManager.instance.ResetJson();
        SceneManager.LoadScene(DataManager.instance.gameData.sceneIndex);

    }

    public void loadStage(int value)
    {
        DataManager.instance.ResetJson();
        SceneManager.LoadScene("Stage"+value);
    }

    // Update is called once per frame
    public void gameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif       
    }
}