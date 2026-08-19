using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public AudioSource PlaySoundFXClip(AudioClip audioClip, Transform spawn, float volume)
    {
        AudioSource source = Instantiate(soundFXObject, spawn.position, Quaternion.identity);

        source.clip = audioClip;

        source.volume = volume;

        source.Play();

        float clipLength = source.clip.length;

        Destroy(source.gameObject, clipLength);

        return source;
    }
}
