using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class for pooling GameObjects.
/// 
/// Based loosely on the description here:
/// http://vonlehecreative.wordpress.com/2010/01/06/unity-resource-gameobjectpool/
/// </summary>
public class LGGameObjectPool
{
	private GameObject _prefab;
	private Transform _prefabTransform;
	private Stack<GameObject> _availableObjects;
	private List<GameObject> _allObjects;
	
	public int ActiveObjectCount {
		get { return _allObjects.Count - _availableObjects.Count; }
	}
	
	public List<GameObject> ActiveObjects {
		get {
			List<GameObject> activeObjects = new List<GameObject> (ActiveObjectCount);
			foreach (GameObject go in _allObjects) {
				if (!_availableObjects.Contains (go)) {
					activeObjects.Add (go);
				}
			}
			return activeObjects;
		}
	}
	
	public LGGameObjectPool (GameObject prefab, int initialCapacity)
	{
		_prefab = prefab;
		_prefabTransform = _prefab.transform;
		
		_availableObjects = new Stack<GameObject> (initialCapacity);
		_allObjects = new List<GameObject> (initialCapacity);
		
		// Pre-allocate our objects.
		for (int i = 0; i < initialCapacity; ++i) {
			Spawn (Vector3.zero);
		}
		UnspawnAll ();
	}
		
	/// <summary>
	/// Spawn a new GameObject with the given position.
	/// Re-uses an object in the pool if available.
	/// </summary>
	public GameObject Spawn (Vector3 position)
	{
		return Spawn (position, _prefabTransform.localRotation);
	}
	
	/// <summary>
	/// Spawn a new GameObject with the given position/rotation.
	/// Re-uses an object in the pool if available.
	/// 
	/// Triggers message OnSpawned( LGGameObjectPool ) on instantiation
	/// of GameObject.
	/// </summary>
	public GameObject Spawn (Vector3 position, Quaternion rotation)
	{
		GameObject result;
		
		if (_availableObjects.Count == 0) {
			result = GameObject.Instantiate (_prefab, position, rotation) as GameObject;
			
			Transform resultTransform = result.transform;
			resultTransform.parent = _prefabTransform.parent;
			resultTransform.localPosition = position;
			resultTransform.localRotation = rotation;
			resultTransform.localScale = _prefabTransform.localScale;
			
			_allObjects.Add (result);
			
			SetActive (result, true);
			
			result.SendMessage ("OnSpawned", this, SendMessageOptions.DontRequireReceiver);
		} else {
			result = _availableObjects.Pop ();
			
			Transform resultTransform = result.transform;
			resultTransform.localPosition = position;
			resultTransform.localRotation = rotation;
			
			SetActive (result, true);
		}
		
		return result;
	}
	
	/// <summary>
	/// Unspawns a given GameObject and adds it back to the available
	/// resource pool.
	/// </summary>
	public void Unspawn (GameObject target)
	{
#if DEBUG
		if (!_allObjects.Contains(target)) {
			Debug.LogError("Attempting to Unspawn an object not belonging to this pool!");
			return;
		}
#endif
		
		if (!_availableObjects.Contains (target)) {
			_availableObjects.Push (target);
			SetActive (target, false);
		}
	}
	
	/// <summary>
	/// Unspawns all GameObjects and adds them all back to the available
	/// resource pool.
	/// </summary>
	public void UnspawnAll ()
	{
		foreach (GameObject go in _allObjects) {
			Unspawn (go);
		}
	}
	
	/// <summary>
	/// Destroys all GameObjects in the pool.
	/// </summary>
	public void DestroyAll ()
	{
		foreach (GameObject go in _allObjects) {
			GameObject.Destroy (go);
		}
		
		_availableObjects.Clear ();
		_allObjects.Clear ();
	}
	
	#region Implementation.
	/// <summary>
	/// Toggles the "active" state of the target GameObject.
	/// </summary>
	private void SetActive (GameObject target, bool state)
	{
		target.SetActive(state);
	}	
	#endregion
}