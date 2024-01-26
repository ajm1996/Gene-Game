using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;


	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	public void Play(string name)
	{
		Sound s = Array.Find(sounds, item => item.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		if (Time.time >= s.lastTimePlayed + s.timeThreshold) {
			s.source.Play();
			s.lastTimePlayed = Time.time;
		}
	}

	 public void StopPlaying (string sound)
		{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
		Debug.LogWarning("Sound: " + name + " not found!");
		return;
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;

		s.source.Stop ();
		}

}


