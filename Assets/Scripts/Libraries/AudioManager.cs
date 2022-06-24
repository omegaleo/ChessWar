using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Main class used to instance Controllers for Audio(e.g. Music and Sound Effects), implements methods that make it easier to manage the Audio Mixer Groups
/// </summary>
/// <typeparam name="T"></typeparam>
public class AudioManager<T>:InstancedBehaviour<T> where T:AudioManager<T>
{
    protected virtual string PlayerPrefTag()
    {
        return "";
    }
    
    protected virtual void Start()
    {
        SetVolume(GetVolume());
    }

    protected virtual AudioMixer GetMixer()
    {
        return null;
    }

    public float GetVolume()
    {
        float volume = -6f;

        if (PlayerPrefs.HasKey(PlayerPrefTag()))
        {
            volume = PlayerPrefs.GetFloat(PlayerPrefTag());
        }

        return volume;
    }
    
    public void SetVolume(float volume)
    {
        GetMixer().SetFloat(PlayerPrefTag(), volume);
        PlayerPrefs.SetFloat(PlayerPrefTag(), volume);
    }

    public void Mute()
    {
        GetMixer().SetFloat(PlayerPrefTag(), -80f);
    }
}