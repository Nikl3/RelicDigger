using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RoundTimer : MonoBehaviour {

    public float timer;
    TextMeshProUGUI timerText;
    public GameObject timesupScreen;

	void Start () {
        timerText = GetComponent<TextMeshProUGUI>();
	}
	
	void Update () {
        if (timer > 0) {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("f0");
        }
        if (timer <= 0) {
            timesupScreen.SetActive(true);
            //times up screen.set active
        }
	}
}
