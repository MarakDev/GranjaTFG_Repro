using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private TMP_Text time;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.win)
            time.text = "Remaining Time: " + GameManager.instance.remainingTimeFinal;

        if(GameManager.instance.lose)
            time.text = "Sheeps that dont make it :(\n" + GameManager.instance.nSheepsFinal;

    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(10);

        GameManager.instance.lose = false;
        GameManager.instance.win = false;
        GameManager.instance.easy = false;
        GameManager.instance.mid = false;
        GameManager.instance.hard = false;

        SceneManager.LoadScene("Menu");
    }
}
