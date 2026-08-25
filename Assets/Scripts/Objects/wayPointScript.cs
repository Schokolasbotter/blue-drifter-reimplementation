using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class wayPointScript : MonoBehaviour
{
    public string text = "";
    public TextMeshProUGUI waypointText;
    public TextMeshProUGUI wayPointMark;
    public RectTransform canvasTransform;
    private GameObject player;
    private GameObject car;
    private float originalY;

    private void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerMovement>().gameObject;
        car = GameObject.FindFirstObjectByType<carMovement>().gameObject;
        waypointText.text = text;
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeSelf)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;
            float t = Mathf.InverseLerp(0f, 1500f, distanceToPlayer);
            float scaleValue = Mathf.Lerp(1f, 100f, t);
            canvasTransform.localScale = new Vector3(scaleValue,scaleValue,scaleValue);
            canvasTransform.position = new Vector3(canvasTransform.position.x, originalY + scaleValue, canvasTransform.position.z);
            Vector3 playerPosition = player.transform.position;
            wayPointMark.rectTransform.LookAt(playerPosition);
            waypointText.rectTransform.LookAt(playerPosition);
        }
        else if (car.activeSelf)
        {
            Vector3 directionTocar = car.transform.position - transform.position;
            float distanceTocar = directionTocar.magnitude;
            float t = Mathf.InverseLerp(0f, 1500f, distanceTocar);
            float scaleValue = Mathf.Lerp(10f, 100f, t); 
            canvasTransform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            canvasTransform.position = new Vector3(canvasTransform.position.x, originalY + scaleValue, canvasTransform.position.z);
            Vector3 carPosition = car.transform.position;
            wayPointMark.rectTransform.LookAt(carPosition);
            waypointText.rectTransform.LookAt(carPosition);
        }
    }
}
