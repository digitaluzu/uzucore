using UnityEngine;
using System.Collections;

namespace Uzu
{
	/// <summary>
	/// Allows regulation of the update frequency of code.
	/// </summary>
	public class Regulator
	{
		private Timer _timer = new Timer ();
		public float _updatePeriod;
		
		/// <summary>
		/// Get/Set the TimeMultiplier.
		/// </summary>
		public float TimeMultiplier {
			get {
				return _timer.TimeMultiplier;
			}
			set {
				_timer.TimeMultiplier = value;
			}
		}
		
		/// <summary>
		/// Regulator creation.
		/// </summary>
		/// <param name="numUpdatesPerSecond">
		/// Number of desired updates per second.
		/// </param>
		public Regulator (float numUpdatesPerSecond)
		{
			SetUpdatePeriod (numUpdatesPerSecond);
			Restart ();
		}
		
		/// <summary>
		/// Sets the desired number of updates per second, overriding the value
		/// initial value set on object creation.
		/// 
		/// Restart() must be explicitly called in order to restart the internal timer.
		/// </summary>
		/// <param name="numUpdatesPerSecond">
		/// Number of desired updates per second.
		/// </param>
		public void SetUpdatePeriod (float numUpdatesPerSecond)
		{
			if (numUpdatesPerSecond <= 0) {
				_updatePeriod = 0;
			} else {
				_updatePeriod = 1.0f / numUpdatesPerSecond;
			}
		}
	
		/// <summary>
		/// Has the desired amount of time elapsed?
		/// If ready, implicitly restarts the next update time to be
		/// the current time plus the update interval.
		/// </summary>
		/// <returns>
		/// True if ready to execute, false if time has not yet elapsed.
		/// </returns>
		public bool IsReady ()
		{
			if (_timer.ElapsedTimeInSeconds >= _updatePeriod) {
				Restart ();
				return true;
			}
	
			return false;
		}
		
		/// <summary>
		/// Has the desired amount of time elapsed?
		/// Does not implicitly restart the next update time.
		/// Restart() must be explicitly called for the time interval to be
		/// restarted.
		/// </summary>
		/// <returns>
		/// True if ready to execute, false if time has not yet elapsed.
		/// </returns>
		public bool IsReadyPeek ()
		{
			if (_timer.ElapsedTimeInSeconds >= _updatePeriod) {
				return true;
			}
	
			return false;
		}
	
		/// <summary>
		/// Restart the regulator to the beginning of it's waiting period.
		/// </summary>
		public void Restart ()
		{
			_timer.Restart ();
			
			// Trigger callback.
			OnRestart ();
		}
		
		#region Implementation.
		protected virtual void OnRestart ()
		{
		}
		#endregion
	}
	
	/// <summary>
	/// Allows regulation of the update frequency of code
	/// within some min/max range.
	/// </summary>
	public class RangedRegulator : Regulator
	{
		public RangedRegulator (float numUpdatesPerSecondMin, float numUpdatesPerSecondMax)
			:base(Random.Range(numUpdatesPerSecondMin, numUpdatesPerSecondMax))
		{
			_minUpdatesPerSecond = numUpdatesPerSecondMin;
			_maxUpdatesPerSecond = numUpdatesPerSecondMax;
		}
		
		#region Implementation.
		private float _minUpdatesPerSecond;
		private float _maxUpdatesPerSecond;
		
		protected override void OnRestart ()
		{
			SetUpdatePeriod (Random.Range (_minUpdatesPerSecond, _maxUpdatesPerSecond));		
		}
		#endregion
	}
}