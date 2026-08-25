using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class gunScript : MonoBehaviour
{
    
    public Transform regularPosition;
    public Transform aimingPosition;
    public PlayerWeaponScript weaponScript;
    [SerializeField][Range(0f, 1f)] private float t = 0;
    [Range(1f,50f)] public float animationSpeed = 1f;
    [SerializeField][Range(0f, 0.1f)] private float shakeRange = 0.02f;
    [SerializeField][Range(0f, 300f)] private float shakeSpeed = 100f;
    private float shakeTimer;
    public MeshRenderer gunLight;
    private Material gunLightMaterial;
    [ColorUsage(false, true)]
    public Color color0,color1,color2;


    private void Start()
    {
        gunLightMaterial = gunLight.material;
    }
    void Update()
    {
        if (weaponScript.charging && weaponScript.chargeLevel <1)
        {
            
            gunLightMaterial.SetColor("_EmissionColor", Color.Lerp(color0,color1,Mathf.Lerp(0f,weaponScript.chargeTime1, weaponScript.chargingTimer)));
        }
        else if(weaponScript.charging && weaponScript.chargeLevel <= 2)
        {
            gunLightMaterial.SetColor("_EmissionColor", Color.Lerp(color1, color2, Mathf.Lerp(weaponScript.chargeTime1, weaponScript.chargeTime2, weaponScript.chargingTimer))) ; 
        }
        else
        {
            gunLightMaterial.SetColor("_EmissionColor", color0);
        }

        bool aiming = weaponScript.aiming;
        if (aiming)
        {
            t = Mathf.Clamp(t + (animationSpeed * Time.deltaTime),0f,1f);
            if (weaponScript.charging)
            {
                shakeTimer += shakeSpeed * Time.deltaTime;
                weaponShake(weaponScript.chargeLevel);
            }
            else
            {
                shakeTimer = 0f;
            }
        }
        else
        {
            t = Mathf.Clamp(t - (animationSpeed * Time.deltaTime), 0f, 1f);
        }
        transform.position = Vector3.Lerp(regularPosition.position, aimingPosition.position + aimingPosition.forward * weaponShake(weaponScript.chargeLevel), t);
        transform.rotation = Quaternion.Slerp(regularPosition.rotation, aimingPosition.rotation, t);
    }

    private float weaponShake(int chargeLevel)
    {
        float shakeOffset = 0f;
        switch (chargeLevel)
        {
            case 0:
                break;
            case 1:
                shakeOffset = Mathf.Sin(shakeTimer) * shakeRange;
                break;
            case 2:
                shakeOffset = Mathf.Sin(shakeTimer) * shakeRange * 2f;
                break;
        }
        return shakeOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(regularPosition.position, 0.01f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(aimingPosition.position, 0.01f);
    }
}
