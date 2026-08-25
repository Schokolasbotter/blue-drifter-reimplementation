using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraManagerScript : MonoBehaviour
{
    public PlayerMovement pm;
    public carMovement cm;
    public Camera playerCamera;
    public Camera carCamera;
    private Animator carAnimator;
    public Collider waterCollider;

    private void Start()
    {
        pm.onEnterCar.AddListener(enterCar);
        cm.onExitCar.AddListener(exitCar);
        carAnimator = cm.gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (cm.canEnter)
        {
            pm.canEnter = true;
        }
        else
        {
            pm.canEnter = false;
        }
    }

    private void enterCar()
    {
        pm.gameObject.GetComponent<healthScript>().resetHealth();
        //Camera Changes
        carCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        //Change COntrols
        pm.disablePlayerControls();
        cm.enableCarControls();
        //Move the player onto the car
        pm.gameObject.transform.SetParent(cm.characterStorage.transform, false);
        pm.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        pm.gameObject.SetActive(false);
        //Stop animation
        carAnimator.SetBool("insideCar", true);
        //Switch off Water Collider
        waterCollider.enabled = false;
    }

    private void exitCar()
    {
        //Activate Player
        pm.gameObject.SetActive(true);
        pm.gameObject.transform.SetParent(null,false);
        pm.gameObject.transform.position = cm.characterStorage.transform.position;
        //Camera Change
        playerCamera.gameObject.SetActive(true);
        carCamera.gameObject.SetActive(false);
        //Change COntrols
        cm.disableCarControls();
        pm.enablePlayerControls();
        //Start Animation
        carAnimator.SetBool("insideCar", false);
        //Enable Water Collider
        waterCollider.enabled = true;
    }
}
