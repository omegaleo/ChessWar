using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SFXAssociation
{
    public string name;
    public AudioClip clip;
}

public class SFXManager : AudioManager<SFXManager>
{
    [SerializeField] private List<SFXAssociation> sfx = new List<SFXAssociation>()
    {
        new SFXAssociation() {name = "pieceMoveSFX"}
    };

    protected override string PlayerPrefTag()
    {
        return "sfxVolume";
    }

    [SerializeField] private AudioMixer mixer;

    protected override AudioMixer GetMixer()
    {
        return mixer;
    }

    private AudioMixerGroup GetMixerGroup()
    {
        return mixer.FindMatchingGroups("SFX")[0];
    }
    
    public void Play(string clipName)
    {
        // Create a new Audio Source
        AudioSource extraSource = gameObject.AddComponent<AudioSource>();
        var clip = sfx.FirstOrDefault(x => x.name == clipName);

        if (clip != null)
        {
            extraSource.clip = clip.clip;
            extraSource.loop = false;
            extraSource.outputAudioMixerGroup = GetMixerGroup();
            extraSource.Play();
            StartCoroutine(WaitForSoundToFinish(clip.clip.length, extraSource));
        }
        
    }

    private IEnumerator WaitForSoundToFinish(float clipLength, AudioSource source)
    {
        yield return new WaitForSeconds(clipLength);
        Destroy(source);
    }
}
