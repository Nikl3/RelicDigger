using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour {

    public float boneClickScore = 500f;
    float totalScore;

    public TextMeshProUGUI scoreText;
    LayerMask background;

    void UpdateTotalScore() {
        scoreText.text = "Score: " + totalScore;
    }

	void Start () {
        background = LayerMask.GetMask("background");

	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount < 0) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, background)) {
                totalScore += boneClickScore;
                UpdateTotalScore();
            }
        }



    }
}
