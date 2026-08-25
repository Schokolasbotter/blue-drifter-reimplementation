using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class guardScript : MonoBehaviour
{
    [Header("Walking")]
    public bool canWalk = true;
    public float moveAfter = 5f;
    public float walkingRange = 5;
    private float moveTimer = 0;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public enum Guardstate { walking, attack};
    public enum Attackstate { startAttack, lookAtPlayer, checkPlayer, walkToPlayer }
    [Header("Attack")]
    public Guardstate guardState = Guardstate.walking;
    public Attackstate attackState = Attackstate.startAttack;
    private Quaternion currentRotation;
    private float lookAtTimer = 0f;
    private bool isTurningToPlayer = false;
    private bool initialAttack = true;
    public Transform rayCastOrigin;
    private Transform playerTransform;
    private Vector3 vectorToPlayer;
    public float distanceToPlayer = 30f;
    public LayerMask playerLayerMask;
    private npcGunScript npcGun;

   

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        npcGun = GetComponentInChildren<npcGunScript>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        if(guardState == Guardstate.attack)
        {
            if(!playerTransform.gameObject.activeSelf)
            {
                navMeshAgent.ResetPath();
                guardState = Guardstate.walking;
            }

            if (initialAttack)
            {
                navMeshAgent.stoppingDistance = distanceToPlayer;
            }

            
            vectorToPlayer = playerTransform.position - transform.position;
            vectorToPlayer.y = 0f;
            vectorToPlayer.Normalize();
            switch (attackState)
            {
                case Attackstate.startAttack:
                    //Stop Walking
                    navMeshAgent.ResetPath();
                    animator.SetBool("isWalking", false);
                    isTurningToPlayer = true;
                    attackState = Attackstate.lookAtPlayer;
                    break;
                case Attackstate.lookAtPlayer: 
                    //Turn to Player
                    lookAtPlayer();
                    break;
                case Attackstate.checkPlayer:
                    //Can you see the player
                    Debug.DrawRay(rayCastOrigin.position, transform.forward * distanceToPlayer, Color.yellow);
                    RaycastHit hit;
                    bool hasHit = Physics.Raycast(rayCastOrigin.position, transform.forward,out hit, distanceToPlayer);
                    if (hasHit)
                    {
                        if (hit.collider == null)
                        {
                            attackState = Attackstate.walkToPlayer;
                        }
                        else
                        {
                            if (hit.collider.gameObject.tag == "Player")
                            {
                                //Shoot
                                npcGun.shoot();
                                navMeshAgent.stoppingDistance = distanceToPlayer;
                            }
                            else
                            {
                                initialAttack = false;
                                navMeshAgent.stoppingDistance = navMeshAgent.stoppingDistance * 0.6f;
                                attackState = Attackstate.walkToPlayer; ;
                            }
                        }
                    }
                    else
                    {
                        attackState = Attackstate.walkToPlayer; ;
                    }                  
                    break;
                case Attackstate.walkToPlayer:
                    walkToPlayer();
                    break;
            }
        }


        if (guardState == Guardstate.walking && canWalk)
        {
            initialAttack = true;
            attackState = Attackstate.startAttack;
            navMeshAgent.stoppingDistance = 0f;
            moveTimer += Time.deltaTime;
            if (moveTimer >= moveAfter)
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

    private void lookAtPlayer()
    {
        if(isTurningToPlayer)
        {
            
            Quaternion rotationLookAtPlayer = Quaternion.LookRotation(vectorToPlayer);
            if (lookAtTimer == 0)
            {
                currentRotation = transform.rotation;
            }
            if (lookAtTimer < 1)
            {
                lookAtTimer = Mathf.Clamp01(lookAtTimer + Time.deltaTime);
                transform.rotation = Quaternion.Slerp(currentRotation, rotationLookAtPlayer, lookAtTimer);
            }
            else if(lookAtTimer >= 1)
            {
                isTurningToPlayer = false;
                lookAtTimer = 0;
                attackState = Attackstate.checkPlayer;
            }

        }
    }

    private void walkToPlayer()
    {
        //Walk to the player
        navMeshAgent.SetDestination(playerTransform.position);
        animator.SetBool("isWalking", true);
        if (navMeshAgent.pathPending)
        {
            return;
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            attackState = Attackstate.startAttack;
        }
    }
}
