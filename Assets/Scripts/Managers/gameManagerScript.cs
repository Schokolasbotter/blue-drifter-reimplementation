using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour
{
    public bool hasQuest = false, quest1Done = false, quest2Done = false, quest3Done = false;
    public string collectedID = "";
    public TextMeshProUGUI moneyText;
    public int moneyCollected = 0;
    public GameObject rocketGate, endgameScreen, UIScreen, pauseScreen, startScreen, deathScreen;
    public GameObject grabMission, returnMission, island1, island2, island3, escape;
    private InputActions inputActions;

    private void Awake()
    {
         
        Time.timeScale = 0.0f;
        inputActions = new InputActions();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        moneyText.text = "£ " + moneyCollected.ToString() + "/900";

        if(moneyCollected >= 900)
        {
            rocketGate.SetActive(false);
        }

        if ((endgameScreen.activeSelf == true || deathScreen.activeSelf == true) && inputActions.Character.MenuConfirm.IsPressed())
        {
            SceneManager.LoadScene(0);
        }

        if(startScreen.activeSelf == true && inputActions.Character.MenuConfirm.IsPressed())
        {
            startGame();
        }

        if (inputActions.Character.Pause.WasPressedThisFrame() || inputActions.Car.Pause.WasPressedThisFrame()){
            pauseGame();
        }

        adjustWaypoint();
    }

   public void adjustWaypoint()
    {
        if(!hasQuest)
        {
            grabMission.SetActive(true);
            returnMission.SetActive(false);
            island1.SetActive(false); 
            island2.SetActive(false);
            island3.SetActive(false);
        }
        else if(hasQuest && collectedID != "")
        {
            grabMission.SetActive(false);
            returnMission.SetActive(true);
            island1.SetActive(false);
            island2.SetActive(false);
            island3.SetActive(false);
        }
        else if(hasQuest && collectedID == "")
        {
            grabMission.SetActive(false);
            returnMission.SetActive(false);
            island1.SetActive(true);
            island2.SetActive(true);
            island3.SetActive(true);
        }
    }

    public void youDied()
    {
        Time.timeScale = 0f;
        UIScreen.SetActive(false);
        deathScreen.SetActive(true);
    }

    public void pauseGame()
    {
        switch (Time.timeScale)
        {
            case 0:
                Time.timeScale =1; 
                break;
            case 1:
                Time.timeScale = 0; 
                break;
        }
        switch (Cursor.lockState)
        {
            case CursorLockMode.Locked:
                Cursor.lockState = CursorLockMode.None;
                break;
            case CursorLockMode.None:
                Cursor.lockState= CursorLockMode.Locked;
                break;
        }
        switch (Cursor.visible)
        {
            case true:
                Cursor.visible = false;
                break;
            case false:
                Cursor.visible = true;
                break;
        }
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        
    }

    public void startGame()
    {
        startScreen.SetActive(false);
        UIScreen.SetActive(true);
        Time.timeScale = 1;
    }

    public void endGame()
    {
        Time.timeScale = 0f;
        UIScreen.SetActive(false);
        endgameScreen.SetActive(true);

    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
}
