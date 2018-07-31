using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTouch : MonoBehaviour {

    Vector2 touchStart;
    int layerMask;
    public float fingerSize;

    void Start() {
        layerMask = LayerMask.GetMask("Tile");
    }


    void Update() {

        if (Input.touchCount == 1){

            Touch touchZero = Input.GetTouch(0);

            if (touchZero.phase == TouchPhase.Moved){
                Vector2 hitSpot = Camera.main.ScreenToWorldPoint((Vector2)touchZero.position);
                Collisions(hitSpot);
            }
        }
    }

    void Collisions(Vector2 spot){

        Collider2D hitColliders = Physics2D.OverlapCircle(spot, fingerSize, layerMask);

        hitColliders.gameObject.SetActive(false);  
    }
}


