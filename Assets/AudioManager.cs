using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
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

    private Dictionary<AudioName, SoundInfo> audioDictionary = new Dictionary<AudioName, SoundInfo>();
    private List<AudioName> BGMList = new List<AudioName>();
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
            audioSource.pitch = sound.pitch;
            sound.source = audioSource;
            sound.songLength = audioSource.clip.length;
            if (audioDictionary.ContainsKey(sound.audioName))
            {
                Debug.LogError("Duplicate Enum key " + sound.audioName);
            }

            if (sound.isBGM) BGMList.Add(sound.audioName);
            
            audioDictionary[sound.audioName] = sound;
        }

        StartCoroutine(ShuffleBGM());
    }

    public AudioClip PlaySound(AudioName audioName)
    {
        var source = audioDictionary[audioName];
        source.source.Play();

        return source.audioClip;
    }

    IEnumerator ShuffleBGM()
    {
        while (true)
        {
            var audioClip = PlaySound(BGMList[Random.Range(0, BGMList.Count)]);
            yield return new WaitForSecondsRealtime(audioClip.length);
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

    [Range(-3f, 3f)] public float pitch = 1;
    public AudioManager.AudioName audioName;
    public AudioSource source;
    public float songLength;
    public bool isBGM = false;
}
