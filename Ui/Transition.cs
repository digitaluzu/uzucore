namespace Uzu
{
	/// <summary>
	/// Base interface for transitions.
	/// </summary>
	public interface TransitionInterface
	{
		/// <summary>
		/// Called when the transition is activated, before update.
		/// Allows the transition to reset to it's default state.
		/// This allows us to re-use the same transition object multiple
		/// times instead of re-allocating each time.
		/// </summary>
		void OnReset ();
		
		/// <summary>
		/// Handles the transition logic.
		/// Called once per frame.
		/// Return true if transition update is finished.
		/// </summary>
		bool OnUpdate ();
	}
}