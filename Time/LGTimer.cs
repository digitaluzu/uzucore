using UnityEngine;
using System;

/// <summary>
/// Timer class.
/// Updated by the Unity game loop using Time.
/// Because of this, retrieving the elapsed time from a LGTimer will
/// always be the same within a given frame.
/// Supports time multipliers.
/// </summary>
public class LGTimer : LGIUpdateable
{
	public LGTimer ()
	{
		// Register event for updates.
		LGUpdater.RegisterForUpdates (this);
	
		Restart ();
	}
	
	/// <summary>
	/// Restarts the timer.
	/// </summary>
	public void Restart ()
	{
		_startTimeInSeconds = 0.0f;
		_currentTimeInSeconds = 0.0f;
	}
	
	/// <summary>
	/// Get the amount of time elapsed in seconds.
	/// </summary>
	public float ElapsedTimeInSeconds {
		get { return _currentTimeInSeconds - _startTimeInSeconds; }
	}
	
	/// <summary>
	/// Get/Set the time multiplier.
	/// </summary>
	public float TimeMultiplier {
		get {
			return _timeMultiplier;
		}
		set {
			// Cannot be negative.
			_timeMultiplier = Mathf.Max (0.0f, value);
		}
	}
	
	#region Implementation.
	private float _startTimeInSeconds;
	private float _currentTimeInSeconds;
	private float _timeMultiplier = 1.0f;
	
	/// <summary>
	/// Perform time updates.
	/// </summary>
	public void OnUpdate ()
	{
		_currentTimeInSeconds += Time.deltaTime * _timeMultiplier;
	}
	#endregion
}

/// <summary>
/// Timer class that utilizes the System.DataTime.
/// Useful for profiling code and other tasks where "precise" time is necessary.
/// </summary>
public class LGSystemTimer
{
	private long _startTicks;
	
	public LGSystemTimer ()
	{
		Restart ();
	}
	
	/// <summary>
	/// Restarts the timer.
	/// </summary>
	public void Restart ()
	{
		_startTicks = System.DateTime.Now.Ticks;
	}
	
	/// <summary>
	/// Get the amount of time elapsed in ticks.
	/// </summary>
	public long ElapsedTimeInTicks {
		get {
			return new System.TimeSpan (System.DateTime.Now.Ticks - _startTicks).Ticks;
		}
	}
	
	/// <summary>
	/// Get the amount of time elapsed in milliseconds.
	/// </summary>
	public double ElapsedTimeInMs {
		get {
			return new System.TimeSpan (System.DateTime.Now.Ticks - _startTicks).TotalMilliseconds;
		}
	}
	
	/// <summary>
	/// Get the amount of time elapsed in seconds.
	/// </summary>
	public double ElapsedTimeInSeconds {
		get {
			return new System.TimeSpan (System.DateTime.Now.Ticks - _startTicks).TotalSeconds;
		}
	}
}

/// <summary>
/// Allows profiling of a block of code w/ timer result automatically called on exit of scope.
/// 
/// Usage:
/// 	using (LGScopedSystemTimer timer = new LGScopedSystemTimer("Log message"))
/// 	{
/// 		// ~ code for profiling ~
///		}
/// </summary>
public struct LGScopedSystemTimer : IDisposable
{
	private LGSystemTimer _timer;
	private string _logMessage;
	
	public LGScopedSystemTimer (string logMessage)
	{
		_timer = new LGSystemTimer ();
		_logMessage = logMessage;
	}
	
	#region IDisposable interface.
	public void Dispose ()
	{
		Debug.Log (_logMessage + ": " + _timer.ElapsedTimeInMs + "(ms)");
	}
	#endregion
}