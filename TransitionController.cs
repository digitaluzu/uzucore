using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Handles transition management.
	/// Register transitions (RegisterTransition) on application startup, and then
	/// execute transitions as needed (DoTransition).
	/// </summary>
	public class TransitionController : BaseBehaviour
	{				
		/// <summary>
		/// Registers a transition with the system.
		/// </summary>
		public void RegisterTransition (string transitionId, TransitionInterface transition)
		{
#if UNITY_EDITOR
			if (_transitions.ContainsKey(transitionId)) {
				Debug.LogWarning("Transition [" + transitionId + "] is already registered.");
				return;
			}
#endif // UNITY_EDITOR
			
			_transitions [transitionId] = transition;
		}

		public void DoTransition (string transitionId, System.Action onEndCallback = null)
		{
			// If a transition is already active, force it to end.
			if (_activeTransition != null) {
				EndActiveTransition ();
			}
			
			TransitionInterface transition;
			if (!_transitions.TryGetValue (transitionId, out transition)) {
				Debug.LogError ("Transition [" + transitionId + "] is not registered. Cannot execute transition.");
				return;
			}

			// Activate this transition.
			_activeTransition = transition;
			_activeTransitionOnEndCallback = onEndCallback;
			_activeTransition.OnReset ();
		}
		
		#region Implementation.
		private Dictionary<string, TransitionInterface> _transitions = new Dictionary<string, TransitionInterface> ();
		private TransitionInterface _activeTransition;
		private System.Action _activeTransitionOnEndCallback;
		
		private void Update ()
		{
			if (_activeTransition == null) {
				return;
			}
			
			// Update the currently active transition.
			bool isFinished = _activeTransition.OnUpdate ();
			if (isFinished) {
				EndActiveTransition ();
			}
		}
		
		private void EndActiveTransition ()
		{
			_activeTransition = null;
			
			// Trigger callback.
			{
				if (_activeTransitionOnEndCallback != null) {
					_activeTransitionOnEndCallback ();
				}				
				_activeTransitionOnEndCallback = null;
			}
		}
		#endregion
	}
}