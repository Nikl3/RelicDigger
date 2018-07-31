using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour {


    public float score;
    public GameObject sandTilePrefab;
    public Vector2 tileSize;
    public int tileRows;
    public int tileColumns;

    public float boneClickScore = 500f;
    float totalScore;

    public TextMeshProUGUI scoreText;

    LayerMask background;
    RoundTimer rt;
    public GameObject[] bones;

    void UpdateTotalScore() {
        scoreText.text = "score " + totalScore;
    }

	void Start () {

       
        float firstX = (tileColumns / -2f + 0.5f) * tileSize.x;
        float firstY = (tileRows / -2f + 0.5f) * tileSize.y;

        for(int i = 0; i < tileColumns; i++) {
            for (int j = 0; j < tileRows; j++) {
                GameObject sandTile = Instantiate(sandTilePrefab);
                Vector3 newPos = new Vector3(firstX + i * tileSize.x, firstY + j * tileSize.y, 0.3f);
                sandTile.transform.position = newPos;
            }
        }

        background = LayerMask.GetMask("background");
        bones = GameObject.FindGameObjectsWithTag("bone");
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
