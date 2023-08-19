using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    private void Start()
    {
        if (DataManager.instance.gameData.sceneIndex == SceneManager.GetActiveScene().buildIndex && DataManager.instance.gameData.respawnPoint == (Vector2) transform.position)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //SceneController.instance.Save(transform.position);
            DataManager.instance.SaveGameData(transform.position);
            gameObject.SetActive(false);
        }
    }
}
