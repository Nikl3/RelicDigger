using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loop : MonoBehaviour {

    public string loopAudio;
    public string stopAudio;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) {
            Fabric.EventManager.Instance.PostEvent(loopAudio);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Fabric.EventManager.Instance.PostEvent(stopAudio);
        }
    }
}
