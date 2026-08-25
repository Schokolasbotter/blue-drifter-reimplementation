using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainManager : MonoBehaviour
{
    public GameObject rainObject;
    public GameObject player;
    public GameObject car;
  

    // Update is called once per frame
    void Update()
    {
        if (player.activeSelf)
        {
            rainObject.transform.position = player.transform.position;
        }
        else
        {
            rainObject.transform.position = car.transform.position;

        }
    }
}
