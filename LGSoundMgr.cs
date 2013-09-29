using UnityEngine;
using System.Collections.Generic;

	/*MEMO  (LIO):
	 * I wanted to be able the create multiple channel of sfx and
	 * be able to control the sound parameter on them.
	 * As long as we just wana manage the sound volume, using PlayClipAtPoint it is fine.
	 * But if we need to control other parameters like the pich we are going to need to implement a Queue system. 
	 * //NOTE: Changing the volume during a fadeIn fadeOut could create a weird effect
	 * */
// TODO: need queuing system:
//   - smooth changes from fading out sound to fading in sound
//	 - fading out of a sound that is still fading in
//   - fading in of a sound that is still fading out

[RequireComponent(typeof(AudioSource))]
public class LGSoundMgr : MonoBehaviour
{
	//BGM defaul channel
	public static int DEFAULT_BGM_SOUND_CHANEL = 0;
	
	#region Class LGAudioClip
	/*
	 * LGAudioClip Parameter management class
	 */
	private class LGAudioClip{
		private AudioClip _audioClip;   // The audio clip
		/* 
		 * Note: The channel this sound source have to be play 
		 * Hope you don't intend to play the same source on diferent channel
		 */
		private int _channel;			
		
		public LGAudioClip(string soundId,string resourcePath,int channel){
			_channel = channel;
			
			/*Doing this will load the clip on register
			 *Witch may be safer to detect memory over but will cause a slower loading
			 *if this is a problem I could change this to load on the fist access 
			*/
			_audioClip = Resources.Load (resourcePath, typeof(AudioClip)) as AudioClip;		
		}
		
		
		public AudioClip Clip {
			get{ return _audioClip;}
		}
		
		public int Channel {
			get{ return _channel;}
		}
	}
	#endregion
	
	#region Class LGAudioChannel
	/*
	 * LGAudioChannel Parameter management class
	 * As we use PlayOneShotClip
	 * Now the only parameter that i want to manage is the volume but 
	 * I may need other AudioSource parameter later if we move to a gameObject Queue
	 */
	private class LGAudioChannel{
		
		float _volume; //The volume for the channel (0.0f- 1.0f)
		public float Volume {
			set{
				_volume = value;
			}
			get{
				return _volume;
			}
		}
		
		public LGAudioChannel(){
			_volume = 1.0f; //Set the max as default
		}
		
	}
	#endregion
	
	#region Sound clip registration
	
	/// <summary>
	/// Registers the soundId with a given resource.
	/// </summary>
	public static void RegisterSoundId (string soundId, string resourcePath)
	{
		RegisterSoundId(soundId,resourcePath,DEFAULT_BGM_SOUND_CHANEL);
	}
	
	/// <summary>
	/// Registers the soundId with a given resource, and set the channel to play it
	/// </summary>
	public static void RegisterSoundId (string soundId, string resourcePath, int channel)
	{
		if (Instance._soundIdLUT.ContainsKey (soundId)) {
			Debug.LogError ("SoundId [" + soundId + "] already registered.");
			return;
		}
		Instance._soundIdLUT.Add (soundId, new LGAudioClip(soundId,resourcePath,channel));
		
		//Channel (0) is the BGM channel create a new one if we need to
		if(Instance.GetAudioChannel(channel) == null){
			Instance._audioChannelParamL[channel] = new LGAudioChannel();		
		}	
	}
	
	
	#endregion
	
	#region Sound effect call (SFX)
	/// <summary>
	/// Plays a given soundId at a given position using a system AudioSource
	/// and "PlayOneShot".
	/// </summary>
	public static void PlayOneShotClip (string soundId, Vector3 pos)
	{
		LGAudioClip clip = Instance.GetClip (soundId);
		if (clip != null) {	
			LGAudioChannel channel = Instance.GetAudioChannel(clip.Channel);
			AudioSource.PlayClipAtPoint (clip.Clip, pos,channel.Volume);
			SoundLog ("Play: [" + soundId + "]");
		}
	}

	/// <summary>
	/// Plays a given sound using "PlayOneShot". Position is undefined, so will
	/// only work properly for 2D sounds.
	/// </summary>
	public static void PlayOneShotClip (string soundId)
	{
		PlayOneShotClip (soundId, Vector3.zero);
	}
	
	#endregion
	
	#region Background music (BGM)
	/// <summary>
	/// Is background music currently playing?
	/// </summary>
	public static bool IsBgmPlaying {
		get { return Instance._audioChannelBgm.isPlaying; }
	}
	
	/// <summary>
	/// Play background music.
	/// </summary>
	public static void PlayBgm (string soundId)
	{
		LGSoundMgr instance = Instance;
		LGAudioClip clip = instance.GetClip (soundId);
		if (clip != null) {
			instance._audioChannelBgm.clip = clip.Clip;
			instance._audioChannelBgm.Play ();	
			SoundLog ("Play BGM: [" + soundId + "]");
		}
	}
	
	/// <summary>
	/// Play background music with a fade-in effect.
	/// </summary>
	public static void PlayBgmWithFadeIn (string soundId, float fadeTimeInSeconds)
	{
		LGSoundMgr instance = Instance;
		if (instance._workMode != WorkMode.None) {
			Debug.LogWarning ("Busy. Cannot currently fade-in a sound.");
			return;
		}
		
		LGAudioClip clip = instance.GetClip (soundId);
		if (clip != null) {
			instance._totalFadeTimeInSeconds = fadeTimeInSeconds;
			instance._elapsedTimeInSeconds = 0.0f;
			instance._workMode = WorkMode.FadeIn;
			
			instance._audioChannelBgm.clip = clip.Clip;
			
			// Keep the volume off at start.
			instance._audioChannelBgm.volume = 0.0f;
			instance._audioChannelBgm.Play ();
			
			SoundLog ("Play BGM: [" + soundId + "] w/ fade-in.");
		}
	}
	
	/// <summary>
	/// Immediately stop the background music.
	/// </summary>
	public static void StopBgm ()
	{
		Instance._audioChannelBgm.Stop ();
		Instance._workMode = WorkMode.None;
		
		SoundLog ("Stop BGM.");
	}

	/// <summary>
	/// Stop the background music with a fade-in effect.
	/// </summary>
	public static void StopBgmWithFadeOut (float fadeTimeInSeconds)
	{
		LGSoundMgr instance = Instance;
		if (instance._workMode != WorkMode.None) {
			// TODO: fix this to prevent sound from going to max volume before it fades out
//			Debug.LogWarning ("Busy. Cannot currently fade-out a sound.");
//			return;
		}
		
		instance._totalFadeTimeInSeconds = fadeTimeInSeconds;
		instance._elapsedTimeInSeconds = 0.0f;
		instance._workMode = WorkMode.FadeOut;
		
		SoundLog ("Stop BGM w/ fade-out.");
	}
	
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	private static void SoundLog (object msg)
	{
		//Debug.Log (msg);
	}
	#endregion
	
	#region Sound channel control
	public static void SetSoundChannelVolume(int channelId, float volume){
		LGSoundMgr instance = Instance;
		LGAudioChannel channel =  instance.GetAudioChannel(channelId);
		if(channel != null){
			channel.Volume = volume;
			instance._audioVolumeChanged = true;
		}
	}                                                                                                                               
	#endregion
	
	#region Commun implementation.

	private enum WorkMode
	{
		None,
		FadeIn,
		FadeOut
	}

	private WorkMode _workMode = WorkMode.None;
	private float _totalFadeTimeInSeconds = 1.0f;
	private float _elapsedTimeInSeconds = 0.0f;
	private AudioSource _audioChannelBgm;
	bool _audioVolumeChanged;
	

	/// <summary>
	/// SoundId / AudioClipResource map.
	/// </summary>
	private Dictionary<string, LGAudioClip> _soundIdLUT = new Dictionary<string, LGAudioClip> ();
	
	/// <summary>
	/// AudioChannel / AudioChannelParamter map.
	/// </summary>
	private Dictionary<int, LGAudioChannel> _audioChannelParamL = new Dictionary<int, LGAudioChannel> ();
	
	/// <summary>
	/// Gets an AudioClip from a given soundId.
	/// </summary>
	private LGAudioClip GetClip (string soundId)
	{
		LGAudioClip clip;
		if (_soundIdLUT.TryGetValue (soundId, out clip)) {
			return clip;
		}
		Debug.LogError ("SoundId [" + soundId + "] not found.");
		return null;
	}
	
	/// <summary>
	/// Gets The Audio Channel
	/// </summary>
	private LGAudioChannel GetAudioChannel(int channelNum){
		LGAudioChannel channel;
		if (_audioChannelParamL.TryGetValue (channelNum, out channel)) {
			return channel;
		}
		return null;
	}
	
	#endregion

	#region Monobehavior
	protected virtual void Awake ()
	{
		// Only allow single instance to exist.
		if (_instance != null && _instance != this) {
			Destroy (gameObject);
			return;
		}
		
		Instance = this;
		DontDestroyOnLoad (gameObject);
		
		// Used for BGM, so we always want it to loop.
		_audioChannelBgm = GetComponent<AudioSource> ();
		_audioChannelBgm.loop = true;
		
		//force the update of the volume on the next update (Maybe not necessary)
		_audioVolumeChanged = true;
	
		//Channel Zero is alway the default channel (0) -> TODO: I have to check how this work on stage load
		_audioChannelParamL[0] = new LGAudioChannel();
	}

	protected virtual void Update ()
	{
		
		//NOTE: Changing the volume during a fadeIn fadeOut could create a weird effect
				
		// Fade in processing.
		if (_workMode == WorkMode.FadeIn) {
			
			LGAudioChannel bgmChannel =  GetAudioChannel(DEFAULT_BGM_SOUND_CHANEL);
			
			float currentVolume = Mathf.Lerp (0.0f, bgmChannel.Volume, _elapsedTimeInSeconds / _totalFadeTimeInSeconds);
			_audioChannelBgm.volume = currentVolume;
			
			_elapsedTimeInSeconds += Time.deltaTime;
			
			if (_elapsedTimeInSeconds > _totalFadeTimeInSeconds) {
				_workMode = WorkMode.None;
				
				// Make sure the volume is full.
				_audioChannelBgm.volume = bgmChannel.Volume;
			}
			// Fade out processing.
		} else if (_workMode == WorkMode.FadeOut) {
			
			LGAudioChannel bgmChannel =  GetAudioChannel(DEFAULT_BGM_SOUND_CHANEL);
			
			float currentVolume = Mathf.Lerp (bgmChannel.Volume, 0.0f, _elapsedTimeInSeconds / _totalFadeTimeInSeconds);
			_audioChannelBgm.volume = currentVolume;
			
			_elapsedTimeInSeconds += Time.deltaTime;
			
			if (_elapsedTimeInSeconds > _totalFadeTimeInSeconds) {
				_workMode = WorkMode.None;
				
				// Stop the sound completely.
				_audioChannelBgm.Stop ();
	
				// Reset the volume.
				_audioChannelBgm.volume = bgmChannel.Volume;
			}
		} else if(_audioVolumeChanged){
			LGAudioChannel bgmChannel =  GetAudioChannel(DEFAULT_BGM_SOUND_CHANEL);
			_audioChannelBgm.volume =  bgmChannel.Volume;			
			_audioVolumeChanged = false;
		}	
	}
	
	private void OnDestroy ()
	{
		Instance = null;
	}
	#endregion
	
	#region Singleton implementation.
	private static LGSoundMgr _instance;

	public static LGSoundMgr Instance {
		get { return _instance; }
		private set { UzuUtil.SingletonSet<LGSoundMgr> (ref _instance, value); }
	}
	#endregion

}