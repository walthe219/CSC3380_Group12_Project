using UnityEngine;
using UnityEngine.AI;

public class RunnerReferences: MonoBehaviour
{

    public NavMeshAgent agent;
    public Animator anim;

    [Header("Attack and Damage Colliders")]
    public CapsuleCollider head;
    public CapsuleCollider body;
    public CapsuleCollider legs;
    public CapsuleCollider lHand;
    public CapsuleCollider rHand;
    public CapsuleCollider lSpike;
    public CapsuleCollider rSpike;

    [Header("Status Floats")]
    public float pathUpdateDelay = 0.2f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

}
