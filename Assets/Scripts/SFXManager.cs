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

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public static SFXManager instance;

    [SerializeField] private List<SFXAssociation> sfx = new List<SFXAssociation>()
    {
        new SFXAssociation() {name = "pieceMoveSFX"}
    };
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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
            extraSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
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
