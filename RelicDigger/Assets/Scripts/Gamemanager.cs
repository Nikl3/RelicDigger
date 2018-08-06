using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI statusText;
    public GameObject timesupScreen;
    public GameObject tutorialTable;

    public float SkeletonTimer;
    public string showSkeleton;
    public GameObject paleSkeleton;
    public Transform bonePos;
    public GameObject[] bones;
    Vector3[] OrginalPos;
    Dictionary<GameObject, Vector3> originalPos;
    public Animator skeletonAnimator;
    public List<GameObject> bonesFound;
    bool tutorialSeen = false;
    int tutorialTextIndex = 0;

    public bool gameOver;
    LayerMask boneLayer;
    LayerMask tileLayer;


    void Start() {
        Fabric.EventManager.Instance.PostEvent(bgmAudio);
        float firstX = (tileColumns / -2f + 0.5f) * tileSize.x;
        float firstY = (tileRows / -2f + 0.5f) * tileSize.y;

        for (int i = 0; i < tileColumns; i++) {
            for (int j = 0; j < tileRows; j++) {

                var prefab = tilePrefab[Random.Range(0, tilePrefab.Count)];
                Vector3 newPos = new Vector3(firstX + i * tileSize.x, firstY + j * tileSize.y, 0 /*Random.Range(-0.2f,-0.4f)*/);
                GameObject sandTile = Instantiate(prefab);
                sandTile.transform.position = newPos;
                sandTile.transform.parent = tileFolder.transform;
            }
        }

        boneLayer = LayerMask.GetMask("Bone");
        tileLayer = LayerMask.GetMask("Tile");

        bones = GameObject.FindGameObjectsWithTag("bone");

        OrginalPos = new Vector3[bones.Length];
        originalPos = new Dictionary<GameObject, Vector3>();
        skeletonAnimator.Play("EndStateClip");

        // same as below
        //for (int i=0; i < bones.Length; i++) {
        //    originalPos.Add(bones[i], bones[i].transform.position);
        //    //OrginalPos[i] = bones[i].transform.position; //asettaa kaikkien orginalPos talteen
        //    bones[i].transform.position = new Vector3(Random.Range(-3, 3),0 , Random.Range(-5,5));
        //}


        foreach (GameObject bone in bones) {
            originalPos.Add(bone, bone.transform.position);
            bone.transform.position = new Vector3(GoodRandom(-4, 6), GoodRandom(-6, 7), 0);
        }
    }



    void Update() {

        if (!tutorialSeen) {
            if (tutorialTextIndex == 0) {
                statusText.text = "Hello\n You wannabe Indiana!\n\nYour job is to find some dinosaur bones.";
                Time.timeScale = 0;
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    tutorialTextIndex = 1;
                }
            } else if (tutorialTextIndex == 1) {
                statusText.text = "But be careful and clear the dust by swiping before picking up the bones or otherwise they will break!\n\nGood Luck!";
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    tutorialSeen = true;
                    tutorialTable.SetActive(false);
                    statusText.text = "";
                    Time.timeScale = 1;
                }
            }

        } else if (Input.GetKeyDown(KeyCode.Mouse0) /*|| Input.touchCount > 0*/) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, boneLayer)) {

                if (energy > 0) {

                    if (Physics2D.OverlapPoint(ray.origin, tileLayer)) {
                        print("osui hiekkaan");
                        Fabric.EventManager.Instance.PostEvent(boneAudio);
                        totalScore -= boneClickScore;
                        UpdateTotalScore();
                        GameObject go = hit.collider.gameObject;
                        go.SetActive(false);
                    } else {


                        Fabric.EventManager.Instance.PostEvent(boneAudio);
                        totalScore += boneClickScore;
                        UpdateTotalScore();
                        //hit.collider.transform.position = bonePos.position;
                        GameObject go = hit.collider.gameObject;
                        go.transform.DetachChildren();
                        go.transform.position = originalPos[hit.collider.gameObject] + new Vector3(0, 0, 1);
                        bonesFound.Add(go);
                        go.GetComponent<BoxCollider>().enabled = false;
                        go.GetComponent<SpriteRenderer>().sortingOrder = 5;
                        ShowSkeleton();
                        //Destroy(hit.collider.gameObject);

                    }
                }
            }
        }

        if (energy > 0) {
            energy -= Time.deltaTime;
            energyText.text = "energy " + energy.ToString("f0") + "%";
        } else {
            if (!gameOver) {
                Fabric.EventManager.Instance.PostEvent(stopAudio);
                Fabric.EventManager.Instance.PostEvent(failAudio);
                gameOver = true;
                statusText.text = "Game over! Your score was: " + totalScore;
                tutorialTable.SetActive(true);
                Time.timeScale = 0;

          
            } else {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    tutorialTable.SetActive(false);
                    statusText.text = "";
                    totalScore = 0;
                    SceneManager.LoadScene("Scene_Final");
                    Time.timeScale = 1;
                }
            }
            //timesupScreen.SetActive(true);
            //times up screen.set active
        }

    }

    public void EnergyEvent(float energyInfo) {

        energy += energyInfo;

        if (energy > 100) {
            energy = 100;
        }
    }

    float GoodRandom(float min, float max) {
        float average = (min + max) / 2;
        float halfDistance = (max - min) / 2;
        float rng = 1 - Mathf.Pow(Random.value, 2f);
        return average + (Random.value < 0.5f ? -1 : 1) * rng * halfDistance;

    }

    void UpdateTotalScore() {
        scoreText.text = "score " + totalScore;
    }

    void ShowSkeleton() {
        //paleSkeleton.SetActive(true);
        //if (SkeletonTimer < 0) {
        //}

        skeletonAnimator.Play("boneGlow");

        foreach (GameObject bone in bonesFound) {
            bone.GetComponent<Animator>().Play("boneGlow");
        }
        //print("skeleton näkyi");
    }
}
