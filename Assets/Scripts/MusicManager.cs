using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : AudioManager<MusicManager>
{
    [SerializeField] private List<AudioClip> music;
    // Flag to be used when there's more music in the soundtrack and we want to switch to the next one when the current one is done
    [SerializeField] private bool playNextWhenDone;
    
    private AudioSource source;

    [SerializeField] private AudioMixer mixer;

    protected override AudioMixer GetMixer()
    {
        return mixer;
    }
    
    protected override string PlayerPrefTag()
    {
        return "musicVolume";
    }
    
    protected override void Start()
    {
        base.Start();
        
        source = GetComponent<AudioSource>();
        source.clip = music.Random();
        source.loop = !playNextWhenDone;
        source.Play();

        if (playNextWhenDone)
        {
            StartCoroutine(WaitForMusicToFinish(source.clip.length));
        }
    }

    IEnumerator WaitForMusicToFinish(float length)
    {
        yield return new WaitForSeconds(length);
        source.clip = music.Where(x => x != source.clip).ToList().Random();
        source.Play();
    }
}
