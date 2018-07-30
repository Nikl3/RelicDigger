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
    RoundTimer rt;

    void UpdateTotalScore() {
        scoreText.text = "Score: " + totalScore;
    }

	void Start () {
        background = LayerMask.GetMask("background");
        rt = FindObjectOfType<RoundTimer>();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, background)) {
                if (rt.timer > 0) {
                    totalScore += boneClickScore;
                    UpdateTotalScore();
                    Destroy(hit.collider.gameObject);
                }
                }
        }



    }
}
