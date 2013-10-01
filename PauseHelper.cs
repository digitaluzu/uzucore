using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Allows generic pausing through string-based tags.
	/// By using tags, it allows us to have certain functionality
	/// paused, while other functionality continues to update.
	/// 
	/// Example:
	///   Uzu.PauseHelper pauseObject = new Uzu.PauseHelper();
	/// 
	///   // ...
	/// 
	///	  // From GUI button:
	///   pauseObject.Pause("BackgroundLayer");
	/// 
	///   // ...
	/// 
	///   // From background layer (Update()):
	///   if (pauseObject.IsPaused("BackgroundLayer")) {
	///        return;
	///   }
	///   
	/// </summary>
	public class PauseHelper
	{
		/// <summary>
		/// Pause the specified tag.
		/// </summary>
		public void Pause (string tag)
		{
			if (!_pauseTags.Contains (tag)) {
				_pauseTags.Add (tag);
			}
		}
		
		/// <summary>
		/// Unpause the specified tag.
		/// </summary>
		public void Unpause (string tag)
		{
			_pauseTags.Remove (tag);
		}
		
		/// <summary>
		/// Determines whether the specified tag is paused.
		/// </summary>
		public bool IsPaused (string tag)
		{
			return _pauseTags.Contains (tag);
		}
		
		#region Implementation.
		private HashSet<string> _pauseTags = new HashSet<string> ();
		#endregion
	}
}