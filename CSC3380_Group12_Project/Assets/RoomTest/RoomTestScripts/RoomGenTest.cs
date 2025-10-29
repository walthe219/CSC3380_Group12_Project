using UnityEngine;
using System.Collections.Generic;

public class RoomGenTest : MonoBehaviour
{
    public GameObject center;
    public GameObject topPoint;
    public GameObject rightPoint;
    public GameObject bottomPoint;
    public GameObject leftPoint;
    public List<GameObject> prefabs;

    public float max_room_radius = 10;


    private void Start()
    {
        Object[] prefab_arr = Resources.LoadAll("Floors", typeof(GameObject));
        List<Object> list = new List<Object>(prefab_arr);
        Shuffle<Object>(list);

        Queue<Object> test = new Queue<Object>(list);

        print(test);

        Instantiate(test.Dequeue(), topPoint.transform.position + new Vector3 (0,0,max_room_radius), topPoint.transform.rotation);
        Instantiate(test.Dequeue(), rightPoint.transform.position + new Vector3(max_room_radius, 0, 0), rightPoint.transform.rotation);
        Instantiate(test.Dequeue(), bottomPoint.transform.position + new Vector3(0, 0, -max_room_radius), bottomPoint.transform.rotation);
        Instantiate(test.Dequeue(), leftPoint.transform.position + new Vector3(-max_room_radius, 0, 0), leftPoint.transform.rotation);

    }

    public static void Shuffle<T>( List<T> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}

}
