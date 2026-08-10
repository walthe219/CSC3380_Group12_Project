using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeSpawner : MonoBehaviour
{
    [SerializeField] bool grenadeUnlocked;

    [Header("Grendade")]
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] InputAction throwButton;

    [SerializeField][Min(0)] float throwStrength;
    [SerializeField][Min(0)] float grenadeCooldown;

    private Camera cam;
    private float elapsed;


    private void Start()
    {
        cam = Camera.main;
        throwButton = InputSystem.actions.FindAction("Throw");
        elapsed = grenadeCooldown;

        UnlockFunctions.UnlockGrenade += () => grenadeUnlocked = true;
    }

    private void Update()
    {
        if(elapsed < grenadeCooldown)
        {
            elapsed += Time.deltaTime;
            return;
        }

        if (throwButton.WasPressedThisFrame())
        {
            SpawnGrenade();
            elapsed = 0;
        }
    }

    private void SpawnGrenade()
    {
        if (!grenadeUnlocked) return;

        GameObject grenade = Instantiate(grenadePrefab);
        var rb = grenade.GetComponent<Rigidbody>();
        var script = grenade.GetComponent<GrenadeScript>();

        if (script == null)
            script = grenade.AddComponent<GrenadeScript>();

        rb.position = cam.transform.position + cam.transform.forward * 2f;
        grenade.transform.position = rb.position;

        rb.linearVelocity += cam.transform.forward * throwStrength;
    }
}
