using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject pauseimage;

	public void PauseActivated() {
        pauseimage.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PauseDeactivated() {
        pauseimage.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TapContinue() {
        if (Input.touchCount > 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


    void Update() {

    }

}
