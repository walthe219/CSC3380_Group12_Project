using UnityEngine;
using System.Collections;

public class TagSearcher
{
    private ArrayList objs = new ArrayList();
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

    public ArrayList search(string tag, Transform parent, int depth)
    {
        this.depth = depth;
        FindTaggedObject(tag, parent, depth);
        return objs;
    }
}
