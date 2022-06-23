using UnityEngine;
using UnityEngine.Audio;

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