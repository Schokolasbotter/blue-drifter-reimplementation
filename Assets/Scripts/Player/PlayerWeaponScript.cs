using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class PlayerWeaponScript : MonoBehaviour
{
    public enum Weaponstate
    {
        Binoculars,
        Gun
    }

    [Header("Variables")]
    //Variables
    public Weaponstate weaponState = Weaponstate.Binoculars;
    [SerializeField] private float normalFov;
    [SerializeField] private float binocularFov;
    [SerializeField] private float gunFov;
    [SerializeField][Range(1f,100f)] private float fovSpeed;
    public AnimationCurve binoculareCurve = new AnimationCurve();
    private float t = 0;
    public bool aiming = false;
    public bool charging = false;
    public float chargingTimer = 0f;
    public float chargeTime1 = 0.5f;
    public float chargeTime2 = 1f;
    public int chargeLevel = 0;
    public bool cooling = false;
    [Range(0f,10f)]public float coolingTime = 1f;
    private float coolingTimer = 0f;

    [Header("References")]
    //References
    [SerializeReference] private Camera playerCamera;
    public GameObject handWithGun;
    public GameObject regularBulletPrefabS;
    public GameObject regularBulletPrefabM;
    public GameObject regularBulletPrefabL;
    public GameObject gunShotPoint;
    public GameObject bulletContainer;
    public GameObject handWithBinoculars;
    private InputActions inputActions;
    public GameObject particleSystemGameObject;
    private new ParticleSystem particleSystem;
    public TextMeshProUGUI scannerText, gunText, crosshairText;
    private AudioSource audiosource;
    public AudioClip laserClip;

    private void Start()
    {
        particleSystem = particleSystemGameObject.GetComponent<ParticleSystem>();
        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Weapon Change
        if (inputActions.Character.WeaponSwap.WasPressedThisFrame() && !inputActions.Character.Aiming.IsPressed())
        {
            changeWeapon();
        }

        //Aiming
        if (inputActions.Character.Aiming.IsPressed())
        {
            aiming = true;
            if(weaponState == Weaponstate.Gun)
            {
                crosshairText.text = " ";
            }
            else if(weaponState == Weaponstate.Binoculars)
            {
                crosshairText.text = "O";
                scan();
            }
            t = Mathf.Clamp01(t + (fovSpeed * Time.deltaTime));
        }
        else
        {
            aiming = false;
            crosshairText.text = "=";
            t = Mathf.Clamp01(t - (fovSpeed * Time.deltaTime));
        }
        aim();

        //Gun
        //Cooldown
        coolingTimer += Time.deltaTime;
        if(coolingTimer >= coolingTime)
        {
            cooling = false;
        }
        //Charging
        if (weaponState == Weaponstate.Gun && inputActions.Character.Shooting.IsPressed() && aiming && !cooling)
        {
            charging = true;
            chargingTimer += Time.deltaTime;
            if (chargingTimer > chargeTime2)
            {
                chargeLevel = 2;
            }
            else if (chargingTimer > chargeTime1)
            {
                chargeLevel = 1;
            }

        }
        else { charging = false;}

        //Shooting
        if (weaponState == Weaponstate.Gun && inputActions.Character.Shooting.WasReleasedThisFrame() && aiming && !cooling)
        {
            shoot();
        }
    }

    private void shoot()
    { 
        switch (chargeLevel)
        {
            case 0:
                Instantiate(regularBulletPrefabS, gunShotPoint.transform.position, gunShotPoint.transform.rotation, bulletContainer.transform);
                break;
            case 1:
                Instantiate(regularBulletPrefabM, gunShotPoint.transform.position, gunShotPoint.transform.rotation, bulletContainer.transform);
                break;
            case 2:
                Instantiate(regularBulletPrefabL, gunShotPoint.transform.position, gunShotPoint.transform.rotation, bulletContainer.transform);
                break;

        }
        particleSystem.Play();
        chargeLevel = 0;
        chargingTimer = 0;
        coolingTimer = 0;
        cooling = true;
        audiosource.PlayOneShot(laserClip);
    }
    void aim()
    {
        switch (weaponState)
        {
            case Weaponstate.Binoculars:
                playerCamera.fieldOfView = Mathf.Lerp(normalFov, binocularFov, binoculareCurve.Evaluate(t));

                break; 
            case Weaponstate.Gun:
                playerCamera.fieldOfView = Mathf.Lerp(normalFov, gunFov, t);
                break;
        }
    }

    void changeWeapon()
    {
        if(weaponState == Weaponstate.Binoculars)
        {
            weaponState = Weaponstate.Gun;
            handWithBinoculars.SetActive(false);
            handWithGun.SetActive(true);
            gunText.text = "Gun<";
            gunText.color = Color.white;
            scannerText.text = "Scanner[Q]";     
            scannerText.color = new Color(0.416f, 0.416f, 0.416f);
        }
        else if(weaponState == Weaponstate.Gun)
        {
            weaponState=Weaponstate.Binoculars;
            handWithGun.SetActive(false);
            handWithBinoculars.SetActive(true);
            gunText.text = "Gun[Q]";
            gunText.color = new Color(0.416f, 0.416f, 0.416f);
            scannerText.text = "Scanner<";
            scannerText.color = Color.white;
        }
    }

    private void scan()
    {
        RaycastHit rayHit;
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(playerCamera.scaledPixelWidth/2,playerCamera.scaledPixelHeight/2,0f));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        if (Physics.Raycast(ray, out rayHit))
        {
            if(rayHit.collider.gameObject.tag == "Guard" || rayHit.collider.gameObject.tag == "NPC" || rayHit.collider.gameObject.tag == "Bandit")
            {
                rayHit.collider.GetComponent<uiMarksScript>().onRaycastHit();
            }
        }
    }

    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


}
