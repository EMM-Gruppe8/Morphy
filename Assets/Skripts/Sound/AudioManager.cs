using UnityEngine.Audio;
using System;
using UnityEngine;

// Source: https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour
{
	/// <summary>
  /// Current AudioManager interface. Needed to enforce singeton behaviour
  /// </summary>
	public static AudioManager instance;

	/// <summary>
  /// Unity Audio Mixer Group
  /// </summary>
	public AudioMixerGroup mixerGroup;

	/// <summary>
  /// A list of sounds that can be played during the game
  /// </summary>
	public Sound[] sounds;

	void Start()
	{
		Play("Background");
	}

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
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	/// <summary>
  /// Play a sound using the AudioManager
  /// </summary>
  /// <param name="sound">Name of the sound as defined in the Sounds array</param>
	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		Debug.Log("Playing: " + sound);

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

}
