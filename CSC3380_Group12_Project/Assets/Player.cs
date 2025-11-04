using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) PlayerInteract();
    }

    public void PlayerInteract(){

        var layermask0 = 1 << 0;
        var layermask3 = 1 << 3;
        var finalmask = layermask0 | layermask3;

        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));

        

        if(Physics.Raycast(ray, out hit, 15, finalmask)){
            Debug.Log("Hit object: " + hit.transform.name);
            Interact InteractScript = hit.transform.GetComponent<Interact>();
            if(InteractScript) InteractScript.CallInteract(this);
        }
    }
}
