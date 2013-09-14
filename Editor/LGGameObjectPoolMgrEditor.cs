using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LGGameObjectPoolMgr))]
public class LGGameObjectPoolMgrEditor : Editor {
	/// <summary>
	/// Constant DEFAU l_ OBJEC t_ ALOCATIO n_ NU.
	/// </summary>
	private const int DEFAULT_OBJECT_ALLOCATION_NUM = 1;
		
	/// <summary>
	/// The _target.
	/// </summary>
	LGGameObjectPoolMgr _target;
	bool _warningOnSameNameObject;

	
	/// <summary>
	/// Enable event.
	/// </summary>
	public void OnEnable ()
	{
		_target = (LGGameObjectPoolMgr)target;
		_warningOnSameNameObject = false;
		//Show existing window instance. If one doesn't exist, make one.

	}
	
	/// <summary>
	/// GU event.
	/// </summary>
	public override void OnInspectorGUI ()
	{
		//int count  = EditorGUILayout.IntField (_target._poolEntries.Count);
		EditorGUILayout.HelpBox ("Pool list\n" +
			"Set the pool size to zero to delete an existing pool ", MessageType.None, true);
		
		
		//TODO now working ..Can't figure out how to set the size properly yet!!!
		GUIStyle myButtonStyle  = new GUIStyle();
		//myButtonStyle.border 

		//Top Label
		EditorGUILayout.BeginHorizontal ();
	
		EditorGUILayout.TextField ("Pool name",myButtonStyle); 
		EditorGUILayout.TextField ("Pooled Object",myButtonStyle);
		EditorGUILayout.TextField ("Size of the pool",myButtonStyle);
		
		EditorGUILayout.EndHorizontal ();
		
		//List of sound clip data
		for (int i=0; i< _target._poolEntries.Count; ++i) {
			EditorGUILayout.BeginHorizontal ();
			_target._poolEntries[i].name = EditorGUILayout.TextField (_target._poolEntries[i].name);
			_target._poolEntries[i].gameObject = (GameObject)EditorGUILayout.ObjectField (_target._poolEntries[i].gameObject, typeof(GameObject), false, null);
			_target._poolEntries[i].count = (int)(object)EditorGUILayout.IntField (_target._poolEntries[i].count);
				
			if (_target._poolEntries[i].count == 0) {
				_target._poolEntries.RemoveAt (i);
			}
			EditorGUILayout.EndHorizontal ();
		}
		
		//int count  = EditorGUILayout.IntField (_target._poolEntries.Count);
		EditorGUILayout.HelpBox ("Drag an object to create a new pool.", MessageType.Info, true);
		
		EditorGUILayout.BeginHorizontal ();
		GameObject obj = (GameObject)EditorGUILayout.ObjectField (null, typeof(GameObject), false, null);
		if (obj != null) {
			
			//Reset the duplication warning 
			_warningOnSameNameObject = false;
			
			// Check for duplication.
			for (int i = 0; i < _target._poolEntries.Count; ++i) {
				// Check for duplication.
				if (_target._poolEntries[i].name == obj.name) {
					_warningOnSameNameObject = true;
					//Debug.LogError ("Pool [" + "] is registered twice.");
					continue;
				}
			}
			
			//If not duplicate add the object and create a new pool
			if (!_warningOnSameNameObject) {
				LGGameObjectPoolMgr.PoolEntry entry = new LGGameObjectPoolMgr.PoolEntry(obj,DEFAULT_OBJECT_ALLOCATION_NUM);
				_target._poolEntries.Add(entry);
			}			
		}
			
		EditorGUILayout.EndHorizontal ();
		if (_warningOnSameNameObject) {
			EditorGUILayout.HelpBox ("You cannot register an object with the name of an existing pool, " +
				"change the existing pool name or the name of the object", MessageType.Error, true);
		}
		
		//*/
	}
}
