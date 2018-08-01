using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RoundTimer : MonoBehaviour {

    public float timer;
    TextMeshProUGUI timerText;
    public GameObject timesupScreen;
    public string stopAudio;
    public string failAudio;
    public bool gameOver;

	void Start () {
        timerText = GetComponent<TextMeshProUGUI>();
	}
	
	void Update () {
        if (timer > 0) {
            timer -= Time.deltaTime;
            timerText.text = "Energyy " + timer.ToString("f0") + "%";
        }
        if (timer <= 0) {
            if (!gameOver) {
                Fabric.EventManager.Instance.PostEvent(stopAudio);
                Fabric.EventManager.Instance.PostEvent(failAudio);
                gameOver = true;
            }
                timesupScreen.SetActive(true);
            //times up screen.set active
        }
	}
}
