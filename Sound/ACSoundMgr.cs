using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// MEMO : Attach a sound source to object and destroy this object could cause a problem so I don't supported it for now
/// AC sound mgr. 
/// The Unity Word used for sound are kind of wierd for me so let presice a litle definition of the term i use in the class.
/// 
/// Clip : Sound clip is the sound data
/// Source : Sound source is the sound emiter in the scene
/// Category : The sound category is the Category of sound (BGM,SFX,ETC...) need this for control multyple sound in same time
/// 
/// 
/// How to use:
/// Had the manager to the scene (Cound add an auto load later)
/// Had a ACSoundDataLoader to the scene. And set the data you wana use in you scene to it
/// 
/// Once this is done you can use the 
/// 
/// PlaySoundClip
/// IsSoundSourceActive
/// MoveSoundSource
/// StopSoundSource
/// 
/// function to play and stop sound effect
/// 
/// 
/// </summary>



/// <summary>
/// AC audio source handle.
/// </summary>
#region Class ACAudioSourceHandle
public class ACAudioSourceHandle
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ACAudioSourceHandle"/> class.
	/// </summary>
	public ACAudioSourceHandle ()
	{
		Id = this.GetHashCode ();
	}
	
	/// <summary>
	/// Source state.
	/// </summary>
	public enum SourceState
	{
		Normal,
		FadeIn,
		FadeOut
	}
	
	/// <summary>
	/// Gets the state.
	/// </summary>
	/// <value>
	/// The state.
	/// </value>
	public SourceState State { get; protected set; }
	
	/// <summary>
	/// Gets the unique ID 
	/// </summary>
	/// <value>
	/// The identifier.
	/// </value>
	public int Id { get; private set; }
	
	/// <summary>
	/// Gets  whether this instance is active and valid.
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance is valid; otherwise, <c>false</c>.
	/// </value>
	public bool IsValid {
		get {
			return ACSoundMgr.IsSoundSourceActive (this);
		}
		private set{}
	}
}
#endregion

public class ACSoundMgr : LGBehaviour
{
	
	#region Class ACAudioClip
	/*
	 * ACAudioClip Parameter management class
	 */
	private class ACAudioClip
	{
		private AudioClip _audioClip;   // The audio clip
		/* 
		 * Note: The category this sound source have to be play 
		 * Hope you don't intend to play the same source on diferent category
		 */
		private int _category;
		
		public ACAudioClip (string soundId, string resourcePath, int category)
		{
 _category = category;
			_audioClip = Resources.Load (resourcePath, typeof(AudioClip)) as AudioClip;		
		}
		
		public ACAudioClip (AudioClip clip, int category)
		{
			_category = category;
			_audioClip = clip;		
		}
		
		public AudioClip Clip {
			get{ return _audioClip;}
		}
		
		public int Category {
			get{ return _category;}
		}
	}
	#endregion
	
	#region Class ACAudioCategoryParameter
	/*
	 * ACAudioCategoryParameter Parameter management class
	 * As we use PlayOneShotClip
	 * Now the only parameter that i want to manage is the volume but 
	 * I may need other AudioSource parameter later if we move to a gameObject Queue
	 */
	private class ACAudioCategoryParameter
	{
		
		float _volume; //The volume for the category (0.0f- 1.0f)
		public float Volume {
			set {
				_volume = value;
			}
			get {
				return _volume;
			}
		}
		
		public ACAudioCategoryParameter ()
		{
			_volume = 1.0f; //Set the max as default
		}
		
	}
	#endregion
		
	#region Class ACAudioSource
	private class ACAudioSource : ACAudioSourceHandle
	{
		/// <summary>
		/// The _source emiter object pool.
		/// </summary>
		private static LGGameObjectPool _sourceEmiterObjectPool;
		
		/// <summary>
		/// Gets the source emiter object pool.
		/// </summary>
		/// <value>
		/// The source emiter object pool.
		/// </value>
		private LGGameObjectPool SourceEmiterObjectPool {
			get {
				if (_sourceEmiterObjectPool == null) {
					//Create a dummy sound emiter object
					GameObject baseSoundEmiter = new GameObject ("SoundSource");
					AudioSource os = baseSoundEmiter.gameObject.AddComponent<AudioSource> ();
					os.loop = true;
					_sourceEmiterObjectPool = new LGGameObjectPool (baseSoundEmiter, DEFAULT_AUDIO_SOURCE_NUM);
					baseSoundEmiter.SetActive (false);
				}
				return _sourceEmiterObjectPool;
			}
		}
		
		/// <summary>
		/// The _emiter object.
		/// </summary>
		private GameObject _emiterObj;
		
		/// <summary>
		/// Gets the emiter object.
		/// </summary>
		/// <value>
		/// The emiter object.
		/// </value>
		public GameObject EmiterObject { get { return _emiterObj; } }
		
		/// <summary>
		/// The sound _clip to play.
		/// </summary>
		private ACAudioClip _clip;		
		
		/// <summary>
		/// The _source that play the sound.
		/// </summary>
		private AudioSource _source; 	
		
		/// <summary>
		/// Gets the source.
		/// </summary>
		/// <value>
		/// The source.
		/// </value>
		public AudioSource Source { get { return _source; } }
		
		/// <summary>
		/// Gets the clip.
		/// </summary>
		/// <value>
		/// The clip.
		/// </value>
		public ACAudioClip Clip { get { return _clip; } }
		
		/// <summary>
		/// The _volumefluctuation spead in volume by seconde (use for fade in fade out)
		/// </summary>
		float _volumefluctuationSpead;
		
		/// <summary>
		/// The _volume max.
		/// </summary>
		float _volumeMax;
		
		/// <summary>
		/// The _volume fade out minimum.
		/// </summary>
		float _volumeFadeOutMin;
		
		/// <summary>
		/// The _pause time.
		/// </summary>
		float _pausePich;
		bool _isPause;
		
		/// <summary>
		/// Pause this instance.
		/// </summary>
		public void Pause(){
			if(!_isPause)
			{
				//Memo Pause was buggin so I use Pitch instead
				_isPause = true;
				_pausePich =  _source.pitch;
				_source.pitch = 0.0f;
				//_source.Pause();
				//_source.enabled = false;
				//LGDbg.Assert(_pausePich != 0.0f);
				//Debug.Log(_pausePich);
			}
		}
		
		/// <summary>
		/// Unpause.
		/// </summary>
		public void UnPause(){
			_source.pitch = _pausePich;
			_isPause = false;
			//_source.enabled = true;
			//_source.Play();
			//_source.timeSamples = _pauseTime;
			//Debug.Log(_pauseTime);
		}
				
		/// <summary>
		/// Gets or sets the local volume.
		/// </summary>
		/// <value>
		/// The local volume.
		/// </value>
		float LocalVolume {
			set {
				_source.volume = value;
			
			}
			get {
				return _source.volume;
			}
		}
		
		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		/// <value>
		/// The volume.
		/// </value>
		public float Volume {
			set {
				_source.volume = value;
				_volumeMax = value; //Reset the max volume for fadein fadeou
			}
			get {
				return _source.volume;
			}
		}
		
		/// <summary>
		/// Attachs the sound source to an object (could cause problem is i delete the object)
		/// </summary>
		/// <param name='objectToAttachTo'>
		/// Object to attach to.
		/// </param>
		public void AttachTo (GameObject objectToAttachTo)
		{
			this._emiterObj.transform.parent = objectToAttachTo.transform;
			//Reset the local transform to match up the  object attach to
			this._emiterObj.transform.localPosition = Vector3.zero;
		}
		
		/// <summary>
		/// Moves the sound source to a new 3d position.
		/// </summary>
		/// <param name='moveToPos'>
		/// Move to position.
		/// </param>
		public void MoveTo (Vector3 moveToPos)
		{
			this._emiterObj.transform.position = moveToPos;
		}
		
		/// <summary>
		/// Fades the soundsource out.
		/// </summary>
		/// <param name='fadeOutTime'>
		/// Fade out time.
		/// </param>
		public void FadeOut (float fadeOutTime)
		{
			//Set the fulctuation speed
			_volumefluctuationSpead = (-(LocalVolume / fadeOutTime));
			State = SourceState.FadeOut;
		}
		
		/// <summary>
		/// Fades the sound source in.
		/// </summary>
		/// <param name='fadeInTime'>
		/// Fade in time.
		/// </param>
		public void FadeIn (float fadeInTime)
		{
			_volumefluctuationSpead = (_volumeMax / fadeInTime);
			State = SourceState.FadeIn;
		}
		
		/// <summary>
		/// Play the sound source.
		/// </summary>
		/// <param name='isLoop'>
		/// Is loop.
		/// </param>
		/// <param name='fadeInTime'>
		/// Fade in time.
		/// </param>
		public void Play (bool isLoop, float fadeInTime)
		{
			//Check if we need the fadeIn effect (I dont surport fade in form something else that zero now)
			if (fadeInTime > 0.0f) {
				_source.volume = 0.0f;
				FadeIn (fadeInTime);
			} else {
				_source.volume = _volumeMax;
			}
			_source.loop = isLoop;
		
			//Reset the pitch as it could have been change.
			_source.pitch = 1.0f;
			
			_source.Play ();
		}
		
		/// <summary>
		/// Stop the sound source.
		/// </summary>
		public void Stop ()
		{
			
			//Reset the pitch as it could have been change.
			_source.pitch = 1.0f;
			
			_source.Stop ();
			//Make sure the object is not attach to anyone.
			this._emiterObj.transform.parent = null;
			
			SourceEmiterObjectPool.Unspawn (_emiterObj);
		}
		
		/// <summary>
		/// Update the clip as playing (doing this to suport fade in fade out)
		/// </summary>
		public void Update ()
		{
			
			if (_volumefluctuationSpead != 0.0f) {
				
				LocalVolume += (_volumefluctuationSpead * Time.deltaTime);
				
				//	Debug.Log ("Audio source update - > Volume adjusting itself to :" + LocalVolume + "  / MAX : " + _volumeMax + "  Fluc " + _volumefluctuationSpead);
				
				//if the volume is smaler than zero stop the clip
				if (LocalVolume <= 0.0f && State == SourceState.FadeOut) {
					//If this work as expected stoping the source will make the the system clean it later
					this._source.Stop ();
					_volumefluctuationSpead = 0.0f;
					State = SourceState.Normal;
				}
				
				if (LocalVolume <= _volumeFadeOutMin) {
					_volumeFadeOutMin = 0.0f;
					_volumefluctuationSpead = 0.0f;
					State = SourceState.Normal;
				}
			
				if (LocalVolume >= _volumeMax) {
					LocalVolume = _volumeMax;
					_volumefluctuationSpead = 0.0f;
					State = SourceState.Normal;
				}
				
			}
		}
		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ACSoundMgr.ACAudioSource"/> class.
		/// </summary>
		/// <param name='clip'>
		/// Clip.
		/// </param>
		/// <param name='volumeMax'>
		/// Volume max.
		/// </param>
		/// <param name='position'>
		/// Position.
		/// </param>
		public ACAudioSource (ACAudioClip clip, float volumeMax, Vector3 position)
		{	
			_emiterObj = SourceEmiterObjectPool.Spawn (position);
			_source = _emiterObj.GetComponent<AudioSource> ();
			_source.clip = clip.Clip;
			_volumeMax = volumeMax;
			_volumefluctuationSpead = 0.0f;
			_clip = clip;
			_volumeFadeOutMin = 0.0f;
			State = SourceState.Normal;
		} 
	}
	#endregion
	
	#region Sound clip registration (Don't call this!! use the ACsoundDataLoader)
	/// <summary>
	/// Registers the soundId with a given resource, and set the category to play it
	/// </summary>
	public static void RegisterAudioClip (string id, AudioClip clip, int category)
	{
		if (Instance._soundClipDataHolder.ContainsKey (id)) {
			Debug.LogError ("SoundId [" + id + "] already registered.");
			return;
		}
		Instance._soundClipDataHolder.Add (id, new ACAudioClip (clip, category));
		//Category (0) is the BGM category create a new one if we need to
		if (Instance.GetAudioCategoryParam (category) == null) {
			Instance._audioCategoryParamL [category] = new ACAudioCategoryParameter ();		
		}	
	}	
	#endregion
	
	#region Sound clip play control
	/// <summary>
	/// Play background music.
	/// </summary>
	public static ACAudioSourceHandle PlaySoundClip (string soundId, bool isLoop, float fadeTimeInSecond = 0.0f)
	{
		return  PlaySoundClip (soundId, isLoop, Vector3.zero, fadeTimeInSecond);
	}
	
	/// <summary>
	/// Plays a given soundId at a given position using a system AudioSource
	/// TODO : This use playClipAtPoint mean that you can't stop source create with this
	/// I'll change this with a queu System
	/// and "PlayOneShot".
	/// </summary>
	public static ACAudioSourceHandle PlaySoundClip (string soundId, bool isLoop, Vector3 pos, float fadeTimeInSecond = 0.0f)
	{
		ACSoundMgr instance = Instance;
		ACAudioClip clip = instance.GetClip (soundId);
		if (clip == null) {
			Debug.LogError ("PlaySoundClip Invalid sound clip id. :" + soundId +
				" Check is you have the correct soundDataLoader in the scene");
			return null;
		}
	
		float clipMaxVolume = instance.GetAudioCategoryParam (clip.Category).Volume;
		//Get an AudioSoure and play the sound
		ACAudioSource acSource = new ACAudioSource (clip, clipMaxVolume, pos);
		acSource.Play (isLoop, fadeTimeInSecond);
		
		//If the manager is mute set the manager to reset the sound volume on the next update
		if (IsMute) {
			instance._audioVolumeChanged = true;
		}

		instance._usedAudioSource [acSource.Id] = acSource;
		//I don't want to expose the source so I down cast it.
		//Of course you could cast the handle to get the source but don't.
		return (ACAudioSourceHandle)acSource;
	}
	
	/// <summary>
	/// Determines whether this instance is clip playing the specified soundId.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is clip playing the specified soundId; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='soundId'>
	/// If set to <c>true</c> sound identifier.
	/// </param>
	public static bool IsSoundSourceActive (ACAudioSourceHandle soundSourcehandle)
	{
		if (soundSourcehandle == null) {
			return false;
		}
		//Get the instance
		return Instance._usedAudioSource.ContainsKey (soundSourcehandle.Id);
	}
	
	/*  TODO lio ....this may cause bug i'll support it later
	/// <summary>
	/// Attachs the sound source.
	/// </summary>
	/// <param name='soundSourcehandle'>
	/// Sound sourcehandle.
	/// </param>
	/// <param name='objectToAttachTo'>
	/// Object to attach to.
	/// </param>/
	public static void AttachSoundSource(ACAudioSourceHandle soundSourcehandle,GameObject objectToAttachTo){	
		if(soundSourcehandle == null){
			Debug.LogError("StopSoundSource Invalid Source handle");
			return;
		}
		
		//Get the instance
		ACSoundMgr instance = Instance;
		//Get the Audio source using the ID
		//I don't do a null check because if it working properly this wont happen.
		ACAudioSource acsource = instance._usedAudioSource [soundSourcehandle.Id];
		
		//Attach to the object
		acsource.AttachTo(objectToAttachTo);
	}
	*/
	
	/// <summary>
	/// Moves the sound source in 3d space.
	/// </summary>
	/// <param name='soundSourcehandle'>
	/// Sound sourcehandle.
	/// </param>
	/// <param name='MoveToPos'>
	/// Move to position.
	/// </param>
	public static void MoveSoundSource (ACAudioSourceHandle soundSourcehandle, Vector3 MoveToPos)
	{
		if (soundSourcehandle == null) {
			Debug.LogError ("StopSoundSource Invalid Source handle");
			return;
		}
		
		//Get the instance
		ACSoundMgr instance = Instance;
		//Get the Audio source using the ID
		//I don't do a null check because if it working properly this wont happen.
		ACAudioSource acsource = instance._usedAudioSource [soundSourcehandle.Id];
		
		//Attach to the object
		acsource.MoveTo (MoveToPos);
	}
	
	/// <summary>
	/// Stops the sound source.
	/// </summary>
	/// <param name='sourceHandle'>
	/// Source handle.
	/// </param>
	/// <param name='fadeTimeInSecond'>
	/// Fade time in second.
	/// </param>
	public static void StopSoundSource (ACAudioSourceHandle soundSourcehandle, float fadeTimeInSecond = 0.0f)
	{
		
		if (soundSourcehandle == null) {
			Debug.LogError ("StopSoundSource Invalid Source handle");
			return;
		}
		
		//Get the instance
		ACSoundMgr instance = Instance;
		//Get the Audio source using the ID
		//I don't do a null check because if it working properly this wont happen.
		ACAudioSource acsource = instance._usedAudioSource [soundSourcehandle.Id];
		
		//If the source is set to stop with fadeOut 
		//Start the fadeOut and return
		if (fadeTimeInSecond > 0.0f) {
			acsource.FadeOut (fadeTimeInSecond);
			return;
		}
		
		//Remove the source from the used list
		//Don't do it here this is cleaned up in the update()
		//instance._usedAudioSource.Remove (soundSourcehandle.Id);
		
		//Stope the sound
		acsource.Stop ();
	}
	
	
	/// <summary>
	/// Pause the sound source.
	/// </summary>
	/// <param name='sourceHandle'>
	/// Source handle.
	public static void PauseSoundSource (ACAudioSourceHandle soundSourcehandle)
	{
		
		if (soundSourcehandle == null) {
			Debug.LogError ("StopSoundSource Invalid Source handle");
			return;
		}
		
		//Get the instance
		ACSoundMgr instance = Instance;
		//Get the Audio source using the ID
		//I don't do a null check because if it working properly this wont happen.
		ACAudioSource acsource = instance._usedAudioSource [soundSourcehandle.Id];
		
		//Pause the sound
		acsource.Pause();
	}
	
	
	/// <summary>
	/// UnPause the sound source.
	/// </summary>
	/// <param name='sourceHandle'>
	/// Source handle.
	public static void UnPauseSoundSource (ACAudioSourceHandle soundSourcehandle)
	{
		
		if (soundSourcehandle == null) {
			Debug.LogError ("StopSoundSource Invalid Source handle");
			return;
		}
		
		//Get the instance
		ACSoundMgr instance = Instance;
		//Get the Audio source using the ID
		//I don't do a null check because if it working properly this wont happen.
		ACAudioSource acsource = instance._usedAudioSource [soundSourcehandle.Id];
		
		//Pause the sound
		acsource.UnPause();
	}
	
	
	
	//Temp Set sound pitch
	public static void SoundSourcePich (ACAudioSourceHandle soundSourcehandle,float pitch)
	{
		
		if (soundSourcehandle == null) {
			Debug.LogError ("StopSoundSource Invalid Source handle");
			return;
		}
		
		//Get the instance
		ACSoundMgr instance = Instance;
		//Get the Audio source using the ID
		//I don't do a null check because if it working properly this wont happen.
		ACAudioSource acsource = instance._usedAudioSource [soundSourcehandle.Id];
		
		acsource.Source.pitch =pitch;
	}
	
	
	
	
	/// <summary>
	/// Stops all sound source.
	/// </summary>
	/// <param name='fadeTimeInSecond'>
	/// Fade time in second.
	/// </param>
	public static void StopAllSoundSource (float fadeTimeInSecond = 0.0f)
	{
		
		//Get the instance
		ACSoundMgr instance = Instance;
		
		//Check the state off all playing source
		foreach (ACAudioSource acsource in instance._usedAudioSource.Values) {
			
			//If the source is set to stop with fadeOut 
			//Start the fadeOut and return
			if (fadeTimeInSecond > 0.0f) {
				acsource.FadeOut (fadeTimeInSecond);
				return;
			}
			
			//Remove the source from the used list
			//Don't do it here this is cleaned up in the update()
			//instance._usedAudioSource.Remove (soundSourcehandle.Id);
			
			//Stop the sound 
			acsource.Stop ();		
		}
	}
	#endregion
	
	#region Sound category parameter control
	/// <summary>
	/// _audio volume changed flag (Update all the active source when trigered)
	/// </summary>
	bool _audioVolumeChanged;
	
	/// <summary>
	/// The sound _is mute.
	/// </summary>
	bool _isMute;

	/// <summary>
	/// Sets the sound volume of a category of sound 
	/// </summary>
	/// <param name='categoryId'>
	/// Category identifier.
	/// </param>
	/// <param name='volume'>
	/// Volume.
	/// </param>
	public static void SetSoundCategoryVolume (int categoryId, float volume)
	{
		ACSoundMgr instance = Instance;
		ACAudioCategoryParameter category = instance.GetAudioCategoryParam (categoryId);
		if (category != null) {
			category.Volume = volume;
			instance._audioVolumeChanged = true;
		}
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="ACSoundMgr"/> is mute.
	/// </summary>
	/// <value>
	/// <c>true</c> if mute; otherwise, <c>false</c>.
	/// </value>
	public static bool IsMute {
		set { 
			Instance._isMute = value;
			Instance._audioVolumeChanged = true;
		}
		get {
			return Instance._isMute;
		}
	}
	#endregion
	
	#region Commun implementation.
	#region Audio Clip data
	/// <summary>
	/// SoundId / AudioClipResource map.
	/// </summary>
	private Dictionary<string, ACAudioClip> _soundClipDataHolder = new Dictionary<string, ACAudioClip> ();
	
	/// <summary>
	/// Gets an AudioClip from a given soundId.
	/// </summary>
	private ACAudioClip GetClip (string soundclipId)
	{
		ACAudioClip clip;
		if (_soundClipDataHolder.TryGetValue (soundclipId, out clip)) {
			return clip;
		}
		Debug.LogError ("soundclipId [" + soundclipId + "] not found.");
		return null;
	}
	
	#endregion
	
	#region Audio category parameter
	/// <summary>
	/// AudioCategory / AudioCategoryParamter map.
	/// </summary>
	private Dictionary<int, ACAudioCategoryParameter> _audioCategoryParamL = new Dictionary<int, ACAudioCategoryParameter> ();
	
	/// <summary>
	/// Gets The Audio Category
	/// </summary>
	private ACAudioCategoryParameter GetAudioCategoryParam (int categoryNum)
	{
		ACAudioCategoryParameter category;
		if (_audioCategoryParamL.TryGetValue (categoryNum, out category)) {
			return category;
		}
		return null;
	}
	#endregion
	
	#region Audio source
	/// <summary>
	/// Constant DEFAUL AUDIO SOURCE NUM.
	/// </summary>
	const int DEFAULT_AUDIO_SOURCE_NUM = 10;
	
	/// <summary>
	/// The _used audio source list
	/// </summary>
	private Dictionary<int, ACAudioSource> _usedAudioSource = new Dictionary<int, ACAudioSource> (DEFAULT_AUDIO_SOURCE_NUM);
	
	/// <summary>
	/// The _audio source to clean. 
	/// </summary>
	private Stack<int> _audioSourceToClean = new Stack<int> ();  
	#endregion
	#endregion

	#region Monobehavior
	protected override void Awake ()
	{
		// Only allow single instance to exist
		if (_instance != null && _instance != this) {
			Destroy (gameObject);
			return;
		}
		
		//Init
		_audioVolumeChanged = false;
		_isMute = false;
		
		Instance = this;
		DontDestroyOnLoad (gameObject);
	
		//Category Zero is alway the default category (0) -> TODO: I have to check how this work on stage load
		_audioCategoryParamL [0] = new ACAudioCategoryParameter ();
	}
	
	private void Update ()
	{
		//Debug.Log(_usedAudioSource.Count);
		
		//Check the state off all playing source
		foreach (ACAudioSource source in _usedAudioSource.Values) {
			//Check if the source still playing and remove it for active source if not.
			//This will happen when the source was a nonlooping sound fx clip.
			if (source.Source.isPlaying == false) {
				
				//Remove the source from the used list
				//_usedAudioSource.Remove (source.Id);
				_audioSourceToClean.Push (source.Id);
				
				//Stope the sound
				source.Stop ();
				
			} else {
		
				//Update the volume if needed
				if (_audioVolumeChanged) {
					//If the sound is mute set the volume to zero
					if (_isMute) {
						source.Volume = 0;
					} else {
						source.Volume = GetAudioCategoryParam (source.Clip.Category).Volume;
					}
				}

				//update the source clip
				source.Update ();
			}
		}
		
		//Clean up the unused source from the used source 
		while (_audioSourceToClean.Count>0) {
			int id = _audioSourceToClean.Pop ();
			_usedAudioSource.Remove (id);		
		}
		
		//Set this flat down every frame
		_audioVolumeChanged = false;
	}
	
	/// <summary>
	/// Destroy event.
	/// </summary>
	private void OnDestroy ()
	{
		Instance = null;
	}
	#endregion
	
	#region Singleton implementation.
	/// <summary>
	/// The _instance.
	/// </summary>
	private static ACSoundMgr _instance;
	
	/// <summary>
	/// Gets or sets the instance.
	/// </summary>
	/// <value>
	/// The instance.
	/// </value>
	public static ACSoundMgr Instance {
		get {
			if (_instance == null) {
				Debug.LogError ("YOU CALLED TO SOUND MANAGER BEFORE CREATE IT, YOU CANNOT CALL IT IN ANY AWAKE!!");
			}
			return _instance; 
		}
		private set { _instance = value;}
	}
	#endregion
}