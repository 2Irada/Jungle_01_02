using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartEnd : MonoBehaviour
{
    
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Ending") StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(0);
    }

    public void loadNextScene()
    {
        //SceneManager.LoadScene("Stage1");
        DataManager.instance.ResetJson();
        SceneManager.LoadScene("Stage"+DataManager.instance.gameData.sceneIndex);

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