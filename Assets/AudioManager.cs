using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundInfo[] Sounds;

    public enum AudioName
    {
        BGM_FixTheGear, 
        BGM_RavingEnergy,
        BGM_Werq,
        SFX_Powerup,
        SFX_Explosion,
        SFX_BigExplosion,
        SFX_Lazer
    }

    private void Awake()
    {
        foreach (var sound in Sounds)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound.audioClip;
            audioSource.loop = sound.isLooping;
            audioSource.outputAudioMixerGroup = sound.mixingGroup;
            audioSource.priority = sound.priority;
            audioSource.volume = sound.volume;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class SoundInfo
{
    public AudioClip audioClip;
    public AudioMixerGroup mixingGroup;
    public bool isLooping = false;
    [Range(0, 256)]
    public int priority = 128;
    [Range(0f, 1f)]
    public float volume = 1f;

    public AudioManager.AudioName audioName;
}
