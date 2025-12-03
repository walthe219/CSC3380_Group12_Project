using UnityEngine;
using UnityEngine.AI;

public class ShooterBehavior : MonoBehaviour
{

    public GameObject nearest;
    public NavMeshAgent agent;

    private float pathUpdateDeadline;
    private float attackDistance;
    private float shootTimer = 0f;

    public Transform playerTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTarget = GameObject.FindWithTag("Player").transform;
        attackDistance = agent.stoppingDistance;
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
        nearest = node.optimalNode;
        node.isOccupied = true;

    }

    void UpdatePath(Transform target)
    {

        if (target != null)
        {
            bool canAttack = Vector3.Distance(transform.position, target.position) <= attackDistance;
            if (canAttack)
            {
                LookAndAttack(playerTarget);
            }
            else
            {
                /*runRef.anim.SetFloat("runSpeed", 1, dampTime, Time.deltaTime);
                runRef.anim.SetBool("isAttacking", false);*/
            }
            //Mathf.Lerp(runRef.anim.GetFloat("speed"), runRef.agent.desiredVelocity.sqrMagnitude, Time.deltaTime*100)

            if (Time.time >= pathUpdateDeadline)
            {

                pathUpdateDeadline = Time.time + 0.2f;
                agent.SetDestination(target.position);

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
        }

        void Shoot()
        {

        }
    }
}
