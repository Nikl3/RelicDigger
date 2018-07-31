using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {

    Vector3 touchStart;
    
    public GameObject digMask;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    
    void Update()
    {

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (touchOne.phase == TouchPhase.Began) {
                touchStart = Camera.main.ScreenToWorldPoint((Vector3)(touchZero.position + touchOne.position) / 2);
            }

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;

            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            //if (Mathf.Abs(difference) > 100)
            //{
            //    zoom(difference * 0.01f);
            //}
            

            //if (touchOne.phase == TouchPhase.Moved) {
            //    GameObject DiggedMask = Instantiate(digMask);
            //    DiggedMask.transform.position = ((Vector3)(touchZero.position + touchOne.position) / 2);
            //}

            

        } else if (Input.touchCount == 1) {

            Touch touchZero = Input.GetTouch(0);

            if (touchZero.phase == TouchPhase.Began) {
                touchStart = Camera.main.ScreenToWorldPoint((Vector3)touchZero.position);
            }

            if (touchZero.phase == TouchPhase.Moved)
            {
                GameObject DiggedMask = Instantiate(digMask);
                DiggedMask.transform.position = Camera.main.ScreenToWorldPoint((Vector3)touchZero.position) + 0.7f * Vector3.down;
            }

            //Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(touchZero.position);
            //Camera.main.transform.position += direction;

        } else if (Input.GetKey(KeyCode.Mouse0)){
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            GameObject DiggedMask = Instantiate(digMask);
            DiggedMask.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + 0.7f * Vector3.down;
            
        }



        
    }

    //void zoom(float increment)
    //{
    //    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    //}
}