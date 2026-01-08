using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private AudioSource soundFXObject;
    [SerializeField]
    private AudioResource RandAudio;

    private void Awake()
    {
        //singleton moments
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform)
    {
        //spawn the game object
        AudioSource audioSource =Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign clip
        audioSource.clip = audioClip;

        //play sound
        audioSource.Play();

        //get sfx clip length
        float clipLength = audioSource.clip.length;

        //destroy game object once clip is done playing
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClip(AudioResource audioResource, Transform spawnTransform)
    {
        //spawn the game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign clip
        audioSource.resource = audioResource;

        //play sound
        audioSource.Play();

        //arbitrary time until the game object explodes
        float clipLength = .5f;

        //destroy game object once clip is done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
