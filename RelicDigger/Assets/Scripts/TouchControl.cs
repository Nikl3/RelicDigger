using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {
    public Camera cam;
    float zoomSpeed = 0.01f;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            Vector2 twoFingerDeltaPos = (touchOne.deltaPosition + touchZero.deltaPosition) / 2;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            //cam.orthographicSize += deltaMagnitudeDiff * zoomSpeed;
            //cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
            
            cam.transform.position -= cam.ScreenToWorldPoint(twoFingerDeltaPos);         
        }
    }
}
