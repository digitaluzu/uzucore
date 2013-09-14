using UnityEngine;

/// <summary>
/// Base message class for all message types.
/// </summary>
public abstract class UzuMessage
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

/// <summary>
/// Message subscriber interface.
/// Implement this interface to receive messages.
/// </summary>
public interface UzuIMessageSubscriber
{
	/// <summary>
	/// Receive a message and process it.
	/// </summary>
	void OnReceiveMessage (UzuMessage message);
}

/// <summary>
/// Message dispatcher interface.
/// Implement this interface to become a message hub, and process
/// message dispatching.
/// </summary>
public interface UzuIMessageDispatcher
{
	/// <summary>
	/// Subscribe to messages of a certain type T.
	/// </summary>
	void AddSubscriber<T> (UzuIMessageSubscriber subscriber);

	/// <summary>
	/// Send a message.
	/// </summary>
	void SendMessage (UzuMessage message);
}
