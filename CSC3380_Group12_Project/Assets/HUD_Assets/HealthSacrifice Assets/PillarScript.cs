using UnityEngine;

public class PillarScript : MonoBehaviour
{
    public Interact OpenFromInteraction;

    private void OnEnable(){
        if(OpenFromInteraction){
            OpenFromInteraction.GetinteractEvent.HasInteracted += OpenPillarMenu;
        }
    }

    private void OnDisable(){
        if(OpenFromInteraction){
            OpenFromInteraction.GetinteractEvent.HasInteracted -= OpenPillarMenu;
        }
    }

    public void OpenPillarMenu(){
        Debug.Log("Opened Pillar Menu");
    }
}
