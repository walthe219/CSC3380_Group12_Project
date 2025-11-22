using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TagSearcher
{
    private List<GameObject> objs = new List<GameObject>();
    private int depth;

    void FindTaggedObject(string tag, Transform parent, int deep)
    {

        for (int i = 0; i < parent.childCount; i++)
        {

            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                objs.Add(child.gameObject);
            }

            if (depth != deep)
            {
                FindTaggedObject(tag, child, deep + 1);
            }
        }

    }

    public GameObject[] search(string tag, Transform parent, int depth)
    {
        this.depth = depth;
        FindTaggedObject(tag, parent, 0);
        return objs.ToArray();
    }
}
