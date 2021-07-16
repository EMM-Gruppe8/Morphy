using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// An audio clip for the Audio Manager
/// </summary>
[System.Serializable]
public class Sound {
	/// <summary>
  /// Name of the audio clip used to reference it in the audio manager
  /// </summary>
	public string name;

	/// <summary>
  /// Audio Clip to play
  /// </summary>
	public AudioClip clip;

	/// <summary>
  /// Volume the clip should have
  /// </summary>
	[Range(0f, 1f)]
	public float volume = .75f;

	/// <summary>
  /// Optional volume variance to play the sound at difference volumes randomly
  /// </summary>
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	/// <summary>
  /// Pitch of the sound. 1 for unchanged pitch
  /// </summary>
	[Range(.1f, 3f)]
	public float pitch = 1f;

	/// <summary>
  /// Optional pitch variance to play the sound at difference pitch randomly
  /// </summary>
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	/// <summary>
  /// Should the sound loop indefinately?
  /// </summary>
	public bool loop = false;

	/// <summary>
  /// Mixer group to use for this sound
  /// </summary>
	public AudioMixerGroup mixerGroup;

	/// <summary>
  /// AudioSource used across sounds - managed by the AudioManager
  /// </summary>
	[HideInInspector]
	public AudioSource source;

}
