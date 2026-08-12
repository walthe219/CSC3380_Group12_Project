using UnityEngine;
using UnityEngine.AI;

public class DasherReference : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;

    [Header("Attack and Damage Colliders")]
    public Collider head;
    public Collider body;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

}
