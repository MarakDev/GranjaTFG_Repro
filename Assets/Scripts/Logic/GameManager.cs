using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public GameObject canvas;

    [HideInInspector] public bool easy;
    [HideInInspector] public bool mid;
    [HideInInspector] public bool hard;

    [HideInInspector] public bool win;
    [HideInInspector] public float remainingTimeFinal;

    [HideInInspector] public bool lose;
    [HideInInspector] public float nSheepsFinal;

    void Awake()
    {
        if (instance == null) { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvas = GameObject.Find("Canvas");
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
