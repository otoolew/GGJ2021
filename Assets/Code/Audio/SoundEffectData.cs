using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName =	"newSoundEffect", menuName = "SoundEffect")]
public class SoundEffectData : ScriptableObject
{
    [SerializeField] private AudioClip audioClip;
    public AudioClip AudioClip { get => audioClip; set => audioClip = value; }

	public void Play(AudioSource source)
	{
		source.clip = audioClip;
		source.Play();
	}
}
