using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiMarksScript : MonoBehaviour
{
    public string npcName = "";
    public RectTransform canvasTransform;
    public TextMeshProUGUI scannerMark;
    public TextMeshProUGUI scannerText;
    private GameObject player;
    private float scanTimer = 0f;
    public float scanTime = 1f;
    public bool scanned = false;

    private void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerMovement>().gameObject;
        scannerText.text = npcName;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeSelf)
        {
            Vector3 playerPosition = player.transform.position;
            scannerMark.rectTransform.LookAt(playerPosition);
            scannerText.rectTransform.LookAt(playerPosition);
            playerPosition.y = canvasTransform.position.y;
            Vector3 vectorTowardsPlayer = playerPosition - canvasTransform.position;
            canvasTransform.rotation = Quaternion.LookRotation(vectorTowardsPlayer);

            //only visible if player uses binoculars
            if(player.GetComponent<PlayerWeaponScript>().weaponState == PlayerWeaponScript.Weaponstate.Binoculars && player.GetComponent<PlayerWeaponScript>().aiming)
            {
                if(scanned)
                {
                    scannerText.gameObject.SetActive(true);
                    scannerMark.gameObject.SetActive(false);
                }
                else
                {
                    scannerMark.gameObject.SetActive(true);
                    scannerText.gameObject.SetActive(false);    
                }
            }
            else
            {
                scannerText.gameObject.SetActive(false);
                scannerMark.gameObject.SetActive(false);
            }

        }
    }

    public void onRaycastHit()
    {
        scanTimer = Mathf.Clamp01(scanTimer + Time.deltaTime);
        if(scanTimer >= scanTime)
        {
            scanned = true;
        }
    }
}
