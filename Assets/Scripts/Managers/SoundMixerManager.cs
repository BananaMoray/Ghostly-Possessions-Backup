using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Instance;

    [SerializeField] 
    private AudioMixer _audioMixer;
    [SerializeField]
    private AudioResource _testSFX;

    private void Awake()
    {
        //singleton moments
        if (Instance == null)
            Instance = this;
    }

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20);
    }

    public void SetMasterPitch(float level)
    {
        _audioMixer.SetFloat("MasterPitch", level);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
    } 

    public void SetSFXVolume(float level)
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20);
        //SoundManager.Instance.PlaySoundFXClip(_testSFX, gameObject.transform);
        SoundManager.Instance.PlaySoundFXClip(_testSFX, transform);
    } 
}
