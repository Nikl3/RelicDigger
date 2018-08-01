using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTouch : MonoBehaviour {

    Vector2 touchStart;
    Vector2 prevPos;
    Vector2 currentPos;
    public Gamemanager gm;
    public float eConsPerDig = -0.1f;
    int layerMask;
    public float fingerSize;
    public string airLoopAudio;
    public string stopLoopAudio;

    void Start() {
        layerMask = LayerMask.GetMask("Tile");
    }


    void Update() {

        if (Input.touchCount == 1){

            
            Touch touchZero = Input.GetTouch(0);

            if (touchZero.phase == TouchPhase.Began){
                prevPos = Camera.main.ScreenToWorldPoint((Vector2)touchZero.position);
            }


            if (touchZero.phase == TouchPhase.Moved){


                Fabric.EventManager.Instance.PostEvent(airLoopAudio);

                //jos kosketusdelta isompi kuin sormi, niin piirrä sormenlevyinen overlap boxi niiden väliin
                
                Vector2 hitSpot = Camera.main.ScreenToWorldPoint((Vector2)touchZero.position);
                Swipe(hitSpot);
                if ((hitSpot - prevPos).magnitude > fingerSize){
                    FastSwipe(prevPos, hitSpot);
                }

                prevPos = Camera.main.ScreenToWorldPoint((Vector2)touchZero.position);
            }
            if (touchZero.phase == TouchPhase.Ended)
            {
                Fabric.EventManager.Instance.PostEvent(stopLoopAudio);
            }
        }
    }

    void Swipe(Vector2 spot) {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spot, fingerSize, layerMask);
        int i = 0;
        while (i < hitColliders.Length){
            hitColliders[i].gameObject.SetActive(false);
            i++;
            gm.EnergyEvent(eConsPerDig);
        }
    }

    void FastSwipe(Vector2 beginSwipe, Vector2 endSwipe){
        float angle = Mathf.Sign((endSwipe - beginSwipe).y) * Vector2.Angle(endSwipe - beginSwipe, Vector2.right);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll((beginSwipe + endSwipe) / 2, new Vector2(Vector2.Distance(beginSwipe, endSwipe), fingerSize), angle);
        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].gameObject.SetActive(false);
            i++;
            
            gm.EnergyEvent(eConsPerDig);
        }
    }

}





