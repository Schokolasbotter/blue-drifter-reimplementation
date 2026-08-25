using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public Rigidbody rb;
    public float bulletVelocity = 20f;
    public float lifeTime = 2f;
    [Range(0, 100)] public int damageValue = 10; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * bulletVelocity;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
