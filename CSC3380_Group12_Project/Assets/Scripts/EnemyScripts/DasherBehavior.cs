using UnityEngine;
using System;
using UnityEngine.AI;

public class DasherBehavior : MonoBehaviour
{
    enum State
    {
        IDLE,
        PACING,
        CHASE,
        SEARCH,
        GUARD,
        PREPARE,
        DASH,
        RECOVER,
        STUN,
        STAGGER
    }

    [Header("References")]
    [SerializeField] DasherReference reference;
    [SerializeField] Transform player;
    [SerializeField] LayerMask playerLayer;

    [Header("State")]
    [SerializeField] State state;

    [Header("Idle")]
    [SerializeField] float idleDuration;

    [Header("Pacing")]
    [SerializeField] float pacingRadius;
    [SerializeField] float pacingSpeed;

    [Header("Chase")]
    [SerializeField] float chaseSpeed;
    [SerializeField] float pathUpdateDelay;
    [SerializeField] float detectionRadius;

    [Header("Guard")]
    [SerializeField] float guardDist;
    [SerializeField] float guardDamageResistMult;
    [SerializeField] float minGuardTime;

    [Header("Prepare")]
    [SerializeField] float prepareTime;

    [Header("Dash")]
    [SerializeField] float dashDamage;
    [SerializeField] float minDashDist;
    [SerializeField] float maxDashDist;
    [SerializeField] float dashSpeed;

    [Header("Stun")]
    [SerializeField] float stunDuration;
    [SerializeField] float stunDamageMultipler;


    private float elapsed = 0;
    private float timer = 0;
    private Vector3 startingPos;


    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        switch (state){
            case State.IDLE:
                state = Idle();
                break;
            case State.PACING:
                state = Pacing();
                break;
            case State.CHASE:
                state = Chase();
                break;
            case State.SEARCH:
                state = Search();
                break;
            case State.GUARD:
                break;
            case State.PREPARE:
                break;
            case State.DASH:
                break;
            case State.RECOVER:
                break;
        }
    }


    bool hasStopped()
    {
        // 1. Check if the agent is currently calculating a path
        if (!reference.agent.pathPending)
        {
            // 2. Check if the distance to the target is within the stopping threshold
            if (reference.agent.remainingDistance <= reference.agent.stoppingDistance)
            {
                // 3. Confirm the agent has no path or has completely stopped moving
                return !reference.agent.hasPath || reference.agent.velocity.sqrMagnitude == 0f;
            }
        }
        return false;
    }

    bool playerInDetectRange()
    {
        return player && Vector3.Distance(player.position, transform.position) <= detectionRadius;
    }

    bool playerInChaseRange()
    {
        return player && Vector3.Distance(player.position, transform.position) <= detectionRadius * 1.5;
    }

    bool playerInGuardRange()
    {
        return player && Vector3.Distance(player.position, transform.position) <= guardDist;
    }

    bool playerInAttackRange()
    {
        return player && Vector3.Distance(player.position, transform.position) <= minDashDist;
    }

    bool playerInLOS()
    {
        return true;
        /*
        Vector3 playerDir = (player.position - transform.position).normalized;
        float playerDist = Vector3.Distance(player.position, transform.position);
        if (!Physics.Raycast(transform.position, playerDir, out RaycastHit hit, playerDist))
        {
            Debug.Log("Nothing seen");
            return false;
        }
            
        if(hit.transform.gameObject.layer == playerLayer)
        {
            Debug.Log("Player seen");
            return true;
        }
        else
        {
            Debug.Log("Player obscured by " + hit.transform.name);
            return false;
        }
        */
    }

    State Idle()
    {
        if (playerInDetectRange() && playerInLOS())
            return State.CHASE;

        if (timer <= idleDuration)
        {
            timer += Time.deltaTime;
            return State.IDLE;
        }
        timer = 0;

        Vector2 point = UnityEngine.Random.insideUnitCircle * pacingRadius;
        reference.agent.SetDestination(startingPos + new Vector3(point.x, 0, point.y));
        return State.PACING;
    }

    State Pacing()
    {
        reference.agent.speed = pacingSpeed;

        if (playerInDetectRange() && playerInLOS()) return State.CHASE;

        if (hasStopped()) return State.IDLE;
        
        return State.PACING;
    }

    State Chase()
    {
        reference.agent.speed = chaseSpeed;

        if (elapsed <= pathUpdateDelay)
            return State.CHASE;
        elapsed = 0;

        if (!playerInChaseRange())
            return State.SEARCH;

        reference.agent.SetDestination(player.transform.position);
        return State.CHASE;
    }

    State Search()
    {
        if (hasStopped())
            return State.IDLE;

        return State.SEARCH;
    }
}
