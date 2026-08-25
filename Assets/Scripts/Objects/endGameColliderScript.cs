using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGameColliderScript : MonoBehaviour
{
    public gameManagerScript gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.quest1Done && gameManager.quest2Done && gameManager.quest3Done)
        {
            gameManager.endGame();
        }
    }
}
