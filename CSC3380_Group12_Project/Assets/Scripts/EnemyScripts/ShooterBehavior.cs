using UnityEngine;
using UnityEngine.AI;

public class ShooterBehavior : MonoBehaviour
{

    public GameObject nearest;
    public ShooterReferences shootRef;

    public float agentVelocity;

    private float pathUpdateDeadline;
    private float attackDistance;
    private float shootTimer = 0f;

    public Transform playerTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        attackDistance = shootRef.agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {

        findNearestField();
        UpdatePath(nearest.transform);
    }

    void findNearestField()
    {
        GameObject[] activeFields = GameObject.FindGameObjectsWithTag("NodeField");

        float nearDist = 10000f;
        float dist;
        nearest = null;
        foreach (GameObject field in activeFields)
        {
            dist = Vector3.Distance(transform.position, field.transform.position);

            if (dist < nearDist)
            {
                nearDist = dist;
                nearest = field;
            }
        }
        var node = nearest.GetComponent<NodeFieldProcessor>();

        if (node.optimalNode != null)
        {
            nearest = node.optimalNode;
            node.isOccupied = true;
        }
        

    }

    void UpdatePath(Transform target)
    {
        agentVelocity = shootRef.agent.desiredVelocity.sqrMagnitude;
        if (target != null)
        {
            LookAndAttack(playerTarget);

            Mathf.Lerp(shootRef.anim.GetFloat("speed"), shootRef.agent.desiredVelocity.sqrMagnitude, Time.deltaTime * 100);

            if (Time.time >= pathUpdateDeadline)
            {

                pathUpdateDeadline = Time.time + 0.2f;
                shootRef.agent.SetDestination(target.position);

            }
            //runRef.anim.SetBool("isAttacking", canAttack);
        }

        void LookAndAttack(Transform target)
        {
            Vector3 lookPosition = target.position - transform.position;
            lookPosition.y = 0;
            Quaternion rotate = Quaternion.LookRotation(lookPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, 0.2f);

            if (shootTimer < 3f)
            {
                shootTimer += Time.deltaTime;
            }
            else
            {
                shootTimer = 0;
                Shoot();
            }
            if (shootRef.anim.GetCurrentAnimatorStateInfo(0).IsName("Demon|Shoot1 0"))
            {
                shootTimer = 0;
            }
            else
            {
                shootTimer = 0;
                shootRef.anim.SetBool("isAttacking", false);
            }
            
        }

        void Shoot()
        {
            shootRef.anim.SetBool("isAttacking", true);
        }
    }
}
