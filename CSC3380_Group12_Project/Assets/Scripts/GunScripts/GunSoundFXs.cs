using UnityEngine;

public class GunSoundFXs : MonoBehaviour
{
    [SerializeField] AudioClip gunSound;
    [SerializeField] AudioClip[] gunHitEnemySounds;
    [SerializeField] AudioClip[] gunHitEnemyHeadSounds;
    [SerializeField] AudioClip reloadSound;


    private void Start()
    {
        
        if (SoundFXManager.instance != null)
        {
            GunScript.OnBulletFired += playGunFireSFX;
            GunScript.OnStartReload += playReloadSFX;
            GunScript.OnTargetHit += playEnemyHit;
        }
        else
        {
            this.enabled = false;
        }
    }

    void playReloadSFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(reloadSound, transform, 1f);
    }

    void playGunFireSFX()
    {
        SoundFXManager.instance.PlaySoundFXClip(gunSound, transform, 1f);
    }

    void playEnemyHit(RaycastHit NOTUSED)
    {
        int random = Random.Range(0, gunHitEnemySounds.Length);
        SoundFXManager.instance.PlaySoundFXClip(gunHitEnemySounds[random], transform, 1f);
    }
}
