using UnityEngine;
using UnityEngine.AI;

public class ShooterReferences : MonoBehaviour
{

    public Animator anim;
    public NavMeshAgent agent;

    [Header("Attack Colliders")]
    public SphereCollider head;
    public CapsuleCollider body;
    public CapsuleCollider legs;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
}
