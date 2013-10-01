using UnityEngine;
using System.Collections;

namespace Uzu
{
	/// <summary>
	/// Monitors 'touch' input events.
	/// Many of the touch event phases supplied by Unity are
	/// not reliable, as they may or may not be fired depending on the
	/// frame rate.
	/// </summary>
	public class TouchObserver
	{
		public delegate void OnTouchBeginDelegate (Touch touch);
	
		public delegate void OnTouchUpdateDelegate (Touch touch);
	
		public delegate void OnTouchEndDelegate (int fingerId);
	
		public OnTouchBeginDelegate OnTouchBegin { get; set; }
	
		public OnTouchUpdateDelegate OnTouchUpdate { get; set; }
	
		public OnTouchEndDelegate OnTouchEnd { get; set; }
		
		/// <summary>
		/// Creates a new TouchObserver.
		/// maxTouchCount # of touches will be monitored.
		/// Any touches over this number will be ignored.
		/// </summary>
		public TouchObserver (int maxTouchCount)
		{
			MAX_TOUCH_TRACKER_COUNT = maxTouchCount;
			
			// Allocate.
			{
				_trackers = new TouchTracker[MAX_TOUCH_TRACKER_COUNT];
				for (int i = 0; i < MAX_TOUCH_TRACKER_COUNT; i++) {
					_trackers [i] = new TouchTracker ();
				}
			}
		}
		
		/// <summary>
		/// Manually clear all currently active touches.
		/// </summary>
		public void ClearTouches ()
		{
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (tracker.IsActive) {
					DoTrackingEnd (tracker);
				}
			}
		}
		
		/// <summary>
		/// Updates the states of all touches.
		/// Call once per frame.
		/// </summary>
		public void Update ()
		{
			// Clear all active trackers dirty state.
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (tracker.IsActive) {
					tracker.IsDirty = false;
				}
			}
			
			// Process all touches...
			int touchCount = Input.touches.Length;
			for (int i = 0; i < touchCount; i++) {
				Touch touch = Input.touches [i];
				
				// Are we already tracking this finger?
				TouchTracker tracker = GetExistingTracker (touch.fingerId);
				
				if (touch.phase == TouchPhase.Began) {
					// If the tracker for this finger is already existing,
					// but the touch phase is just beginning, then this is
					// a different touch event. End the previous event, and detect
					// a new event.
					if (tracker != null) {
						DoTrackingEnd (tracker);
						tracker = null;
					}
					
					// New finger detected - start tracking if possible.
					if (tracker == null) {				
						tracker = GetNewTracker ();
						if (tracker != null) {
							DoTrackingBegin (tracker, touch);
						}
					}
				}
				
				// Update the tracker.
				if (tracker != null) {
					tracker.Update ();
					if (OnTouchUpdate != null) {
						OnTouchUpdate (touch);
					}
				}
			}
			
			// Deactivate all active trackers that weren't updated this frame so they can be re-used.
			// Ideally, TouchPhase.Ended could be used to do this, but it is not reliable.
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (tracker.IsActive && !tracker.IsDirty) {
					DoTrackingEnd (tracker);
				}
			}
		}
		
		#region Implementation.
		private readonly int MAX_TOUCH_TRACKER_COUNT;
		private TouchTracker[] _trackers;
		
		/// <summary>
		/// Tracks a single touch.
		/// </summary>
		private class TouchTracker
		{	
			private int _fingerId;
			private bool _isActive = false;
			private bool _isDirty = false;
			
			public int FingerId {
				get { return _fingerId; }
			}
			
			public bool IsActive {
				get { return _isActive; }
			}
			
			public bool IsDirty {
				get { return _isDirty; }
				set { _isDirty = value; }
			}
			
			public void BeginTracking (int fingerId)
			{
				_isActive = true;
				_fingerId = fingerId;
			}
			
			public void Update ()
			{
				_isDirty = true;
			}
			
			public void EndTracking ()
			{
				_isActive = false;
			}
		}
	
		private void DoTrackingBegin (TouchTracker tracker, Touch touch)
		{
			tracker.BeginTracking (touch.fingerId);
			if (OnTouchBegin != null) {
				OnTouchBegin (touch);
			}
		}
		
		private void DoTrackingEnd (TouchTracker tracker)
		{
			if (OnTouchEnd != null) {
				OnTouchEnd (tracker.FingerId);
			}
			tracker.EndTracking ();
		}
		
		private TouchTracker GetExistingTracker (int fingerId)
		{
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (tracker.IsActive && tracker.FingerId == fingerId) {
					return tracker;
				}
			}
			
			return null;
		}
		
		private TouchTracker GetNewTracker ()
		{
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (!tracker.IsActive) {
					return tracker;
				}
			}
			
			return null;
		}
		#endregion
	}
}