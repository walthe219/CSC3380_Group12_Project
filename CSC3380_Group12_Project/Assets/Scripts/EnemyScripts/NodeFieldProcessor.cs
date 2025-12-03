using System.Collections.Generic;
using UnityEngine;

public class NodeFieldProcessor : MonoBehaviour
{
    public List<GameObject> fieldNodes = new List<GameObject>();

    public List<GameObject> visibleNodes = new List<GameObject>();

    public GameObject optimalNode = null;
    public float updateTimer = 0;
    public bool activator = false;
    bool hasBeenActive = false;
    public bool isOccupied = false;

    public LayerMask playerMask;

    void FixedUpdate()
    {
        activator = Physics.CheckSphere(transform.position, 100f, playerMask);

        if (activator)
        {
            if (!hasBeenActive)
            {
                foreach (Transform node in transform)
                {
                    if (node.gameObject.CompareTag("VisNode"))
                    {
                        node.gameObject.SetActive(true);
                    }
                }
                hasBeenActive = true;
            }
            
            updaterFunc();
        }
        if (!activator)
        {
            foreach (Transform node in transform)
            {
                if (node.gameObject.CompareTag("VisNode"))
                {
                    node.gameObject.SetActive(false);
                }
            }
            hasBeenActive = false;
            isOccupied = false;
        }
    }
    
    void updaterFunc()
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
                var info = node.GetComponent<VisInfo>();
                if (info.visScore > 0 && !info.visible)
                {
                    visibleNodes.Add(node);
                    info.visible = true;
                }
                else if (!(info.visScore > 0) && info.visible)
                {
                    visibleNodes.Remove(node);
                    info.visible = false;
                }
            }

            int index = Random.Range(0, visibleNodes.Count-1);

            optimalNode = visibleNodes[index];
        }
    }
}
