using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;


    public void SetMusicVolume(float value)
    {
        SetVolume("musicVol", value);
    }

    public void SetSfxVolume(float value)
    {
        SetVolume("sfxVol", value);
    }

    private void SetVolume(string parameter, float value)
    {
        value = value == -10 ? -80 : value;
        mixer.SetFloat(parameter, value);
    }
}
