using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour {

    float totalScore;
    public float score;
    public float boneClickScore = 500f;
    public float energy = 100;

    public List<GameObject> tilePrefab;
    public GameObject tileFolder;
    public Vector2 tileSize;
    public int tileRows;
    public int tileColumns;

    public string bgmAudio;
    public string boneAudio;
    public string stopAudio;
    public string failAudio;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI energyText;
    public GameObject timesupScreen;

    public float SkeletonTimer;
    public string showSkeleton;
    public GameObject paleSkeleton;
    public Transform bonePos;
    public GameObject[] bones;
    Vector3[] OrginalPos;
    Dictionary<GameObject, Vector3> originalPos;
    Animator skeletonAnimator;

    public bool gameOver;
    LayerMask background;

    void Start(){
        Fabric.EventManager.Instance.PostEvent(bgmAudio);
        float firstX = (tileColumns / -2f + 0.5f) * tileSize.x;
        float firstY = (tileRows / -2f + 0.5f) * tileSize.y;

        for (int i = 0; i < tileColumns; i++){
            for (int j = 0; j < tileRows; j++){

                var prefab = tilePrefab[Random.Range(0, tilePrefab.Count)];
                Vector3 newPos = new Vector3(firstX + i * tileSize.x, firstY + j * tileSize.y,0 /*Random.Range(-0.2f,-0.4f)*/);
                GameObject sandTile = Instantiate(prefab);
                sandTile.transform.position = newPos;
                sandTile.transform.parent = tileFolder.transform;
            }
        }

        background = LayerMask.GetMask("background");
        bones = GameObject.FindGameObjectsWithTag("bone");
        OrginalPos = new Vector3[bones.Length];
        originalPos = new Dictionary<GameObject, Vector3>();
        skeletonAnimator = FindObjectOfType<Animator>();

        // same as below
        //for (int i=0; i < bones.Length; i++) {
        //    originalPos.Add(bones[i], bones[i].transform.position);
        //    //OrginalPos[i] = bones[i].transform.position; //asettaa kaikkien orginalPos talteen
        //    bones[i].transform.position = new Vector3(Random.Range(-3, 3),0 , Random.Range(-5,5));
        //}


        foreach (GameObject bone in bones)
        {
            originalPos.Add(bone, bone.transform.position);
            bone.transform.position = new Vector3(GoodRandom(-4, 6), GoodRandom(-6, 7), 0);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) /*|| Input.touchCount > 0*/)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, background))
            {
                if (energy > 0)
                {
                    Fabric.EventManager.Instance.PostEvent(boneAudio);
                    totalScore += boneClickScore;
                    UpdateTotalScore();
                    hit.collider.transform.DetachChildren();
                    hit.collider.transform.position = originalPos[hit.collider.gameObject] + new Vector3(0, 0, 1);
                    hit.collider.transform.SetParent(paleSkeleton.transform, true);

                    // hit.collider.GetComponent<Animator>().enabled = true;
                    ShowSkeleton();

                    GameObject go = hit.collider.gameObject;
                    go.GetComponent<BoxCollider>().enabled = false;
                    go.GetComponent<SpriteRenderer>().sortingOrder = 5;

                    //Destroy(hit.collider.gameObject);
                }
            }
        }

        if (energy > 0){
            energy -= Time.deltaTime;
            energyText.text = "energy " + energy.ToString("f0") + "%";
        }
        if (energy <= 0){
            if (!gameOver)
            {
                Fabric.EventManager.Instance.PostEvent(stopAudio);
                Fabric.EventManager.Instance.PostEvent(failAudio);
                gameOver = true;
            }
            timesupScreen.SetActive(true);
            //times up screen.set active
        }

    }

    public void EnergyEvent(float energyInfo){
        
        energy += energyInfo;

        if (energy > 100){
            energy = 100;
        }
    }

    float GoodRandom(float min, float max){
        float average = (min + max) / 2;
        float halfDistance = (max - min) / 2;
        float rng = 1 - Mathf.Pow(Random.value, 2f);
        return average + (Random.value < 0.5f ? -1 : 1) * rng * halfDistance;

    }

    void UpdateTotalScore(){
        scoreText.text = "Score " + totalScore;
    }

    void ShowSkeleton(){
        //paleSkeleton.SetActive(true);
        //if (SkeletonTimer < 0) {
        //}
        skeletonAnimator.Play("boneGlow");
        foreach (Animator animator in skeletonAnimator.GetComponentsInChildren<Animator>()) {
            animator.Play("boneGlow");
        }
    }

}
