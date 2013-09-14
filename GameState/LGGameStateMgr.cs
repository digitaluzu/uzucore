using UnityEngine;
using System.Collections;

public class LGGameStateMgr : MonoBehaviour
{
	/// <summary>
	/// Get the current state.
	/// </summary>
	public static LGIGameState CurrentState {
		get { return Instance._currentState; }
	}
	
	/// <summary>
	/// Get the previous state.
	/// </summary>
	public static LGIGameState PreviousState {
		get { return Instance._previousState; }
	}

	/// <summary>
	/// Change state immediately.
	/// </summary>
	public static void ChangeState (LGIGameState newState)
	{
		if (newState == null) {
			Debug.LogError ("Invalid state (null).");
			return;
		}
		
		LGGameStateMgr instance = Instance;
		if (newState == instance._currentState) {
			Debug.LogWarning ("Already in state [" + newState.StateName + "].");
			return;
		}
		
		if (instance._currentState != null) {
			Debug.Log("Exiting: " + instance._currentState.StateName);
			instance._currentState.OnExit ();
		}
		
		instance._previousState = instance._currentState;
		instance._currentState = newState;
		
		Debug.Log("Entering: " + instance._currentState.StateName);
		instance._currentState.OnEnter ();
	}

	#region Implementation.
	#region Singleton implementation.
	private static LGGameStateMgr _instance;

	private static LGGameStateMgr Instance {
		get {
			// Create on demand.
			if (_instance == null) {
				GameObject go = new GameObject ("LGGameStateMgr");
				_instance = go.AddComponent<LGGameStateMgr> ();
			}
			return _instance;
		}
		set { LGUtil.SingletonSet<LGGameStateMgr> (ref _instance, value); }
	}
	#endregion
	
	private LGIGameState _previousState;
	private LGIGameState _currentState;

	protected void Awake ()
	{
		// Only allow single instance to exist.
		if (_instance != null && _instance != this) {
			Destroy (gameObject);
			return;
		}
		
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	protected void Update ()
	{
		// Update the current state.
		if (_currentState != null) {
			_currentState.OnUpdate ();
		}
	}
	
	protected void OnDestroy ()
	{
		Instance = null;
	}
	#endregion
}
