using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idScript : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public string npcName;
    public gameManagerScript gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<gameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gameManager.collectedID = npcName;
            Destroy(this.gameObject);
        }
    }

}
