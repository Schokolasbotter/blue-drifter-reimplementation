using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class regularNPC : MonoBehaviour
{
    public bool canWalk = true;
    public float moveAfter = 5f;
    public float walkingRange = 5;
    private float moveTimer = 0;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canWalk)
        {
            moveTimer += Time.deltaTime;
            if(moveTimer >= moveAfter)
            {
                moveTimer = 0;
                navMeshAgent.SetDestination(transform.position + Random.onUnitSphere * walkingRange);
            }
            if (navMeshAgent.velocity != Vector3.zero)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }
}
