using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arbitro : MonoBehaviour
{
    public Transform spawnArea;
    public Transform sheepFree;

    [Header("Canvas")]
    [SerializeField] private TMP_Text __time;
    [SerializeField] private TMP_Text __sheepRemaining;


    private GameObject sheep;
    private GameObject wolf;

    private float spawnAreaNumberX;
    private float spawnAreaNumberY;

    private float n_sheeps;
    private float n_wolfs;
    private float timeRemaining;

    private void Awake()
    {
        sheep = Resources.Load<GameObject>("Prefabs/Sheep");
        wolf = Resources.Load<GameObject>("Prefabs/Wolf");

        spawnAreaNumberX = (float)(spawnArea.parent.transform.lossyScale.x * spawnArea.lossyScale.x * 0.5);
        spawnAreaNumberY = (float)(spawnArea.parent.transform.lossyScale.y * spawnArea.lossyScale.y * 0.5);
    }
    void Start()
    {
        DificultyStart();
        SpawnSheeps();
        SpawnWolf();


    }

    private void DificultyStart()
    {

        ////GameManager.instance.easy = true;
        ////GameManager.instance.mid = true;
        //GameManager.instance.hard = true;

        if (GameManager.instance.easy)
        {
            n_sheeps = 30;
            n_wolfs = 1;
            timeRemaining = 180;
        }
        else if (GameManager.instance.mid)
        {
            n_sheeps = 60;
            n_wolfs = 2;
            timeRemaining = 180 + 60;
        }
        else if (GameManager.instance.hard)
        {
            n_sheeps = 300;
            n_wolfs = 3;
            timeRemaining = 180 + 120;
        }


    }

    private void SpawnSheeps()
    {
        for(int i = 0; i < n_sheeps; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(-spawnAreaNumberX, spawnAreaNumberX), Random.Range(-spawnAreaNumberY, spawnAreaNumberY) + spawnArea.transform.position.y);

            Instantiate(sheep, randomPos, Quaternion.identity, sheepFree);
        }
    }

    private void SpawnWolf()
    {
        //Vector3(-52.4000015,-34.0999985,0)
        //Vector3(50,52.5,0)
        //Vector3(-52.7999992,51.9000015,0)
        //Vector3(52.4000015,-38.7000008,0)
        for (int i = 0; i < n_wolfs; i++)
        {
            Vector2 randomPos;

            if (Random.Range(0f,1f) >= 0.5)
            {
                randomPos = new Vector2(52, Random.Range(-26, 65));
            }
            else
            {
                randomPos = new Vector2(-52, Random.Range(-26, 65));
            }

            //WolfMovement spawnedWolf = Instantiate(wolf, randomPos, Quaternion.identity).GetComponent<WolfMovement>();

            //if (GameManager.instance.easy)
            //{
            //    spawnedWolf.wolfStartCooldown = 24f;
            //    spawnedWolf.wolfRestartCooldown = 24;
            //}
            //else if (GameManager.instance.mid)
            //{
            //    spawnedWolf.wolfStartCooldown = 22f;
            //    spawnedWolf.wolfRestartCooldown = 22;
            //}
            //else if (GameManager.instance.hard)
            //{
            //    spawnedWolf.wolfStartCooldown = 20f;
            //    spawnedWolf.wolfRestartCooldown = 20;
            //}
        }
    }

    private void Update()
    {
        UpdateUI();
        EndGame();
    }

    private void UpdateUI()
    {
        timeRemaining -= Time.deltaTime;
        int min = Mathf.FloorToInt(timeRemaining / 60);
        int sec = Mathf.FloorToInt(timeRemaining % 60);

        if (timeRemaining > 0)
        {
            __time.text = string.Format("{0:00}:{1:00}", min, sec);
            GameManager.instance.nSheepsFinal = sheepFree.childCount;

        }
        if (sheepFree.childCount > 0)
        {
            __sheepRemaining.text = string.Format("Sheep Remaining: " + sheepFree.childCount);
            GameManager.instance.remainingTimeFinal = timeRemaining;
        }
    }

    private void EndGame()
    {


        if (sheepFree.childCount <= 0)
        {
            //endgame 1
            Debug.Log("ganaste");
            GameManager.instance.win = true;

            SceneManager.LoadScene("End");

        }
        if (timeRemaining <= 0)
        {
            //endgame 2
            Debug.Log("se acabo el tiempo");
            GameManager.instance.lose = true;

            SceneManager.LoadScene("End");

        }

    }
}
