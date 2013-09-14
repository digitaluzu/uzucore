using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// NOTE: the priority of execution of this script should be higher that average to prevent the sound to be call before
/// been loaded
/// 
/// AC sound data loader.
/// 
/// What for?
/// We need to separate the data and the sound manager,
/// But I still prefere to drag and drop data in the editor rather than
/// write every sound file I want to use in a program file.
/// This dataLoad take care of this.
/// 
/// How to;
/// Create a game object.
/// Add this script as a component and add all the sound clip
/// you want to use in you stage to it.
/// You can have multyple dataLoader in the same scene. 
/// this can be usefull the create generique sound data prefab.
/// </summary>
public class ACSoundDataLoader : MonoBehaviour
{
	
	/// <summary>
	/// Sound type. (Can add new one if needed) 
	/// </summary>
	public enum SoundType
	{
		BackgroundMusic,
		SoundEffect,
		Voice,
	}

	/// <summary>
	/// The _ audio clip list
	/// </summary>
	public List<AudioClip> _AudioClip = new List<AudioClip> ();
	
	/// <summary>
	/// The _ audio clip name identifier. (this is what you have to call from the programe)
	/// I need a separate list for serialize purpose ad change the name directly on the audioclip won't 
	/// work correctly
	/// </summary>
	public List<string> _AudioClipNameId = new List<string> ();
	
	/// <summary>
	/// The type of the _ audio clip list
	/// </summary>
	public List<int> _AudioClipType = new List<int> ();

	/// <summary>
	/// Start Load all the clip to the sound manager 
	/// </summary>
	void Start ()
	{
		for (int i=0; i<_AudioClip.Count; ++i) {	
			ACSoundMgr.RegisterAudioClip (_AudioClipNameId [i], _AudioClip [i], _AudioClipType [i]);
		}
		
		//We don't need this gameobject once the data is loader
		//set it the inactif.
		this.gameObject.SetActive (false);
	}
	
}
