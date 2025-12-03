using System.Data;
using UnityEngine;

public class VisInfo : MonoBehaviour
{
    public int visScore;
    public float updateTimer = 0;

    public Transform player;

    void FixedUpdate()
    {
        if (updateTimer < 2f)
        {
            updateTimer += Time.fixedDeltaTime;
        } 
        else
        {
            updateTimer = 0f;
            getVisibilityScore();
        }
    }

    void getVisibilityScore()
    {
        visScore = 0;
        RaycastHit hit;
        Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit);

        if (hit.transform.gameObject.CompareTag("Player"))
        {
            visScore = 1;
        }
    }
}
