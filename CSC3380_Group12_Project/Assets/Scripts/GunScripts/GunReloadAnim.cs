using UnityEngine;
using System.Collections;

public class GunReloadAnim : MonoBehaviour
{
    public float rate;
    private bool midReload;
    private bool isReloading;

    private void OnEnable()
    {
        GunScript.OnStartReload += () => isReloading = true;
        GunScript.OnFinishReload += () => isReloading = false;
    }

    void Update()
    {
        if(isReloading)
        {
            if(!midReload)
            {
                StartCoroutine(Reload());
            }
        }    
    }

    IEnumerator Reload()
    {
        midReload = true;
        float timeElapsed = 0;

        while (timeElapsed < rate)
        {
            float t = timeElapsed / rate;
            float rot = Mathf.Lerp(0, 360, t);
            transform.localRotation =  Quaternion.Euler(-rot, 0, 0);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0, 0, 0);
        midReload = false;
    }
}
