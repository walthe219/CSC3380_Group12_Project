using UnityEngine;

public class GunIconsManager : MonoBehaviour
{
    [SerializeField] GameObject SemiToggle_icon;
    [SerializeField] GameObject AutoToggle_icon;
    [SerializeField] GameObject reload_icon;

    void Start()
    {
        GunScript.OnStartReload += showReload;
        GunScript.OnFinishReload += hideReload;
        GunScript.OnToggleAutoFire += toggleAutoFire;

        SemiToggle_icon.SetActive(true);
        AutoToggle_icon.SetActive(false);
    }

    void showReload()
    {
        reload_icon.SetActive(true);
    }
    void hideReload()
    {
        reload_icon.SetActive(false);
    }

    void toggleAutoFire(bool isAuto)
    {
        SemiToggle_icon.SetActive(!isAuto);
        AutoToggle_icon.SetActive(isAuto);
    }
    
}
