using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RunnerBehavior : MonoBehaviour
{

    public RunnerReferences runRef;
    public bool behaviorActivator;
    public bool isActivated = false;
    private float BehaviorTimer = 0;

    public Transform playerTarget;
    public Transform portalTarget;
    public Transform playerCheck;
    public LayerMask playerMask;
    public float detectRadius = 25f;
    private bool isTargetingPlayer;

    private float pathUpdateDeadline;
    private float attackDistance;
    private float enemySpeed;

    private float dampTime = 0.2f;

    private bool hurtboxStatus = false;

    void Awake()
    {
        
        runRef = GetComponent<RunnerReferences>();
        attackDistance = runRef.agent.stoppingDistance;
        enemySpeed = runRef.agent.speed;
        ToggleHurtboxes();

    }
    /* 
     * Handles checking for player to start behavior loop
     */
    void Update()
    {
        if (!isActivated)
        {
            behaviorActivator = Physics.CheckSphere(playerCheck.position, 100f, playerMask);
            
        }
        if (behaviorActivator)
        {
            BehaviorTimer += Time.deltaTime;
            if (BehaviorTimer > 3f)
            {
                CheckTargetDistance();
            }
        }
        
    }
    //Checks where the player is in relation to enemy, and determines if enemy should change targets
    void CheckTargetDistance()
    {

        isActivated = true;

        isTargetingPlayer = Physics.CheckSphere(playerCheck.position, detectRadius, playerMask);

        if (isTargetingPlayer)
        {

            UpdatePath(playerTarget);

        }
        else
        {

            UpdatePath(portalTarget);

        }
        
    }
    //Handles the actual navmesh agent path updates and what to do when the enemy reaches destination, also activates animation booleans
    void UpdatePath(Transform target)
    {

        if (target != null)
        {
            bool canAttack = Vector3.Distance(transform.position, target.position) <= attackDistance;
            if (canAttack)
            {
                LookAndAttack(target);
            }
            else
            {
                runRef.anim.SetFloat("runSpeed", 1, dampTime, Time.deltaTime);
                runRef.anim.SetBool("isAttacking", false);
            }
            //Mathf.Lerp(runRef.anim.GetFloat("speed"), runRef.agent.desiredVelocity.sqrMagnitude, Time.deltaTime*100)

            if (Time.time >= pathUpdateDeadline)
            {

                pathUpdateDeadline = Time.time + runRef.pathUpdateDelay;
                runRef.agent.SetDestination(target.position);
                
            }
            //runRef.anim.SetBool("isAttacking", canAttack);
        }
        
        

    }
    //turns enemy tpwards target for attack, also stops enemy movement during attack
    void LookAndAttack(Transform target)
    {

        //runRef.anim.SetBool("isRunning", false);

        Vector3 lookPosition = target.position - transform.position;
        lookPosition.y = 0;
        Quaternion rotate = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, 0.2f);

        runRef.anim.SetBool("isAttacking", true);
        if (runRef.anim.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            hurtboxStatus = true;
            runRef.agent.speed = 0f;
            
        } else
        {
            hurtboxStatus = true;
            runRef.agent.speed = enemySpeed;
        }
        ToggleHurtboxes();
    }
    //turns on and off hurtboxes to not interfere with movement and collisions
    void ToggleHurtboxes()
    {

        if (hurtboxStatus)
        {
            runRef.lHand.enabled = true;
            runRef.rHand.enabled = true;
            runRef.lSpike.enabled = true;
            runRef.rSpike.enabled = true;
        } 
        else
        {
            runRef.lHand.enabled = false;
            runRef.rHand.enabled = false;
            runRef.lSpike.enabled = false;
            runRef.rSpike.enabled = false;
        }

    }
}
