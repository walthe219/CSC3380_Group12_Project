using System.Collections.Generic;
using UnityEngine;
using System;


public static class TilePooling 
{
  
    static UnityEngine.Object[] prefab_arr;

    static List<Stack<GameObject>> tilePool = new List<Stack<GameObject>>();
    static int TilesPerPool = 4;

    static GameObject TilePoolObj;

    static Vector3 resetPosition;


    public static void initialize(string prefabFolderPath, Vector3 reset)
    {
        resetPosition = reset;
        TilePoolObj = new GameObject("TilePool");

        prefab_arr = Resources.LoadAll(prefabFolderPath, typeof(GameObject));

        foreach (GameObject tilePrefab in prefab_arr)
        {
            Stack<GameObject> tileStack = new Stack<GameObject>();
            for(int i = 0; i < TilesPerPool; i++)
            {
                GameObject tile = GameObject.Instantiate(tilePrefab, reset, Quaternion.identity, TilePoolObj.transform);
                //tile.SetActive(false);
                tileStack.Push(tile);
            }
            tilePool.Add(tileStack);
        }
        print();
    }
    public static (GameObject,Stack<GameObject>) pullTile(int i)
    {
        var tileStack = tilePool[i];
        var tile = tileStack.Pop();
        return (tile, tileStack);
    }

    public static (GameObject, Stack<GameObject>)[] pullRandonTiles(int num)
    {
        List<int> ints = new List<int>();
        (GameObject, Stack<GameObject>)[] tiles = new (GameObject, Stack<GameObject>)[num];

        for (int i = 0; i < tilePool.Count; i++)
        {
            ints.Add(i);
        }
        for(int i = 0; i < num; i++)
        {
            int r = UnityEngine.Random.Range(0, ints.Count);
            tiles[i] = pullTile(ints[r]);
            ints.RemoveAt(r);
        }

        Debug.Log($"RandTiles:[{string.Join(",",tiles)}] ");
        return tiles;
    }

    public static void reclaimTile(GameObject tile, Stack<GameObject> tileStack)
    {
        tile.transform.parent = TilePoolObj.transform;
        tile.transform.position = resetPosition;
        tile.transform.rotation = Quaternion.identity;
        tile.transform.localScale = Vector3.one;
        //tile.SetActive(false);
        
        tileStack.Push(tile);
    }

    static void print()
    {
        int i = 0;
        foreach(var stack in tilePool)
        {

            List<String> names = new List<String>();
            foreach(var tile in stack)
            {
                names.Add(tile.name);
            }
            Debug.Log($"TileStack {i++}: [{String.Join(", ", names)}]");
        }

    }
}