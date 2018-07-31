﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public string stopAudio;
    public string gameAudio;
    public string pauseAudio;

    public GameObject pauseimage;

	public void PauseActivated() {
        Fabric.EventManager.Instance.PostEvent(stopAudio);
        Fabric.EventManager.Instance.PostEvent(pauseAudio);
        pauseimage.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PauseDeactivated() {
        Fabric.EventManager.Instance.PostEvent(stopAudio);
        Fabric.EventManager.Instance.PostEvent(gameAudio);
        pauseimage.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RetryLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu() {
        SceneManager.LoadScene(0);
    }

    public void TapContinue() {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void Start() {
        Fabric.EventManager.Instance.PostEvent(gameAudio);

    }

    void Update() {

    }

}
