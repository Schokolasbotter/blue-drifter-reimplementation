using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class terminalScript : MonoBehaviour
{
    [Header("Quest Details")]
    public string targetName1;
    public string targetDestination1, targetName2, targetDestination2, targetName3, targetDestination3;
    [Header("Variables")]
    public pointScript point1;
    public pointScript point2, point3;
    public TextMeshProUGUI instructionText, targetNameText, destinationText;
    public InputActions inputActions;
    public gameManagerScript gameManager;
    public GameObject player;

    private void Awake()
    {
        inputActions = new InputActions();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {  
        if (inputActions.Character.Interact.WasPressedThisFrame())
        {
            //Accept Quest
            if (!gameManager.hasQuest)
            {
                if(point1.isTriggered && !gameManager.quest1Done)
                {
                    targetNameText.text = "Target: " + targetName1;
                    destinationText.text = "Destination: " + targetDestination1;
                    instructionText.text = "";
                    gameManager.hasQuest = true;
                }
                else if (point2.isTriggered && !gameManager.quest2Done)
                {
                    targetNameText.text = "Target: " + targetName2;
                    destinationText.text = "Destination: " + targetDestination2;
                    instructionText.text = "";
                    gameManager.hasQuest = true;
                }
                else if (point3.isTriggered && !gameManager.quest3Done)
                {
                    targetNameText.text = "Target: " + targetName3;
                    destinationText.text = "Destination: " + targetDestination3;
                    instructionText.text = "";
                    gameManager.hasQuest = true;
                }
            }
            //Return Quest
            else if(gameManager.hasQuest)
            {
                if (point1.isTriggered && gameManager.collectedID == targetName1)
                {
                    targetNameText.text = "";
                    destinationText.text = "";
                    gameManager.quest1Done = true;
                    returnQuest();
                    
                }
                else if(point2.isTriggered && gameManager.collectedID == targetName2)
                {
                    targetNameText.text = "";
                    destinationText.text = "";
                    gameManager.quest2Done = true; 
                    returnQuest();
                }
                else if(point3.isTriggered && gameManager.collectedID == targetName3)
                {
                    targetNameText.text = "";
                    destinationText.text = "";
                    gameManager.quest3Done = true;
                    returnQuest();
                }
                
            }
        }
    }

    private void returnQuest()
    {
        gameManager.collectedID = "";
        instructionText.text = "";
        targetNameText.text = "";
        destinationText.text = "";
        gameManager.hasQuest = false;
        gameManager.moneyCollected += 300;
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
