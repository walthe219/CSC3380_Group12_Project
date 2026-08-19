using UnityEngine;
using System;
using UnityEngine.AI;

// Dasher enemy behavior controller using a finite state machine.
// Performs dash attack when player in range

// Has three primary state groups: Idling, Chasing, and Attacking
// Will idle until player within detect range
// Will chase player until within attack range
// Will prepare attack then perform a dash
public class DasherBehavior : MonoBehaviour
{
    enum State
    {
        // IDLE STATES
        IDLE,
        PACING,

        //CHASE STATES
        CHASE,
        SEARCH,
        GUARD,

        //ATTACKING STATES
        PREPARE,
        LOCKED,
        DASH,
        RECOVER,
        STUN,
        DEAD
    }


    [Header("References")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] DasherReference dasher;
    [SerializeField] Transform player;
    [SerializeField] LayerMask playerLayer;

    [Header("State")]
    [SerializeField] State currentState;
    [SerializeField] State previousState;
    [SerializeField] float timer = 0;

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
    [SerializeField] float guardingRadius;
    [SerializeField] float minGuardDuration;

    [Header("Prepare")]
    [SerializeField] float attackRadius;
    [SerializeField] float prepareDuration;
    [SerializeField] float prepareTurningSpeed;
    [SerializeField] float maxTurningAngle;

    [Header("Locked")]
    [SerializeField] float lockedDuration;

    [Header("Dash")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashDistance;

    [Header("Recover")]
    [SerializeField] float recoverDuration;

    [Header("Stun")]
    [SerializeField] float stunDuration;


    public event Action<String> OnStateChange;


    private float elapsed = 0;
    private Vector3 startingPos;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startingPos = transform.position;


        dasher.back.OnDamageTaken += () => 
        { 
            if (currentState == State.RECOVER) 
                ChangeState(State.STUN); 
        };

        dasher.target.OnDamageTaken += () => 
        { 
            if (currentState == State.IDLE || currentState == State.CHASE) 
                ChangeState(State.CHASE); 
        };

        dasher.target.OnDeath += () => ChangeState(State.DEAD);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        timer += Time.deltaTime;

        State newState = State.IDLE;
        switch (currentState)
        {
            case State.IDLE:
                newState = Idle();
                break;
            case State.PACING:
                newState = Pacing();
                break;
            case State.CHASE:
                newState = Chase();
                break;
            case State.SEARCH:
                newState = Search();
                break;
            case State.GUARD:
                newState = Guard();
                break;
            case State.PREPARE:
                newState = Prepare();
                break;
            case State.LOCKED:
                newState = Locked();
                break;
            case State.DASH:
                newState = Dash();
                break;
            case State.RECOVER:
                newState = Recover();
                break;
            case State.STUN:
                newState = Stun();
                break;
        }

        ChangeState(newState);
            
    }

    void ChangeState(State newState)
    {
        if (newState == currentState || currentState == State.DEAD)
            return;

        Debug.Log("Changing state from " + currentState + " to " + newState);

        timer = 0;
        elapsed = 0;

        previousState = currentState;
        currentState = newState;
        OnStateChange?.Invoke(currentState.ToString());
    }

    bool hasStopped()
    {
        // 1. Wait if the path is still being calculated asynchronously
        if (agent.pathPending) return false;

        // 2. If it explicitly doesn't have a path, it is stopped
        if (!agent.hasPath) return true;

        // 3. Only safely check remaining distance once hasPath is confirmed true
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // 4. Double check it has ceased physical movement
            return agent.velocity.sqrMagnitude == 0f;
        }

        return false;
    }

    bool TargetInRange(Transform target, double range)
    {
        return player && Vector3.Distance(player.position, transform.position) <= detectionRadius;
    }

    bool playerInDetectRange()
    {
        return TargetInRange(player, detectionRadius);
    }

    bool playerInChaseRange()
    {
        return TargetInRange(player, detectionRadius * 1.5);
    }

    bool playerInGuardRange()
    {
        return TargetInRange(player, guardingRadius);
    }

    bool playerInAttackRange()
    {
        return TargetInRange(player, attackRadius);
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

    void LookAt(Vector3 target, float turnSpeed)
    {
        DisableAgent();

        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0; // Prevent looking up or down into the floor

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed * Mathf.Deg2Rad);
        }

        EnableAgent();
    }

    void DisableAgent()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }

    void EnableAgent()
    {
        agent.isStopped = false;
    }


    //==================================================================================
    //                              STATE METHODS
    //==================================================================================

    State Idle()
    {
        if (playerInDetectRange() && playerInLOS())
            return State.CHASE;

        if (timer <= idleDuration)
        {
            return State.IDLE;
        }
        timer = 0;

        Vector2 point = UnityEngine.Random.insideUnitCircle * (pacingRadius + agent.stoppingDistance);
        agent.SetDestination(startingPos + new Vector3(point.x, 0, point.y));
        return State.PACING;
    }

    State Pacing()
    {
        agent.speed = pacingSpeed;

        if (playerInDetectRange() && playerInLOS()) return State.CHASE;

        if (hasStopped()) return State.IDLE;
        
        return State.PACING;
    }

    State Chase()
    {
        agent.speed = chaseSpeed;

        if (hasStopped())
            LookAt(player.position, agent.angularSpeed);

        if (elapsed <= pathUpdateDelay)
            return State.CHASE;
        elapsed = 0;

        if (!playerInChaseRange())
            return State.SEARCH;

        agent.SetDestination(player.transform.position);


        if (playerInGuardRange())
            return State.GUARD;

        return State.CHASE;
    }

    State Search()
    {
        if (hasStopped())
            return State.IDLE;

        if (playerInDetectRange() && playerInLOS()) return State.CHASE;

        return State.SEARCH;
    }

    State Guard()
    {
        if (hasStopped())
            LookAt(player.position, agent.angularSpeed);

        if (elapsed <= pathUpdateDelay)
            return State.GUARD;
        elapsed = 0;

        if (!playerInChaseRange())
            return State.SEARCH;

        agent.SetDestination(player.transform.position);


        if (timer <= minGuardDuration)
        {
            return State.GUARD;
        }

        if (!playerInAttackRange())
            return State.GUARD;

        DisableAgent();
        dasher.dashPreview.enabled = true;

        return State.PREPARE;
    }

    State Prepare()
    {
        Vector3 playerDirection = player.position - transform.position;

        if(Vector3.Angle(playerDirection, transform.forward) < maxTurningAngle)
            LookAt(player.position, prepareTurningSpeed);

        if (timer <= prepareDuration)
        {
            return State.PREPARE;
        }

        return State.LOCKED;
    }

    State Locked()
    {
        if (timer <= lockedDuration)
        {
            return State.LOCKED;
        }

        dasher.dashPreview.enabled = false;
        return State.DASH;
    }

    State Dash()
    {
        dasher.dashAttack.ToggleOn();
        if (timer <= dashDuration)
        {
            float dashSpeed = dashDistance / dashDuration;
            agent.Move(transform.forward * dashSpeed * Time.deltaTime);
            return State.DASH;
        }

        dasher.dashAttack.ToggleOFF();
        EnableAgent();
        return State.RECOVER;
    }

    State Recover()
    {
        if (timer <= recoverDuration)
        {
            return State.RECOVER;
        }

        return State.CHASE;
    }

    State Stun()
    {
        if (timer <= recoverDuration)
        {
            return State.STUN;
        }

        return State.CHASE;
    }
}
