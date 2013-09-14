using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Interface for all updateable objects that are managed by
/// the LGUpdater.
/// </summary>
public interface LGIUpdateable
{
	void OnUpdate ();
}

/// <summary>
/// Provides basic update functionality for registered objects.
/// </summary>
public class LGUpdater : MonoBehaviour
{	
	/// <summary>
	/// Registers an object to be updated every frame.
	/// </summary>
	static public void RegisterForUpdates (LGIUpdateable updateable)
	{
		LGUpdater instance = Instance;
		
#if DEBUG
		// Duplicate check.
		foreach (WeakReference weakRef in instance._updateables) {
			LGIUpdateable u = weakRef.Target as LGIUpdateable;
			if (u == updateable) {
				Debug.LogWarning ("LGIUpdateable[" + updateable + "] already registered.");
				return;
			}
		}
#endif
		
		instance._updateables.Add (new WeakReference (updateable));
	}
	
	#region Implementation.
	#region Singleton implementation.
	private static LGUpdater _instance;

	private static LGUpdater Instance {
		get {
			// Create on demand.
			if (_instance == null) {
				GameObject go = new GameObject ("LGUpdater");
				_instance = go.AddComponent<LGUpdater> ();
			}
			return _instance;
		}
		set { LGUtil.SingletonSet<LGUpdater> (ref _instance, value); }
	}
	#endregion
	
	/// <summary>
	/// List of all the updateable objects.
	/// We use weak references so that updateable objects don't need
	/// to worry about unregistering.
	/// </summary>
	private List<WeakReference> _updateables = new List<WeakReference> ();
	
	protected void Awake ()
	{
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}
	
	protected void Update ()
	{
		// Update events.
		for (int i = 0; i < _updateables.Count; /*++i*/) {
			LGIUpdateable u = _updateables [i].Target as LGIUpdateable;
			if (u == null) {
				_updateables.RemoveAt (i);
			} else {
				u.OnUpdate ();
				i++;
			}
		}
	}
	#endregion
}