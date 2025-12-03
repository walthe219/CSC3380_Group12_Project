using System.Collections.Generic;
using UnityEngine;

public class NodeFieldProcessor : MonoBehaviour
{
    public List<GameObject> fieldNodes = new List<GameObject>();

    public List<GameObject> visibleNodes = new List<GameObject>();
    public List<GameObject> invisibleNodes = new List<GameObject>();

    public GameObject optimalNode = null;
    public float updateTimer = 0;


    
    void FixedUpdate()
    {
        
        if (updateTimer < 2f)
        {
            updateTimer += Time.deltaTime;
        }
        else
        {
            updateTimer = 0;
            getOptimalNode();
        }

    }

    void getOptimalNode()
    {
        
        if (!(fieldNodes.Count > 0))
        {
            foreach (Transform node in transform)
            {
                if (node.gameObject.CompareTag("VisNode"))
                {
                    fieldNodes.Add(node.gameObject);
                }
            }
        }
        else
        {
            foreach (GameObject node in fieldNodes)
            {
                Debug.Log("in 1");
                if (node.GetComponent<VisInfo>().visScore > 0)
                {
                    Debug.Log("in 2");
                    visibleNodes.Add(node);
                }
            }

            int index = Random.Range(0, visibleNodes.Count);

            optimalNode = visibleNodes[index];
        }
    }
}
