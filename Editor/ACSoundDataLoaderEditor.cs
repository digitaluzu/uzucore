using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ACSoundDataLoader))] 
public class ACSoundDataLoaderEditor : Editor
{
	
	/// <summary>
	/// The _target.
	/// </summary>
	ACSoundDataLoader _target;
	
	/// <summary>
	/// Enable event.
	/// </summary>
	public void OnEnable ()
	{
		_target = (ACSoundDataLoader)target;
	}
	
	/// <summary>
	/// GU event.
	/// </summary>
	public override void OnInspectorGUI ()
	{
		
		//Top Label
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Clip Id");
		EditorGUILayout.LabelField ("Audio Clip");
		EditorGUILayout.LabelField ("Audio Type");
		EditorGUILayout.EndHorizontal ();
		
		//List of sound clip data
		for (int i=0; i< _target._AudioClip.Count; ++i) {
			EditorGUILayout.BeginHorizontal ();
			_target._AudioClipNameId [i] = EditorGUILayout.TextField (_target._AudioClipNameId [i]);
			_target._AudioClip [i] = (AudioClip)EditorGUILayout.ObjectField (_target._AudioClip [i], typeof(AudioClip), false, null);
			_target._AudioClipType [i] = (int)(object)EditorGUILayout.EnumPopup ((ACSoundDataLoader.SoundType)_target._AudioClipType [i]);
				
			if (_target._AudioClip [i] == null) {
				_target._AudioClip.RemoveAt (i);
				_target._AudioClipType.RemoveAt (i);
				_target._AudioClipNameId.RemoveAt (i);
			}
			EditorGUILayout.EndHorizontal ();
		}
		
		EditorGUILayout.BeginHorizontal ();
		AudioClip clip = (AudioClip)EditorGUILayout.ObjectField (null, typeof(AudioClip), false, null);
		if (clip != null) {
			_target._AudioClip.Add (clip);
			_target._AudioClipNameId.Add (clip.name);
			_target._AudioClipType.Add (0);
		}
			
		EditorGUILayout.EndHorizontal ();
		if (_target._AudioClip.Count > 0) {
			EditorGUILayout.HelpBox ("The sound type is used to differentiate and control the sound clip parameters in grounp (Volume)", MessageType.Info, true);
			EditorGUILayout.HelpBox ("The sound clip Id is usualy the name of the clip but you can change it. It also what you have to use in the programe to call it", MessageType.Info, true);
		}
	}
}

