using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class carMovement : MonoBehaviour
{
    //Variables and References
    #region
    //Variables

    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintingSpeed;
    [SerializeField] private float backwardsSpeed;
    [SerializeField] private float turnSpeed;
    public bool canDock = false, isDocking = false, isDriving = false;
    private Vector3 startingPosition, dockPosition;
    private Quaternion startingRotation, dockRotation;
    private float dockTimer = 0f;
    public bool canEnter;
    public TextMeshProUGUI instructionsText;

    //References and Objects
    private InputActions inputActions;
    private CharacterController carController;
    public Transform characterStorage;
    private AudioSource audioSource;

    public UnityEvent onExitCar;
    #endregion


    private void Awake()
    {
         
        carController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (onExitCar == null)
        {
            onExitCar = new UnityEvent();
        }
    }

    private void Update()
    {
       
        //car Driving Movement
        Vector2 inputVector = inputActions.Car.Driving.ReadValue<Vector2>();
        Vector3 movementVector = transform.forward * inputVector.y;
        float usedSpeed;
        if(inputVector.y < 0)
        {
            usedSpeed = backwardsSpeed;
        }
        else if (inputActions.Car.Boost.IsPressed())
        {
            usedSpeed = sprintingSpeed;
        }
        else
        {
            usedSpeed = movementSpeed;
        }
        if(movementVector == Vector3.zero)
        {
            audioSource.Stop();
        }
        else if(movementVector != Vector3.zero && !audioSource.isPlaying) 
        {
            audioSource.Play();
        }
        carController.Move(movementVector * usedSpeed * Time.deltaTime);
        carController.gameObject.transform.Rotate(Vector3.up, inputVector.x * turnSpeed * Time.deltaTime);
        carController.transform.position = new Vector3(transform.position.x,6f,transform.position.z);


        //Entering Car
        if (inputActions.Car.ExitCar.IsPressed() && canDock)
        {
            isDocking = true;
            startingPosition = transform.position;
            startingRotation = transform.rotation;
        }

        if(isDocking)
        {
            dockTimer = Mathf.Clamp01(dockTimer += Time.deltaTime);
            transform.position = Vector3.Lerp(startingPosition, dockPosition, dockTimer);
            transform.rotation = Quaternion.Slerp(startingRotation, dockRotation, dockTimer);
            if(dockTimer == 1)
            {
                onExitCar.Invoke();
                dockTimer = 0;
                isDocking = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dock" && isDriving)
        {
            canDock = true;
            dockPosition = other.transform.position;
            dockRotation = other.transform.rotation;
            dockPosition = new Vector3(dockPosition.x,transform.position.y, dockPosition.z);
            instructionsText.text = "[E] To Exit Car";
        }
        else if(other.gameObject.tag == "Player")
        {
            canEnter = true;
            instructionsText.text = "[E] To Enter Car";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Dock")
        {
            canDock = false;
            instructionsText.text = "";
        }
        else if(other.gameObject.tag == "Player")
        {
            canEnter = false;
            instructionsText.text = "";
        }
    }

    //InputActions
    public void enableCarControls()
    {
        inputActions.Car.Enable();
        isDriving = true;
    }

    public void disableCarControls()
    {
        inputActions.Car.Disable();
        isDriving = false;
    }

    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Enable();
        inputActions.Car.Disable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
