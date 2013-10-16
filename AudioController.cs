using UnityEngine;
using System.Collections;

namespace Uzu
{
	/// <summary>
	/// Configuration for an audio controller.
	/// </summary>
	public struct AudioControllerConfig
	{
		public int AudioSourceMaxCount { get; set; }
		
		public AudioLoader AudioLoader { get; set; }
	}
	
	/// <summary>
	/// Handles playing of audio.
	/// </summary>
	public class AudioController : BaseBehaviour
	{		
		public struct PlaySettings
		{
			public bool Loop;
			public Vector3 Position;
			public float Volume;
		}
		
		/// <summary>
		/// Initializes the audio controller.
		/// </summary>
		public void Initialize (AudioControllerConfig config)
		{
			// AudioSource allocation.
			{
				int maxCount = Mathf.Max (1, config.AudioSourceMaxCount);
				_availableSources = new FixedList<AudioSource> (maxCount);
				_activeSources = new FixedList<AudioSource> (maxCount);
				for (int i = 0; i < maxCount; i++) {
					GameObject go = new GameObject ("AudioSource_" + i);
					Transform xform = go.transform;
					xform.parent = this.CachedXform;
					AudioSource audioSource = go.AddComponent<AudioSource> ();
					ReturnSourceToPool (audioSource);
				}
			}
			
			_audioLoader = config.AudioLoader;
			
			if (_audioLoader == null) {
				Debug.LogError ("AudioLoader not set!");
			}
		}
		
		public void Play (string id, PlaySettings settings)
		{
			AudioClip clip = GetClip (id);
			if (clip != null) {
				AudioSource source = GetSource ();
				if (source != null) {
					source.clip = clip;
					source.loop = settings.Loop;
					source.transform.localPosition = settings.Position;
					source.volume = settings.Volume;
					source.Play ();
				}
			}
		}
		
		public bool IsPlaying (int channel)
		{
			return false;
		}
		
		public void Stop (int channel)
		{
			
		}
		
		#region Implementation.
		private AudioLoader _audioLoader;
		private FixedList<AudioSource> _availableSources;
		private FixedList<AudioSource> _activeSources;
		
		private AudioClip GetClip (string clipId)
		{
			if (_audioLoader == null) {
				Debug.LogWarning ("AudioLoader not registered.");
				return null;
			}
			
			AudioClip clip = _audioLoader.GetClip (clipId);
			if (clip == null) {
				Debug.LogWarning ("AudioClip id [" + clipId + "] not found.");
			}
			
			return clip;
		}
		
		private AudioSource GetSource ()
		{
			if (_availableSources.Count > 0) {
				int index = _availableSources.Count - 1;
				AudioSource source = _availableSources [index];
				_availableSources.RemoveAt (index);
				source.gameObject.SetActive (true);
				_activeSources.Add (source);
				return source;
			}
			
			Debug.LogWarning ("No AudioSources available.");
			return null;
		}
		
		private void ReturnSourceToPool (AudioSource source)
		{
			source.gameObject.SetActive (false);
			_availableSources.Add (source);
		}
		
		private void Update ()
		{
			// Clean up finished sounds and return to available pool.
			{
				for (int i = _activeSources.Count - 1; i >= 0; i--) {
					AudioSource source = _activeSources [i];
					if (!source.isPlaying) {
						_activeSources.RemoveAt (i);
						ReturnSourceToPool (source);
					}
				}
			}
		}
		#endregion
	}
}