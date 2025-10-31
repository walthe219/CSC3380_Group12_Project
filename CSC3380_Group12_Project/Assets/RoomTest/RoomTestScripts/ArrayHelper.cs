using UnityEngine;

public static class ArrayHelper
{
    /*
    * Fisher-Yates Random in-place shuffle algorithm
    */
    public static Object[] Shuffle(Object[] arr)
    {
        int n = arr.Length;
        for (int i = n - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i);
            Object temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
        return arr;
    }

    public static Object[] Clone(Object[] arr)
    {
        Object[] cloned = new Object[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            cloned[i] = arr[i];
        }
        return cloned;
    }
    public static string print(Object[] arr)
    {
        if(arr.Length == 0)
        {
            return "[]";
        }
        string s = "["+arr[0].ToString()+" ";
        for (int i = 1; i < arr.Length; i++)
        {
            s += arr[i].ToString()+", ";
        }
        s = s.Substring(0, s.Length - 2) + "]";

        return s;
    }

}
