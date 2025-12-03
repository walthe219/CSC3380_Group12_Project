using System.Data;
using UnityEngine;

public class VisInfo : MonoBehaviour
{
    public int visScore;
    public float updateTimer = 0;
    public bool visible = false;

    public Transform player;

    void Update()
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
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit))
        {

            Debug.Log(hit.transform.name);
            
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("kill yourself");
                visScore = 1;
            }
        }

    }
}
