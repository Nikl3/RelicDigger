using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject pauseimage;

    public float score;


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

    public void ScoreClick() {
        // add score
    }

    private void Update() {
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(/*Input.touches[0].position ||*/ Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                print("jeba");
                // ScoreClick();
            }
        }
    }

}
//void MovePlayerRay(Vector3 rayVector) {
//    Ray ray = Camera.main.ScreenPointToRay(rayVector);
//    RaycastHit hit;

//    if (Physics.Raycast(ray, out hit)) {
//        transform.position = hit.point;
//    }
//}