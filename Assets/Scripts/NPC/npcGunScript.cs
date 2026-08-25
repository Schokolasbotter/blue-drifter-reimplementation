using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class npcGunScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunShotPoint;
    public GameObject bulletContainer;
    public GameObject particleSystemGameobject;
    private new ParticleSystem particleSystem;
    public bool cooling = false;
    [Range(0f, 10f)] public float coolingTime = 1f;
    private float coolingTimer = 0f;
    private Transform player;
    private AudioSource audioSource;
    public AudioClip lazerShot;

    private void Start()
    {
        particleSystem = particleSystemGameobject.GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
        audioSource = gameObject.GetComponentInParent<AudioSource>();
    }

    private void Update()
    {
        coolingTimer += Time.deltaTime;
        if (coolingTimer >= coolingTime)
        {
            cooling = false;
        }
    }
    public void shoot()
    {
        if (cooling)
        {
            return;
        }
        audioSource.PlayOneShot(lazerShot);
        Vector3 playerVector = player.position;
        playerVector.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation(playerVector-transform.position);
        Instantiate(bulletPrefab, gunShotPoint.position, gunShotPoint.rotation, bulletContainer.transform);
        particleSystem.Play();
        coolingTimer = 0;
        cooling = true;
    }
}
