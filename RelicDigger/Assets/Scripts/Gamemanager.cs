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
    public int boneUpdate = 0;
    public int bonesBroken = 0;

    public string bgmAudio;
    public string boneAudio;
    public string stopAudio;
    public string failAudio;
    public string winAudio;
    public string destroyAudio;
    public string startAudio;
    public string boopAudio;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI boneText;
    public GameObject timesupScreen;
    public GameObject tutorialTable;
    public GameObject pauseButton;

    public float SkeletonTimer;
    public string showSkeleton;
    public GameObject paleSkeleton;
    public Transform bonePos;
    public GameObject[] bones;
    Vector3[] OrginalPos;
    Dictionary<GameObject, Vector3> originalPos;
    public Animator skeletonAnimator;
    public List<GameObject> bonesFound;
    public bool tutorialSeen = false;
    int tutorialTextIndex = 0;

    public bool gameLost = false;
    public bool gameWon = false;
    public bool gameOver = false;
    bool startSoundPlayed = false;
    public float waitTimer = 0.5f;
    public float counterTimer;
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

        waitTimer -= Time.fixedUnscaledDeltaTime;
        counterTimer -= Time.unscaledDeltaTime;
        if (!tutorialSeen) {
            if (tutorialTextIndex == 0) {
                statusText.text = "Hello\n You wannabe Indiana!\n\nYour job is to find some dinosaur bones.";
                Time.timeScale = 0;               
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    tutorialTextIndex = 1;
                    ResetWaitTimer();
                }
            } else if (tutorialTextIndex == 1) {
                statusText.text = "But be careful and clear the dust by swiping before picking up the bones or otherwise they will break!\n\nGood Luck!";
                if (Input.GetKeyDown(KeyCode.Mouse0) && waitTimer < 0) {

                    tutorialTable.SetActive(false);
                    pauseButton.SetActive(true);
                    statusText.text = "";
                    tutorialTextIndex = 2;
                    counterTimer = 3.49f;
                    waitTimer = 0.5f;
                }
            } else if (tutorialTextIndex == 2){
                if (counterTimer >= 3.49f) {
                    //Fabric.EventManager.Instance.PostEvent(boopAudio);
                    counterText.text = "";
                } else if (counterTimer >= 0.51f) {
                    counterText.text = counterTimer.ToString("f0");
                } else if (counterTimer >= -1f) {
                    counterText.text = "Start Diggin'!";
                    if (!startSoundPlayed) {
                        startSoundPlayed = true;
                        Fabric.EventManager.Instance.PostEvent(startAudio);
                    }
                } else {
                    tutorialSeen = true;
                    counterText.text = "";
                    Time.timeScale = 1;
                }
            }



        } else if (Input.GetKeyDown(KeyCode.Mouse0) && counterTimer <= 0) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, boneLayer)) {

                if (energy > 0) {

                    if (Physics2D.OverlapPoint(ray.origin, tileLayer)) {
                        print("osui hiekkaan");
                        Fabric.EventManager.Instance.PostEvent(destroyAudio);
                        totalScore -= boneClickScore;
                        UpdateTotalScore();
                        GameObject go = hit.collider.gameObject;
                        go.transform.DetachChildren();
                        go.SetActive(false);
                        boneUpdate++;
                        bonesBroken++;
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
                        boneUpdate++;
                        //Destroy(hit.collider.gameObject);

                    }
                }
            }
        }



        if (energy > 0) {
            energy -= Time.deltaTime;
            energyText.text = "energy " + energy.ToString("f0") + "%";
            boneText.text = "Bones left:" + (10 - boneUpdate) + "\nBroken:" + (bonesBroken);
        }

        if (boneUpdate == 10 || energy <= 0) {

            if (energy <= 0) {
                gameLost = true;
            }

            if (bones.Length == bonesFound.Count) {
                gameWon = true;
            } else gameLost = true;
        }

        if (gameLost && !gameOver) {
            Fabric.EventManager.Instance.PostEvent(stopAudio);
            Fabric.EventManager.Instance.PostEvent(failAudio);
            gameOver = true;
            if (bonesBroken <= 0) {
                statusText.text = ("Game over!\nYour score was: " + totalScore + "\nand you found\n" + bonesFound.Count + " bones out of " + bones.Length + "!" + "\n\nTap to continue");
            } else {
                statusText.text = ("Game over!\nYour score was: " + totalScore + "\nand you found\n" + bonesFound.Count + " bones out of " + bones.Length + "\nbut broke " + bonesBroken + "!" + "\n\nTap to continue");
            }
            tutorialTable.SetActive(true);
            pauseButton.SetActive(false);
            ResetWaitTimer();
            Time.timeScale = 0;
        }

        if (gameWon && !gameOver) {
            Fabric.EventManager.Instance.PostEvent(stopAudio);
            Fabric.EventManager.Instance.PostEvent(winAudio);
            statusText.text = ("You won!\nYour score was: " + totalScore + "\nand you found\n" + bonesFound.Count + " bones out of " + bones.Length + "!" + "\n\nTap to continue");
            tutorialTable.SetActive(true);
            pauseButton.SetActive(false);
            gameOver = true;
            ResetWaitTimer();
            print("Wait timer reset");
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && waitTimer < 0 && gameOver) {
            Fabric.EventManager.Instance.PostEvent(stopAudio);
            tutorialTable.SetActive(false);
            statusText.text = "";
            totalScore = 0;
            SceneManager.LoadScene("Scene_Menu");
            Time.timeScale = 1;
        }
        
        //timesupScreen.SetActive(true);
        //times up screen.set active


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

    void ResetWaitTimer()
    {
        waitTimer = 0.5f;
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
