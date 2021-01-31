using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }

    [SerializeField] private SoundEffectData soundEffectData;
    public SoundEffectData SoundEffectData { get => soundEffectData; set => soundEffectData = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        soundEffectData.Play(audioSource);
    }
}
