using UnityEngine;

//Object will destroy itself after lifetime passes
public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float lifetime = 3.0f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    
}
