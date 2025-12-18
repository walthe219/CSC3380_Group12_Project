using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    //tutorial: https://youtu.be/MdOi9ymb07s?si=BoIQ8mIhSsAbsHEb

    public bool CthulhuUnlocked;
    public GameObject DialogueBox;

    void OnEnable()
    {
        UnlockFunctions.UnlockCthulhuEvent += unlockCthulhu;
    }

    void unlockCthulhu()
    {
        CthulhuUnlocked = true;
    }

    void Start() { 
        CthulhuUnlocked = false;
    }

    private IEnumerator ShowDialogueForSeconds(float duration)
    {
        DialogueBox.SetActive(true);
        yield return new WaitForSeconds(duration);
        DialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CthulhuUnlocked)
        { 
            PlayerInteract();
        }
        if(Input.GetKeyDown(KeyCode.E) && !CthulhuUnlocked)
        {
            Debug.Log("The statue whispers through the thunder");
            StartCoroutine(ShowDialogueForSeconds(3f));
        }
    }

    public void PlayerInteract(){

        var layermask0 = 1 << 0; //Transparent FX Layer
        var layermask3 = 1 << 3; //Ground Layer
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
