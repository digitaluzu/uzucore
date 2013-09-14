using UnityEngine;

/// <summary>
/// Base message class for all message types.
/// </summary>
public abstract class LGMessage
{
	/// <summary>
	/// If a message requires a receiver and no one receives
	/// the message, an error will be displayed.
	/// </summary>
	public bool DoesRequireReceiver {
		get { return _doesRequireReceiver; }
		set { _doesRequireReceiver = value; }
	}

	#region Implementation.
	private bool _doesRequireReceiver = true;
	#endregion
}
