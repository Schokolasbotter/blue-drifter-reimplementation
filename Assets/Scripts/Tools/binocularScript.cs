using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class binocularScript : MonoBehaviour
{
    public Transform regularPosition;
    public Transform watchingPosition;
    public PlayerWeaponScript weaponScript;
    [SerializeField][Range(0f, 1f)] private float t = 0;
    [Range(1f, 50f)] public float animationSpeed = 1f;
    public Image spectrumImage;
    public Image filterImage;

    void Update()
    {
        bool aiming = weaponScript.aiming;
        if (aiming)
        {
            t = Mathf.Clamp(t + (animationSpeed * Time.deltaTime), 0f, 1f);
        }
        else
        {
            t = Mathf.Clamp(t - (animationSpeed * Time.deltaTime), 0f, 1f);
        }
        transform.position = Vector3.Lerp(regularPosition.position, watchingPosition.position, t);
        transform.rotation = Quaternion.Slerp(regularPosition.rotation, watchingPosition.rotation, t);
        if(t == 1)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            spectrumImage.gameObject.SetActive(true);
            filterImage.gameObject.SetActive(true);
        }
        else
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            spectrumImage.gameObject.SetActive(false);
            filterImage.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(regularPosition.position, 0.01f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(watchingPosition.position, 0.01f);
    }
}
