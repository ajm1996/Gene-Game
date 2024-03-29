using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start() {
        FindObjectOfType<AudioManager>().Play("MainMusic");
    }
  
    public void PlayGame () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {

        Debug.Log("Player pressed QUIT");
        Application.Quit();
    }

}
