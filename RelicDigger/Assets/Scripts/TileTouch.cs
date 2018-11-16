using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTouch : MonoBehaviour {

    Vector2 touchStart;
    Vector2 prevPos;
    Vector2 currentPos;
    public Gamemanager gm;
    public float eConsPerDig = -0.05f;
    public float eAddedPerItem = 10f;
    public static float fingerSize = 0.5f;
    public string airLoopAudio;
    public string stopLoopAudio;
    int layerMaskTile;
    int layerMaskEnergy;

    void Start() {
        layerMaskTile = LayerMask.GetMask("Tile");
        layerMaskEnergy = LayerMask.GetMask("Energy");
    }

    void Update() {

        if (Input.touchCount == 1 && gm.waitTimer < 0 && gm.tutorialSeen){
            Touch touchZero = Input.GetTouch(0);

            if (touchZero.phase == TouchPhase.Began){
                prevPos = Camera.main.ScreenToWorldPoint((Vector2)touchZero.position);
            }

            if (touchZero.phase == TouchPhase.Moved){
                Fabric.EventManager.Instance.PostEvent(airLoopAudio);
                Vector2 hitSpot = Camera.main.ScreenToWorldPoint((Vector2)touchZero.position);
                Swipe(hitSpot);

                if ((hitSpot - prevPos).magnitude > fingerSize){
                    FastSwipe(prevPos, hitSpot);
                }

                prevPos = Camera.main.ScreenToWorldPoint((Vector2)touchZero.position);
            }

            if (touchZero.phase == TouchPhase.Ended){
                Fabric.EventManager.Instance.PostEvent(stopLoopAudio);
            }
        }
    }

    //TODO: Siisti Swipet alta

    void Swipe(Vector2 spot) {

        Collider2D[] hitCollidersTile = Physics2D.OverlapCircleAll(spot, fingerSize, layerMaskTile);
        int i = 0;
        while (i < hitCollidersTile.Length){
            hitCollidersTile[i].gameObject.SetActive(false);
            i++;
            gm.EnergyEvent(eConsPerDig);
        }

        Collider2D[] hitCollidersEnergy= Physics2D.OverlapCircleAll(spot, fingerSize, layerMaskEnergy);
        int j = 0;
        while (j < hitCollidersEnergy.Length){
            hitCollidersEnergy[j].gameObject.SetActive(false);
            j++;
            gm.EnergyEvent(eAddedPerItem);
        }
    }

    void FastSwipe(Vector2 beginSwipe, Vector2 endSwipe){
        float angle = Mathf.Sign((endSwipe - beginSwipe).y) * Vector2.Angle(endSwipe - beginSwipe, Vector2.right);
        Collider2D[] hitCollidersTile = Physics2D.OverlapBoxAll((beginSwipe + endSwipe) / 2, new Vector2(Vector2.Distance(beginSwipe, endSwipe), fingerSize), angle, layerMaskTile);
        int i = 0;
        while (i < hitCollidersTile.Length){
            hitCollidersTile[i].gameObject.SetActive(false);
            i++;      
            gm.EnergyEvent(eConsPerDig);
        }

        Collider2D[] hitCollidersEnergy = Physics2D.OverlapBoxAll((beginSwipe + endSwipe) / 2, new Vector2(Vector2.Distance(beginSwipe, endSwipe), fingerSize), angle, layerMaskEnergy);
        int j = 0;
        while (j < hitCollidersEnergy.Length)
        {
            hitCollidersEnergy[j].gameObject.SetActive(false);
            j++;
            gm.EnergyEvent(eAddedPerItem);
        }
    }

}





