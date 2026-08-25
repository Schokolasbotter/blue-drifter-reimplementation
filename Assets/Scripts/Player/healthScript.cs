using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class healthScript : MonoBehaviour
{
    public int health = 111;
    public float callRange;
    public TextMeshProUGUI healthText;
    public GameObject idPrefab;
    private gameManagerScript gameManager;
    public AudioClip hitClip;
    private AudioSource audioSource;

    private void Start()
    {
        if(this.gameObject.tag == "Player")
        {
            gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<gameManagerScript>();
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            healthText.text = "Health " + health.ToString();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            if (gameObject.tag == "NPC" || gameObject.tag == "Guard")
            {
                var souroundingGuards = Physics.OverlapSphere(transform.position, callRange);
                foreach (Collider c in souroundingGuards)
                {
                    if(c.gameObject.tag == "Guard")
                    {
                        c.GetComponent<guardScript>().guardState = guardScript.Guardstate.attack;
                    }
                }
            }
            if(gameObject.tag == "Bandit")
            {
                gameObject.GetComponent<banditScript>().banditstate = banditScript.Banditstate.attack;
            }
            else if(gameObject.tag == "Guard")
            {
                gameObject.GetComponent<guardScript>().guardState = guardScript.Guardstate.attack;
            }

            health -= collision.gameObject.GetComponent<bulletScript>().damageValue;
            audioSource.PlayOneShot(hitClip);
            if(gameObject.tag == "Player")
            {
                healthText.text = "Health " + health.ToString();
            }
        }
        if(health <= 0)
        {
            if(this.gameObject.tag != "Player")
            {
                string myName = gameObject.GetComponent<uiMarksScript>().npcName;
                if (myName != "Guard" && myName != "Bandit" && myName != "Civilian")
                {
                    Vector3 dropPosition = transform.position;
                    dropPosition.y += 1;
                   GameObject droppedID = Instantiate(idPrefab,dropPosition, Quaternion.LookRotation(Vector3.forward, Vector3.up));
                   droppedID.GetComponent<idScript>().npcName = myName;
                }
                Destroy(this.gameObject);
            }
            else
            {
                gameManager.youDied();
            }
        }
    }

    public void resetHealth()
    {
        health = 111;
        if (gameObject.tag == "Player")
        {
            healthText.text = "Health " + health.ToString();
        }
    }
}
