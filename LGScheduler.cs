using UnityEngine;
using System.Collections.Generic;

public class LGScheduler : MonoBehaviour
{
	public delegate void ScheduledWork ();

	/// <summary>
	/// Adds "work" to the scheduler to be executed at some
	/// time in the future.
	/// 
	/// Example usage:
	/// 	// Prints a message in 3 seconds:
	/// 	LGScheduler.AddWork(
	/// 					3.0f,
	/// 					() => { 
	///							Debug.Log("Hello World!");
	///						});
	/// </summary>
	public static void AddWork (float delayInSeconds, ScheduledWork scheduledWork)
	{
		if (delayInSeconds <= 0.0f) {
			Debug.LogWarning ("Work must be scheduled for sometime in the future.");
			return;
		}
		
		ScheduledWorkImpl work = new ScheduledWorkImpl ();
		work._delayInSeconds = delayInSeconds;
		work._timeElapsedInSeconds = 0.0f;
		work._theWork = scheduledWork;
		
		Instance._workList.Add (work);
	}

	#region Implementation.
	#region Singleton implementation.
	private static LGScheduler _instance;
	public static LGScheduler Instance {
		get {
			// Create on demand.
			if (_instance == null) {
				GameObject go = new GameObject ("LGScheduler");
				_instance = go.AddComponent<LGScheduler> ();
			}
			return _instance;
		}
		private set { LGUtil.SingletonSet<LGScheduler> (ref _instance, value); }
	}
	#endregion

	private class ScheduledWorkImpl
	{
		public float _delayInSeconds;
		public float _timeElapsedInSeconds;
		public ScheduledWork _theWork;
	}

	private List<ScheduledWorkImpl> _workList = new List<ScheduledWorkImpl> ();

	protected virtual void Awake ()
	{
		// Only allow single instance to exist.
		if (_instance != null && _instance != this) {
			Destroy (gameObject);
			return;
		}
		
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	protected virtual void Update ()
	{
		// Process all work.
		for (int i = 0; i < _workList.Count;) {
			ScheduledWorkImpl work = _workList[i];
			
			// Progress the work's timer.
			work._timeElapsedInSeconds += Time.deltaTime;
			
			if (work._timeElapsedInSeconds >= work._delayInSeconds) {
				// Execute and remove.
				work._theWork ();
				_workList.RemoveAt (i);
			} else {
				// Next.
				i++;
			}
		}
	}
	#endregion
}
