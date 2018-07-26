using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject pauseimage;

	public void PauseActivated() {
        pauseimage.SetActive(true);
        Time.timeScale = 0f;
    }

	void Update () {
	
	}
}
