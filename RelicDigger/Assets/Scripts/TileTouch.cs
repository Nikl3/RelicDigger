using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTouch : MonoBehaviour {

    Vector2 touchStart;
    int layerMask;
    public float fingerSize;
    public string brushAudio;

    void Start() {
        layerMask = LayerMask.GetMask("Tile");
    }


    void Update() {

        if (Input.touchCount == 1){

            
            Touch touchZero = Input.GetTouch(0);

            if (touchZero.phase == TouchPhase.Began){
                
            }


            if (touchZero.phase == TouchPhase.Moved){
                Fabric.EventManager.Instance.PostEvent(brushAudio);
                Vector2 hitSpot = Camera.main.ScreenToWorldPoint((Vector2)touchZero.position);
                Collisions(hitSpot);
            }
        }
    }

    void Collisions(Vector2 spot) {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spot, fingerSize, layerMask);
        int i = 0;
        while (i < hitColliders.Length){
            hitColliders[i].gameObject.SetActive(false);
            i++;
        }
    }
}





