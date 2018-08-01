using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour {


    public float score;

    public Vector2 tileSize;
    public int tileRows;
    public int tileColumns;
    public List<GameObject> tilePrefab;
    public float boneClickScore = 500f;
    float totalScore;
    public string bgmAudio;
    public string boneAudio;

    public TextMeshProUGUI scoreText;

    LayerMask background;
    RoundTimer rt;
    public Transform bonePos;
    public GameObject[] bones;
    Vector3[] OrginalPos;
    Dictionary<GameObject, Vector3> originalPos;
    public GameObject paleSkeleton;
    public float SkeletonTimer;
    Animator skeletonAnimator;
    public string showSkeleton;

    void UpdateTotalScore() {
        scoreText.text = "Score " + totalScore;
    }

    void ShowSkeleton() {
        //paleSkeleton.SetActive(true);
        //if (SkeletonTimer < 0) {
        //}

        skeletonAnimator.Play("boneGlow");
        //print("skeleton näkyi");
    }







    void Start() {
        Fabric.EventManager.Instance.PostEvent(bgmAudio);
        float firstX = (tileColumns / -2f + 0.5f) * tileSize.x;
        float firstY = (tileRows / -2f + 0.5f) * tileSize.y;

        for (int i = 0; i < tileColumns; i++)
        {
            for (int j = 0; j < tileRows; j++)
            {
                var prefab = tilePrefab[Random.Range(0, tilePrefab.Count)];
                Vector3 newPos = new Vector3(firstX + i * tileSize.x, firstY + j * tileSize.y, Random.Range(0.2f,0.5f));
                GameObject sandTile = Instantiate(prefab);

                //if (prefab == tilePrefab[tilePrefab.Count])
                //{
                //    newPos.z = 0.15f;
                //}
                    sandTile.transform.position = newPos;
            }
        }

        background = LayerMask.GetMask("background");
        bones = GameObject.FindGameObjectsWithTag("bone");
        rt = FindObjectOfType<RoundTimer>();
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
            bone.transform.position = new Vector3(GoodRandom(-4, 6), 0.12f, GoodRandom(-6, 7));
        }
    }

    float GoodRandom(float min, float max) {
        float average = (min + max) / 2;
        float halfDistance = (max - min) / 2;
        float rng = 1 - Mathf.Pow(Random.value, 2f);
        return average + (Random.value < 0.5f ? -1 : 1) * rng * halfDistance;

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, background))
            {
                if (rt.timer > 0)
                {
                    Fabric.EventManager.Instance.PostEvent(boneAudio);
                    totalScore += boneClickScore;
                    UpdateTotalScore();
                    ShowSkeleton();
                    //hit.collider.transform.position = bonePos.position;
                    hit.collider.transform.position = originalPos[hit.collider.gameObject] + new Vector3(0, 1, 0);
                    GameObject go = hit.collider.gameObject;
                    go.GetComponent<BoxCollider>().enabled = false;

                    //Destroy(hit.collider.gameObject);
                }
            }
        }

    }
}
