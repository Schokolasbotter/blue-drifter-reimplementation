using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pointScript : MonoBehaviour
{
    public bool isTriggered = false;
    public TextMeshProUGUI instructionText;
    public gameManagerScript gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTriggered = true;
            if (!gameManager.hasQuest)
            {
                if (!gameManager.quest1Done && gameObject.name == "Point1")
                {
                    instructionText.text = "[E] to accept Quest";
                }
                else if (!gameManager.quest2Done && gameObject.name == "Point2")
                {
                    instructionText.text = "[E] to accept Quest";
                }
                else if(!gameManager.quest3Done && gameObject.name == "Point3")
                {
                    instructionText.text = "[E] to accept Quest";
                }
            }
            else if (gameManager.hasQuest)
            {
                if(gameManager.collectedID == gameObject.GetComponentInParent<terminalScript>().targetName1 && gameObject.name == "Point1")
                {
                    instructionText.text = "[E] to return Quest";
                }
                else if (gameManager.collectedID == gameObject.GetComponentInParent<terminalScript>().targetName2 && gameObject.name == "Point2")
                {
                    instructionText.text = "[E] to return Quest";
                }
                else if (gameManager.collectedID == gameObject.GetComponentInParent<terminalScript>().targetName3 && gameObject.name == "Point3")
                {
                    instructionText.text = "[E] to return Quest";
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isTriggered = false;
            instructionText.text = "";
        }
    }
}
