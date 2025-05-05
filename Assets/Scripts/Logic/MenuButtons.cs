using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    private EventSystem _eventSystem;
    public GameObject pagina1;

    public GameObject modo1PopUp;

    public GameObject dificulty;
    public GameObject controlsPopUp;
    public GameObject optionsPopUp;

    public void Start()
    {
        _eventSystem = EventSystem.current;
        modo1PopUp.SetActive(false);

        dificulty.SetActive(false);
        controlsPopUp.SetActive(false);
        optionsPopUp.SetActive(false);
    }

    public void _PlayButton()
    {
        pagina1.SetActive(false);
        modo1PopUp.SetActive(true);
        _eventSystem.SetSelectedGameObject(modo1PopUp.GetComponentInChildren<Button>().gameObject);
    }

    public void _ControlsButton()
    {
        controlsPopUp.SetActive(true);
    }

    public void _OptionsButton()
    {
        optionsPopUp.SetActive(true);
    }

    public void _SelectDificulty()
    {
        modo1PopUp.SetActive(false);
        dificulty.SetActive(true);
        _eventSystem.SetSelectedGameObject(dificulty.transform.Find("easy").gameObject);

    }

    public void _PlayGameMode()
    {
        SceneManager.LoadScene("ModoUTLS");
    }

    public void _Dificulty1PopUpEasy()
    {
        GameManager.instance.easy = true;
        GameManager.instance.mid = false;
        GameManager.instance.hard = false;
        modo1PopUp.SetActive(true);
    }
    public void _Dificulty1PopUpMid()
    {
        GameManager.instance.easy = false;
        GameManager.instance.mid = true;
        GameManager.instance.hard = false;
        modo1PopUp.SetActive(true);
    }
    public void _Dificulty1PopUpHard()
    {
        GameManager.instance.easy = false;
        GameManager.instance.mid = false;
        GameManager.instance.hard = true;
        modo1PopUp.SetActive(true);
    }


    public void _Back()
    {

    }
}
