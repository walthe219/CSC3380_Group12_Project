using UnityEngine;
using UnityEngine.AI;

public class DasherReference : MonoBehaviour
{
    [Header("References")]
    public Animator anim;

    [Header("Targets")]
    public Target target;
    public SubTarget head;
    public SubTarget body;
    public SubTarget legs;
    public SubTarget back;
    public SubTarget guard;

    [Header("Scripts")]
    public DasherBehavior behavior;
    public DasherGuard guardScript;
    public DasherAttack dashAttack;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

}
